using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface IDoctorService
    {
        List<DoctorDTO> GetAllDoctors();
        DoctorDTO GetDoctor(int id);
        DoctorDTO GetMyDoctor(string userId);
        Task<(bool Success, string Error)> CreateDoctor(CreateDoctorDTO doctorDTO);
        void UpdateDoctor(DoctorDTO doctorDTO);
        bool DeleteDoctor(int id);
        List<PatientNameDTO> GetMyPatients(string userId);
    }
}
