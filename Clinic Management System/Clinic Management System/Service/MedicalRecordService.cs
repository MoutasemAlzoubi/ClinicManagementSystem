using Clinic_Management_System.Data;
using Clinic_Management_System.Model;
using Microsoft.EntityFrameworkCore;

namespace Clinic_Management_System.Service
{
    public class MedicalRecordsService : IMedicalRecordsService
    {
        private readonly ClinicContext context;
        public MedicalRecordsService(ClinicContext _context) { context = _context; }

        public List<MedicalRecordsDTO> GetAllMedicalRecords() { return ConvertRecords(context.medicalRecords.Include("doctor").Include("patient").ToList()); }
        public List<MedicalRecordsDTO> GetPatientRecords(int patientId) { return ConvertRecords(context.medicalRecords.Include("doctor").Include("patient").Where(r => r.PatientId == patientId).ToList()); }
        public List<MedicalRecordsDTO> GetDoctorRecords(int doctorId) { return ConvertRecords(context.medicalRecords.Include("doctor").Include("patient").Where(r => r.DoctorId == doctorId).ToList()); }

        private List<MedicalRecordsDTO> ConvertRecords(List<MedicalRecords> records)
        {
            List<MedicalRecordsDTO> recordDTOs = new List<MedicalRecordsDTO>();
            foreach (MedicalRecords record in records)
            {
                MedicalRecordsDTO recordDTO = new MedicalRecordsDTO();
                recordDTO.Id = record.Id;
                recordDTO.RecordDate = record.RecordDate;
                recordDTO.Observation = record.Observation;
                recordDTO.Diagnosis = record.Diagnosis;
                recordDTO.Treatment = record.Treatment;
                recordDTO.Notes = record.Notes;
                recordDTO.DoctorId = record.DoctorId;
                recordDTO.DoctorName = record.doctor.FirstName + " " + record.doctor.LastName;
                recordDTO.PatientId = record.PatientId;
                recordDTO.PatientName = record.patient.FirstName + " " + record.patient.LastName;
                recordDTOs.Add(recordDTO);
            }
            return recordDTOs;
        }

        public bool SaveMedicalRecord(MedicalRecordsDTO medicalRecordsDTO, int doctorId)
        {
            if (!context.patients.Any(p => p.Id == medicalRecordsDTO.PatientId)) return false;
            if (!context.appointments.Any(a => a.PatientId == medicalRecordsDTO.PatientId && a.DoctorId == doctorId && a.Status != "Cancelled")) return false;
            MedicalRecords medicalRecord = new MedicalRecords();
            medicalRecord.RecordDate = medicalRecordsDTO.RecordDate == default ? DateTime.UtcNow : medicalRecordsDTO.RecordDate;
            medicalRecord.Observation = medicalRecordsDTO.Observation;
            medicalRecord.Diagnosis = medicalRecordsDTO.Diagnosis;
            medicalRecord.Treatment = medicalRecordsDTO.Treatment;
            medicalRecord.Notes = medicalRecordsDTO.Notes;
            medicalRecord.PatientId = medicalRecordsDTO.PatientId;
            medicalRecord.DoctorId = doctorId;
            context.medicalRecords.Add(medicalRecord);
            context.SaveChanges();
            return true;
        }

        public bool UpdateMedicalRecord(MedicalRecordsDTO medicalRecordsDTO, int doctorId)
        {
            MedicalRecords medicalRecord = context.medicalRecords.Where(r => r.Id == medicalRecordsDTO.Id && r.DoctorId == doctorId).FirstOrDefault();
            if (medicalRecord == null) return false;
            medicalRecord.Observation = medicalRecordsDTO.Observation;
            medicalRecord.Diagnosis = medicalRecordsDTO.Diagnosis;
            medicalRecord.Treatment = medicalRecordsDTO.Treatment;
            medicalRecord.Notes = medicalRecordsDTO.Notes;
            context.SaveChanges();
            return true;
        }

        public bool DeleteMedicalRecord(int id)
        {
            MedicalRecords medicalRecord = context.medicalRecords.Find(id);
            if (medicalRecord == null) return false;
            context.medicalRecords.Remove(medicalRecord);
            context.SaveChanges();
            return true;
        }
    }
}
