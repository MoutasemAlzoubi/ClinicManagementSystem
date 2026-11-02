using Clinic_Management_System.Data;
using Clinic_Management_System.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Service
{
    public class DoctorService : IDoctorService
    {
        private readonly ClinicContext context;
        private readonly UserManager<ApplicationUser> users;

        public DoctorService(ClinicContext _context, UserManager<ApplicationUser> _users)
        {
            context = _context;
            users = _users;
        }

        public List<DoctorDTO> GetAllDoctors()
        {
            List<Doctor> doctors = context.doctors.Include("User").ToList();
            List<DoctorDTO> doctorDTOs = new List<DoctorDTO>();
            foreach (Doctor doctor in doctors)
            {
                DoctorDTO doctorDTO = new DoctorDTO();
                doctorDTO.Id = doctor.Id;
                doctorDTO.FirstName = doctor.FirstName;
                doctorDTO.LastName = doctor.LastName;
                doctorDTO.Email = doctor.User.Email;
                doctorDTO.Specialization = doctor.Specialization;
                doctorDTO.Bio = doctor.Bio;
                doctorDTO.IsActive = doctor.IsActive;
                doctorDTOs.Add(doctorDTO);
            }
            return doctorDTOs;
        }

        public DoctorDTO GetDoctor(int id)
        {
            Doctor doctor = context.doctors.Include("User").Where(d => d.Id == id).FirstOrDefault();
            if (doctor == null) return null;
            return ConvertDoctor(doctor);
        }

        public DoctorDTO GetMyDoctor(string userId)
        {
            Doctor doctor = context.doctors.Include("User").Where(d => d.UserId == userId).FirstOrDefault();
            if (doctor == null) return null;
            return ConvertDoctor(doctor);
        }

        private DoctorDTO ConvertDoctor(Doctor doctor)
        {
            DoctorDTO doctorDTO = new DoctorDTO();
            doctorDTO.Id = doctor.Id;
            doctorDTO.FirstName = doctor.FirstName;
            doctorDTO.LastName = doctor.LastName;
            doctorDTO.Email = doctor.User == null ? "" : doctor.User.Email;
            doctorDTO.Specialization = doctor.Specialization;
            doctorDTO.Bio = doctor.Bio;
            doctorDTO.IsActive = doctor.IsActive;
            return doctorDTO;
        }

        public async Task<(bool Success, string Error)> CreateDoctor(CreateDoctorDTO doctorDTO)
        {
            if (await users.FindByEmailAsync(doctorDTO.Email) != null) return (false, "Email is already registered.");

            ApplicationUser user = new ApplicationUser();
            user.UserName = doctorDTO.Email;
            user.Email = doctorDTO.Email;
            user.EmailConfirmed = true;
            user.FirstName = doctorDTO.FirstName;
            user.LastName = doctorDTO.LastName;

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
            var result = await users.CreateAsync(user, doctorDTO.TemporaryPassword);
            if (!result.Succeeded) { await transaction.RollbackAsync(); return (false, string.Join(" ", result.Errors.Select(e => e.Description))); }

            var roleResult = await users.AddToRoleAsync(user, "Doctor");
            if (!roleResult.Succeeded)
            {
                await transaction.RollbackAsync();
                return (false, "Could not assign Doctor role.");
            }

            Doctor doctor = new Doctor();
            doctor.FirstName = doctorDTO.FirstName;
            doctor.LastName = doctorDTO.LastName;
            doctor.Specialization = doctorDTO.Specialization;
            doctor.Bio = doctorDTO.Bio;
            doctor.UserId = user.Id;
            context.doctors.Add(doctor);
            context.SaveChanges();
            await transaction.CommitAsync();
            }
            return (true, "");
        }

        public void UpdateDoctor(DoctorDTO doctorDTO)
        {
            Doctor doctor = context.doctors.Include("User").Where(d => d.Id == doctorDTO.Id).FirstOrDefault();
            if (doctor == null) return;
            doctor.FirstName = doctorDTO.FirstName;
            doctor.LastName = doctorDTO.LastName;
            doctor.Specialization = doctorDTO.Specialization;
            doctor.Bio = doctorDTO.Bio;
            doctor.IsActive = doctorDTO.IsActive;
            if (doctor.User != null)
            {
                doctor.User.FirstName = doctorDTO.FirstName;
                doctor.User.LastName = doctorDTO.LastName;
            }
            context.SaveChanges();
        }

        public bool DeleteDoctor(int id)
        {
            Doctor doctor = context.doctors.Include("User").Where(d => d.Id == id).FirstOrDefault();
            if (doctor == null) return false;
            if (context.appointments.Any(a => a.DoctorId == id) || context.medicalRecords.Any(r => r.DoctorId == id) || context.prescriptions.Any(p => p.DoctorId == id)) return false;
            context.doctors.Remove(doctor);
            if (doctor.User != null) context.Users.Remove(doctor.User);
            context.SaveChanges();
            return true;
        }

        public List<PatientNameDTO> GetMyPatients(string userId)
        {
            Doctor doctor = context.doctors.Where(d => d.UserId == userId).FirstOrDefault();
            if (doctor == null) return new List<PatientNameDTO>();
            List<Patient> patients = context.appointments.Where(a => a.DoctorId == doctor.Id && a.Status != "Cancelled").Select(a => a.patient).Distinct().ToList();
            List<PatientNameDTO> patientDTOs = new List<PatientNameDTO>();
            foreach (Patient patient in patients)
            {
                PatientNameDTO patientDTO = new PatientNameDTO();
                patientDTO.Id = patient.Id;
                patientDTO.FirstName = patient.FirstName;
                patientDTO.LastName = patient.LastName;
                patientDTOs.Add(patientDTO);
            }
            return patientDTOs;
        }
    }
}
