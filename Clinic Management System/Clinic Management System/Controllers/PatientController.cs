using System.Security.Claims;
using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService patientService;
        public PatientController(IPatientService _patientService) { patientService = _patientService; }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult SavePatient(PatientDTO patientDTO)
        {
            if (!patientService.SavePatient(patientDTO)) return BadRequest(new { message = "Email is already used or country is invalid." });
            return Ok();
        }

        [HttpGet]
        [Route("GetThePatients")]
        [Authorize(Roles = "Admin")]
        public List<PatientDTO> GetThePatients() { return patientService.GetAllPatient(); }

        [HttpGet]
        [Route("GetThePatient")]
        [Authorize(Roles = "Admin")]
        public ActionResult<PatientDTO> EditThePatient(int PatientId)
        {
            PatientDTO patientDTO = patientService.EditPatient(PatientId);
            if (patientDTO == null) return NotFound();
            return patientDTO;
        }

        [HttpGet]
        [Route("my-profile")]
        [Authorize(Roles = "Patient")]
        public ActionResult<PatientDTO> MyProfile()
        {
            PatientDTO patientDTO = patientService.GetMyPatient(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patientDTO == null) return NotFound();
            return patientDTO;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdatePatient(PatientDTO patientDTO)
        {
            if (patientService.EditPatient(patientDTO.Id) == null) return NotFound();
            if (!patientService.UpdatePatient(patientDTO)) return BadRequest(new { message = "Patient details are invalid." });
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePatient(int id)
        {
            if (!patientService.DeletePatient(id)) return Conflict(new { message = "Patient not found or has clinical history. Archive instead of deleting." });
            return NoContent();
        }
    }
}
