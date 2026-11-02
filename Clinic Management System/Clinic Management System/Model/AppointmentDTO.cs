using System.ComponentModel.DataAnnotations;

namespace Clinic_Management_System.Model
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string? PatientName { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
    }

}
