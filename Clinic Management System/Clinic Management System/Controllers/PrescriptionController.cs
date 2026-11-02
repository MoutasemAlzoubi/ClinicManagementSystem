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
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService prescriptionService;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        public PrescriptionController(IPrescriptionService _prescriptionService, IPatientService _patientService, IDoctorService _doctorService)
        {
            prescriptionService = _prescriptionService; patientService = _patientService; doctorService = _doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<PrescriptionDTO> GetAll() { return prescriptionService.GetAll(); }

        [HttpGet("my-prescriptions")]
        [Authorize(Roles = "Patient")]
        public IActionResult Mine()
        {
            PatientDTO patientDTO = patientService.GetMyPatient(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patientDTO == null) return NotFound();
            return Ok(prescriptionService.GetForPatient(patientDTO.Id));
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public IActionResult Create(PrescriptionDTO prescriptionDTO)
        {
            DoctorDTO doctorDTO = doctorService.GetMyDoctor(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (doctorDTO == null) return Forbid();
            if (!prescriptionService.Create(prescriptionDTO, doctorDTO.Id)) return BadRequest(new { message = "Patient, appointment or medical record is invalid." });
            return Ok();
        }
    }
}
