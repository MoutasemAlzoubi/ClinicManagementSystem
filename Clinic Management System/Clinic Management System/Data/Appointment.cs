using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Data
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient patient { get; set; }
        public int DoctorId { get; set; }
        public Doctor doctor { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Pending";

        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
