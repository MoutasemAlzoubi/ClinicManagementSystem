using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface IPatientService
    {
        bool SavePatient(PatientDTO patientDTO);
        List<PatientDTO> GetAllPatient();
        PatientDTO EditPatient(int patientId);
        bool UpdatePatient(PatientDTO patientDTO);
        bool DeletePatient(int id);
        PatientDTO GetMyPatient(string userId);
    }
}
