using System.ComponentModel.DataAnnotations;
namespace Clinic_Management_System.Model
{
    public class CreateAppointmentDTO
    {
        [Required] public int DoctorId { get; set; }
        [Required] public DateTime StartDateTime { get; set; }
        [Required] public DateTime EndDateTime { get; set; }
        public string? Notes { get; set; }
    }
}
