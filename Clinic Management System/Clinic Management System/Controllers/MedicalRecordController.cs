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
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordsService medicalRecordsService;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        public MedicalRecordController(IMedicalRecordsService _medicalRecordsService, IPatientService _patientService, IDoctorService _doctorService)
        {
            medicalRecordsService = _medicalRecordsService; patientService = _patientService; doctorService = _doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<MedicalRecordsDTO> GetAll() { return medicalRecordsService.GetAllMedicalRecords(); }

        [HttpGet("my-records")]
        [Authorize(Roles = "Patient")]
        public IActionResult MyRecords()
        {
            PatientDTO patientDTO = patientService.GetMyPatient(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patientDTO == null) return NotFound();
            return Ok(medicalRecordsService.GetPatientRecords(patientDTO.Id));
        }

        [HttpGet("doctor-records")]
        [Authorize(Roles = "Doctor")]
        public IActionResult DoctorRecords()
        {
            DoctorDTO doctorDTO = doctorService.GetMyDoctor(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (doctorDTO == null) return NotFound();
            return Ok(medicalRecordsService.GetDoctorRecords(doctorDTO.Id));
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public IActionResult Create(MedicalRecordsDTO medicalRecordsDTO)
        {
            DoctorDTO doctorDTO = doctorService.GetMyDoctor(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (doctorDTO == null) return Forbid();
            if (!medicalRecordsService.SaveMedicalRecord(medicalRecordsDTO, doctorDTO.Id)) return BadRequest(new { message = "A valid appointment with this patient is required." });
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Doctor")]
        public IActionResult Update(MedicalRecordsDTO medicalRecordsDTO)
        {
            DoctorDTO doctorDTO = doctorService.GetMyDoctor(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (doctorDTO == null || !medicalRecordsService.UpdateMedicalRecord(medicalRecordsDTO, doctorDTO.Id)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id) { return medicalRecordsService.DeleteMedicalRecord(id) ? NoContent() : NotFound(); }
    }
}
