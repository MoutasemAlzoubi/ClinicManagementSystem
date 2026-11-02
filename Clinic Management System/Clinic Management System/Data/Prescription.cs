using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Data
{
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor doctor { get; set; }
        public int? MedicalRecordId { get; set; }
        public MedicalRecords medicalRecord { get; set; }

        [Required]
        [StringLength(150)]
        public string MedicationName { get; set; }

        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? Duration { get; set; }
        public string? Instructions { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.Now;
    }
}
