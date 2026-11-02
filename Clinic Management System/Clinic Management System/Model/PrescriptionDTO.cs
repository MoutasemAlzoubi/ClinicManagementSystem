using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Model
{
    public class PrescriptionDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int? MedicalRecordId { get; set; }

        [Required]
        public string MedicationName { get; set; }

        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }
        public DateTime IssuedAt { get; set; }
        public string? PatientName { get; set; }
        public string? DoctorName { get; set; }
    }
}
