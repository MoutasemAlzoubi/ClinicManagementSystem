using Clinic_Management_System.Model;
namespace Clinic_Management_System.Service
{
    public interface ICountryService
    {
        bool AddCountry(CountryDTO countryDTO);
        List<CountryDTO> GetAllCountry();
        CountryDTO EditTheCountry(int id);
        bool UpdateTheCountry(CountryDTO countryDTO);
        bool DeleteTheCountry(int id);
    }
}
