using System.Security.Claims;
using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService doctorService;
        public DoctorController(IDoctorService _doctorService) { doctorService = _doctorService; }

        [HttpGet]
        [AllowAnonymous]
        public List<object> GetAll()
        {
            List<DoctorDTO> doctors = doctorService.GetAllDoctors().Where(d => d.IsActive).ToList();
            List<object> result = new List<object>();
            foreach (DoctorDTO doctor in doctors) result.Add(new { doctor.Id, doctor.FirstName, doctor.LastName, doctor.Specialization, doctor.Bio });
            return result;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public List<DoctorDTO> GetAllForAdmin() { return doctorService.GetAllDoctors(); }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateDoctorDTO doctorDTO)
        {
            var result = await doctorService.CreateDoctor(doctorDTO);
            if (!result.Success) return BadRequest(new { message = result.Error });
            return Ok(new { message = "Doctor account created." });
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(DoctorDTO doctorDTO)
        {
            if (doctorService.GetDoctor(doctorDTO.Id) == null) return NotFound();
            doctorService.UpdateDoctor(doctorDTO);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            if (!doctorService.DeleteDoctor(id)) return Conflict(new { message = "Doctor not found or has clinical history. Deactivate the account instead." });
            return NoContent();
        }

        [HttpGet("my-patients")]
        [Authorize(Roles = "Doctor")]
        public List<PatientNameDTO> MyPatients() { return doctorService.GetMyPatients(User.FindFirstValue(ClaimTypes.NameIdentifier)); }

        [HttpGet("my-profile")]
        [Authorize(Roles = "Doctor")]
        public ActionResult<DoctorDTO> MyProfile()
        {
            DoctorDTO doctorDTO = doctorService.GetMyDoctor(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (doctorDTO == null) return NotFound();
            return doctorDTO;
        }
    }
}
