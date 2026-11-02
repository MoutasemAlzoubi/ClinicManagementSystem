using Clinic_Management_System.Data;
using Clinic_Management_System.Model;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Clinic_Management_System.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ClinicContext context;

        public AppointmentService(ClinicContext _context)
        {
            context = _context;
        }

        public bool Create(int patientId, CreateAppointmentDTO appointmentDTO, out string error)
        {
            error = "";
            if (appointmentDTO.EndDateTime <= appointmentDTO.StartDateTime) { error = "End time must be after start time."; return false; }
            if (appointmentDTO.StartDateTime <= DateTime.UtcNow) { error = "Appointment must be in the future."; return false; }
            if (!context.doctors.Any(d => d.Id == appointmentDTO.DoctorId && d.IsActive)) { error = "Doctor not found or inactive."; return false; }

            using (var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                bool overlap = context.appointments.Any(a => a.DoctorId == appointmentDTO.DoctorId && a.Status != "Cancelled" && appointmentDTO.StartDateTime < a.EndDateTime && appointmentDTO.EndDateTime > a.StartDateTime);
                if (overlap) { transaction.Rollback(); error = "This doctor already has an appointment during the selected time."; return false; }

                Appointment appointment = new Appointment();
                appointment.PatientId = patientId;
                appointment.DoctorId = appointmentDTO.DoctorId;
                appointment.StartDateTime = appointmentDTO.StartDateTime;
                appointment.EndDateTime = appointmentDTO.EndDateTime;
                appointment.Notes = appointmentDTO.Notes;
                appointment.Status = "Pending";
                context.appointments.Add(appointment);
                context.SaveChanges();
                transaction.Commit();
            }
            return true;
        }

        public List<AppointmentDTO> GetAll() { return ConvertAppointments(context.appointments.Include("patient").Include("doctor").ToList()); }
        public List<AppointmentDTO> GetForPatient(int patientId) { return ConvertAppointments(context.appointments.Include("patient").Include("doctor").Where(a => a.PatientId == patientId).ToList()); }
        public List<AppointmentDTO> GetForDoctor(int doctorId) { return ConvertAppointments(context.appointments.Include("patient").Include("doctor").Where(a => a.DoctorId == doctorId).ToList()); }

        private List<AppointmentDTO> ConvertAppointments(List<Appointment> appointments)
        {
            List<AppointmentDTO> appointmentDTOs = new List<AppointmentDTO>();
            foreach (Appointment appointment in appointments)
            {
                AppointmentDTO appointmentDTO = new AppointmentDTO();
                appointmentDTO.Id = appointment.Id;
                appointmentDTO.PatientId = appointment.PatientId;
                appointmentDTO.PatientName = appointment.patient.FirstName + " " + appointment.patient.LastName;
                appointmentDTO.DoctorId = appointment.DoctorId;
                appointmentDTO.DoctorName = appointment.doctor.FirstName + " " + appointment.doctor.LastName;
                appointmentDTO.StartDateTime = appointment.StartDateTime;
                appointmentDTO.EndDateTime = appointment.EndDateTime;
                appointmentDTO.Status = appointment.Status;
                appointmentDTO.Notes = appointment.Notes;
                appointmentDTOs.Add(appointmentDTO);
            }
            return appointmentDTOs;
        }

        public bool UpdateStatus(int id, string status)
        {
            if (status != "Pending" && status != "Confirmed" && status != "Cancelled" && status != "Completed") return false;
            Appointment appointment = context.appointments.Find(id);
            if (appointment == null) return false;
            appointment.Status = status;
            context.SaveChanges();
            return true;
        }

        public bool Cancel(int id, int patientId)
        {
            Appointment appointment = context.appointments.Where(a => a.Id == id && a.PatientId == patientId).FirstOrDefault();
            if (appointment == null || appointment.Status == "Completed" || appointment.Status == "Cancelled") return false;
            appointment.Status = "Cancelled";
            context.SaveChanges();
            return true;
        }
    }
}
