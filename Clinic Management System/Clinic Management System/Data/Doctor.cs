using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_System.Data
{
    public class Doctor
    {
        public int Id { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Specialization { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Bio { get; set; }

        public bool IsActive { get; set; } = true;
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<MedicalRecords> MedicalRecords { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Prescription> Prescriptions { get; set; }
    }
}
