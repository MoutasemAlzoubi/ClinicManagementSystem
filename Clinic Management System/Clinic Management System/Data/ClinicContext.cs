using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Data
{
    public class ClinicContext : IdentityDbContext<ApplicationUser>
    {
        public ClinicContext(DbContextOptions<ClinicContext> options) : base(options)
        {
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<MedicalRecords> medicalRecords { get; set; }
        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Prescription> prescriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Patient)
                .WithOne(p => p.User)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Doctor)
                .WithOne(d => d.User)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Patient>().HasIndex(p => p.UserId).IsUnique().HasFilter("[UserId] IS NOT NULL");
            modelBuilder.Entity<Patient>().HasIndex(p => p.Email).IsUnique();
            modelBuilder.Entity<Doctor>().HasIndex(d => d.UserId).IsUnique();
            modelBuilder.Entity<Country>().HasMany(c => c.patients).WithOne(p => p.country).HasForeignKey(p => p.CountryId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MedicalRecords>().HasOne(r => r.patient).WithMany(p => p.MedicalRecords).HasForeignKey(r => r.PatientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MedicalRecords>().HasOne(r => r.doctor).WithMany(d => d.MedicalRecords).HasForeignKey(r => r.DoctorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>().HasOne(a => a.patient).WithMany(p => p.Appointments).HasForeignKey(a => a.PatientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Appointment>().HasOne(a => a.doctor).WithMany(d => d.Appointments).HasForeignKey(a => a.DoctorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription>().HasOne(p => p.patient).WithMany(x => x.Prescriptions).HasForeignKey(p => p.PatientId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription>().HasOne(p => p.doctor).WithMany(x => x.Prescriptions).HasForeignKey(p => p.DoctorId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Prescription>().HasOne(p => p.medicalRecord).WithMany().HasForeignKey(p => p.MedicalRecordId).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<Appointment>().HasIndex(a => new { a.DoctorId, a.StartDateTime, a.EndDateTime });
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        }
    }
}
