using Clinic_Management_System.Data;
using Clinic_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Service
{
    public class PatientService : IPatientService
    {
        private readonly ClinicContext context;

        public PatientService(ClinicContext _context)
        {
            context = _context;
        }

        public bool SavePatient(PatientDTO patientDTO)
        {
            if (context.Countries.FirstOrDefault(c => c.Id == patientDTO.CountryId) == null) return false;
            if (context.patients.Any(p => p.Email == patientDTO.Email)) return false;

            Patient patient = new Patient();
            patient.FirstName = patientDTO.FirstName;
            patient.LastName = patientDTO.LastName;
            patient.Email = patientDTO.Email;
            patient.DOB = patientDTO.DOB;
            patient.Gender = patientDTO.Gender;
            patient.Phone = patientDTO.Phone;
            patient.Address = patientDTO.Address;
            patient.CountryId = patientDTO.CountryId;
            context.patients.Add(patient);
            context.SaveChanges();
            return true;
        }

        public List<PatientDTO> GetAllPatient()
        {
            List<Patient> patients = context.patients.Include("country").ToList();
            List<PatientDTO> patientDTOs = new List<PatientDTO>();

            foreach (Patient patient in patients)
            {
                PatientDTO patientDTO = new PatientDTO();
                patientDTO.Id = patient.Id;
                patientDTO.FirstName = patient.FirstName;
                patientDTO.LastName = patient.LastName;
                patientDTO.DOB = patient.DOB;
                patientDTO.Gender = patient.Gender;
                patientDTO.Phone = patient.Phone;
                patientDTO.Address = patient.Address;
                patientDTO.Email = patient.Email;
                patientDTO.CountryId = patient.CountryId;
                patientDTO.CountryName = patient.country.Name;
                patientDTO.UserId = patient.UserId;
                patientDTOs.Add(patientDTO);
            }
            return patientDTOs;
        }

        public PatientDTO EditPatient(int patientId)
        {
            Patient patient = context.patients.Include("country").Where(p => p.Id == patientId).FirstOrDefault();
            if (patient == null) return null;

            PatientDTO patientDTO = new PatientDTO();
            patientDTO.Id = patient.Id;
            patientDTO.FirstName = patient.FirstName;
            patientDTO.LastName = patient.LastName;
            patientDTO.DOB = patient.DOB;
            patientDTO.Gender = patient.Gender;
            patientDTO.Address = patient.Address;
            patientDTO.Email = patient.Email;
            patientDTO.CountryId = patient.CountryId;
            patientDTO.CountryName = patient.country.Name;
            patientDTO.Phone = patient.Phone;
            patientDTO.UserId = patient.UserId;
            return patientDTO;
        }

        public PatientDTO GetMyPatient(string userId)
        {
            Patient patient = context.patients.Include("country").Where(p => p.UserId == userId).FirstOrDefault();
            if (patient == null) return null;
            return EditPatient(patient.Id);
        }

        public bool UpdatePatient(PatientDTO patientDTO)
        {
            Patient patient = context.patients.Find(patientDTO.Id);
            if (patient == null) return false;
            if (patient.UserId != null && patient.Email != patientDTO.Email) return false;
            if (context.patients.Any(p => p.Email == patientDTO.Email && p.Id != patientDTO.Id)) return false;
            if (!context.Countries.Any(c => c.Id == patientDTO.CountryId)) return false;
            patient.FirstName = patientDTO.FirstName;
            patient.LastName = patientDTO.LastName;
            patient.Email = patientDTO.Email;
            patient.DOB = patientDTO.DOB;
            patient.Gender = patientDTO.Gender;
            patient.Phone = patientDTO.Phone;
            patient.Address = patientDTO.Address;
            patient.CountryId = patientDTO.CountryId;
            context.SaveChanges();
            return true;
        }

        public bool DeletePatient(int id)
        {
            Patient patient = context.patients.Include("User").Where(p => p.Id == id).FirstOrDefault();
            if (patient == null) return false;
            if (context.medicalRecords.Any(r => r.PatientId == id) || context.appointments.Any(a => a.PatientId == id) || context.prescriptions.Any(p => p.PatientId == id)) return false;
            context.patients.Remove(patient);
            if (patient.User != null) context.Users.Remove(patient.User);
            context.SaveChanges();
            return true;
        }
    }
}
