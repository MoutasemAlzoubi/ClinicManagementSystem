using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface IAppointmentService
    {
        bool Create(int patientId, CreateAppointmentDTO appointmentDTO, out string error);
        List<AppointmentDTO> GetAll();
        List<AppointmentDTO> GetForPatient(int patientId);
        List<AppointmentDTO> GetForDoctor(int doctorId);
        bool UpdateStatus(int id, string status);
        bool Cancel(int id, int patientId);
    }
}
