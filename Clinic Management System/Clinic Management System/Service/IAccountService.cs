using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface IAccountService
    {
        Task<(bool Success, string Error, LoginResponseDTO Data)> Login(LoginDTO loginDTO);
        Task<(bool Success, string Error)> RegisterPatient(RegisterPatientDTO patientDTO);
    }
}
