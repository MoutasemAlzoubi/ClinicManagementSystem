using Clinic_Management_System.Model;
using Clinic_Management_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService countryService;
        public CountryController(ICountryService _countryService) { countryService = _countryService; }

        [HttpGet]
        [Route("LoadAll")]
        [AllowAnonymous]
        public List<CountryDTO> ListOfCountries() { return countryService.GetAllCountry(); }

        [HttpGet]
        [Route("LoadCountry")]
        [Authorize(Roles = "Admin")]
        public ActionResult<CountryDTO> EditCountry(int countryId)
        {
            CountryDTO countryDTO = countryService.EditTheCountry(countryId);
            if (countryDTO == null) return NotFound();
            return countryDTO;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Save(CountryDTO countryDTO)
        {
            if (!countryService.AddCountry(countryDTO)) return Conflict(new { message = "Country already exists." });
            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCountry(CountryDTO countryDTO)
        {
            if (countryService.EditTheCountry(countryDTO.Id) == null) return NotFound();
            if (!countryService.UpdateTheCountry(countryDTO)) return Conflict(new { message = "Country name already exists." });
            return NoContent();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCountry(int Id)
        {
            if (countryService.EditTheCountry(Id) == null) return NotFound();
            if (!countryService.DeleteTheCountry(Id)) return Conflict(new { message = "Country has patients and cannot be deleted." });
            return NoContent();
        }
    }
}
