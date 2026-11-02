using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface IMedicalRecordsService
    {
        List<MedicalRecordsDTO> GetAllMedicalRecords();
        List<MedicalRecordsDTO> GetPatientRecords(int patientId);
        List<MedicalRecordsDTO> GetDoctorRecords(int doctorId);
        bool SaveMedicalRecord(MedicalRecordsDTO medicalRecordsDTO, int doctorId);
        bool UpdateMedicalRecord(MedicalRecordsDTO medicalRecordsDTO, int doctorId);
        bool DeleteMedicalRecord(int id);
    }
}
