using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface IPrescriptionService
    {
        List<PrescriptionDTO> GetForPatient(int patientId);
        List<PrescriptionDTO> GetAll();
        bool Create(PrescriptionDTO prescriptionDTO, int doctorId);
    }
}
