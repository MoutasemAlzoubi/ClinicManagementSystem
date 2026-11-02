using Clinic_Management_System.Data;
using Clinic_Management_System.Model;

namespace Clinic_Management_System.Service
{
    public class CountryService : ICountryService
    {
        private readonly ClinicContext context;

        public CountryService(ClinicContext _context)
        {
            context = _context;
        }

        public bool AddCountry(CountryDTO countryDTO)
        {
            if (context.Countries.Any(c => c.Name == countryDTO.Name)) return false;
            Country country = new Country();
            country.Name = countryDTO.Name;
            context.Countries.Add(country);
            context.SaveChanges();
            return true;
        }

        public List<CountryDTO> GetAllCountry()
        {
            List<Country> countries = context.Countries.OrderBy(c => c.Name).ToList();
            List<CountryDTO> countryDTOs = new List<CountryDTO>();

            foreach (Country country in countries)
            {
                CountryDTO countryDTO = new CountryDTO();
                countryDTO.Id = country.Id;
                countryDTO.Name = country.Name;
                countryDTOs.Add(countryDTO);
            }
            return countryDTOs;
        }

        public CountryDTO EditTheCountry(int id)
        {
            Country country = context.Countries.Where(c => c.Id == id).FirstOrDefault();
            if (country == null) return null;

            CountryDTO countryDTO = new CountryDTO();
            countryDTO.Id = country.Id;
            countryDTO.Name = country.Name;
            return countryDTO;
        }

        public bool UpdateTheCountry(CountryDTO countryDTO)
        {
            Country country = context.Countries.Where(c => c.Id == countryDTO.Id).FirstOrDefault();
            if (country == null) return false;
            if (context.Countries.Any(c => c.Name == countryDTO.Name && c.Id != countryDTO.Id)) return false;
            country.Name = countryDTO.Name;
            context.SaveChanges();
            return true;
        }

        public bool DeleteTheCountry(int id)
        {
            Country country = context.Countries.Where(c => c.Id == id).FirstOrDefault();
            if (country == null) return false;
            if (context.patients.Any(p => p.CountryId == id)) return false;
            context.Countries.Remove(country);
            context.SaveChanges();
            return true;
        }
    }
}
