using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Model
{
    public class MedicalRecordsDTO
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }

        [Required]
        public string Observation { get; set; }

        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Notes { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
    }
}
