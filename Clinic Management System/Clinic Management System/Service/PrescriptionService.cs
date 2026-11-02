using Clinic_Management_System.Data;
using Clinic_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Service
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly ClinicContext context;
        public PrescriptionService(ClinicContext _context) { context = _context; }

        public List<PrescriptionDTO> GetForPatient(int patientId) { return ConvertPrescriptions(context.prescriptions.Include("patient").Include("doctor").Where(p => p.PatientId == patientId).ToList()); }
        public List<PrescriptionDTO> GetAll() { return ConvertPrescriptions(context.prescriptions.Include("patient").Include("doctor").ToList()); }

        private List<PrescriptionDTO> ConvertPrescriptions(List<Prescription> prescriptions)
        {
            List<PrescriptionDTO> prescriptionDTOs = new List<PrescriptionDTO>();
            foreach (Prescription prescription in prescriptions)
            {
                PrescriptionDTO prescriptionDTO = new PrescriptionDTO();
                prescriptionDTO.Id = prescription.Id;
                prescriptionDTO.PatientId = prescription.PatientId;
                prescriptionDTO.DoctorId = prescription.DoctorId;
                prescriptionDTO.MedicalRecordId = prescription.MedicalRecordId;
                prescriptionDTO.MedicationName = prescription.MedicationName;
                prescriptionDTO.Dosage = prescription.Dosage;
                prescriptionDTO.Frequency = prescription.Frequency;
                prescriptionDTO.Duration = prescription.Duration;
                prescriptionDTO.Instructions = prescription.Instructions;
                prescriptionDTO.IssuedAt = prescription.IssuedAt;
                prescriptionDTO.PatientName = prescription.patient.FirstName + " " + prescription.patient.LastName;
                prescriptionDTO.DoctorName = prescription.doctor.FirstName + " " + prescription.doctor.LastName;
                prescriptionDTOs.Add(prescriptionDTO);
            }
            return prescriptionDTOs;
        }

        public bool Create(PrescriptionDTO prescriptionDTO, int doctorId)
        {
            if (!context.patients.Any(p => p.Id == prescriptionDTO.PatientId)) return false;
            if (!context.appointments.Any(a => a.PatientId == prescriptionDTO.PatientId && a.DoctorId == doctorId && a.Status != "Cancelled")) return false;
            if (prescriptionDTO.MedicalRecordId.HasValue && !context.medicalRecords.Any(r => r.Id == prescriptionDTO.MedicalRecordId && r.PatientId == prescriptionDTO.PatientId && r.DoctorId == doctorId)) return false;
            Prescription prescription = new Prescription();
            prescription.PatientId = prescriptionDTO.PatientId;
            prescription.DoctorId = doctorId;
            prescription.MedicalRecordId = prescriptionDTO.MedicalRecordId;
            prescription.MedicationName = prescriptionDTO.MedicationName;
            prescription.Dosage = prescriptionDTO.Dosage;
            prescription.Frequency = prescriptionDTO.Frequency;
            prescription.Duration = prescriptionDTO.Duration;
            prescription.Instructions = prescriptionDTO.Instructions;
            context.prescriptions.Add(prescription);
            context.SaveChanges();
            return true;
        }
    }
}
