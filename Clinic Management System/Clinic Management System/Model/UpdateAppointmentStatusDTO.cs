using System.ComponentModel.DataAnnotations;
namespace Clinic_Management_System.Model
{
    public class UpdateAppointmentStatusDTO
    {
        [Required] public string Status { get; set; }
    }
}
