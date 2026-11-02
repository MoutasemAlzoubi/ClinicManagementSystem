using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        public AccountController(IAccountService _accountService) { accountService = _accountService; }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await accountService.Login(loginDTO);
            if (!result.Success) return Unauthorized(new { message = result.Error });
            return Ok(result.Data);
        }

        [HttpPost("register-patient")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPatient(RegisterPatientDTO patientDTO)
        {
            var result = await accountService.RegisterPatient(patientDTO);
            if (!result.Success) return BadRequest(new { message = result.Error });
            return Ok(new { message = "Patient account created successfully." });
        }
    }
}
