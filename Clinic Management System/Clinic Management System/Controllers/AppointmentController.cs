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
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService appointmentService;
        private readonly IPatientService patientService;
        private readonly IDoctorService doctorService;
        public AppointmentController(IAppointmentService _appointmentService, IPatientService _patientService, IDoctorService _doctorService)
        {
            appointmentService = _appointmentService; patientService = _patientService; doctorService = _doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<AppointmentDTO> GetAll() { return appointmentService.GetAll(); }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public IActionResult Book(CreateAppointmentDTO appointmentDTO)
        {
            PatientDTO patientDTO = patientService.GetMyPatient(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patientDTO == null) return NotFound(new { message = "Patient profile not found." });
            string error;
            if (!appointmentService.Create(patientDTO.Id, appointmentDTO, out error)) return BadRequest(new { message = error });
            return Ok(new { message = "Appointment request submitted." });
        }

        [HttpGet("my-appointments")]
        [Authorize(Roles = "Patient")]
        public IActionResult PatientAppointments()
        {
            PatientDTO patientDTO = patientService.GetMyPatient(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patientDTO == null) return NotFound();
            return Ok(appointmentService.GetForPatient(patientDTO.Id));
        }

        [HttpGet("doctor-appointments")]
        [Authorize(Roles = "Doctor")]
        public IActionResult DoctorAppointments()
        {
            DoctorDTO doctorDTO = doctorService.GetMyDoctor(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (doctorDTO == null) return NotFound();
            return Ok(appointmentService.GetForDoctor(doctorDTO.Id));
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        public IActionResult Status(int id, UpdateAppointmentStatusDTO appointmentDTO)
        {
            if (!appointmentService.UpdateStatus(id, appointmentDTO.Status)) return BadRequest(new { message = "Invalid appointment or status." });
            return NoContent();
        }

        [HttpPut("{id:int}/cancel")]
        [Authorize(Roles = "Patient")]
        public IActionResult Cancel(int id)
        {
            PatientDTO patientDTO = patientService.GetMyPatient(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (patientDTO == null || !appointmentService.Cancel(id, patientDTO.Id)) return NotFound();
            return NoContent();
        }
    }
}
