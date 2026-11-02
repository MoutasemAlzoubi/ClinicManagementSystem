using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic_Management_System.Data
{
    public class MedicalRecords
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime RecordDate { get; set; }

        [Column(TypeName = "varchar(2000)")]
        public string Observation { get; set; }

        [Column(TypeName = "varchar(1000)")]
        public string? Diagnosis { get; set; }

        [Column(TypeName = "varchar(2000)")]
        public string? Treatment { get; set; }

        [Column(TypeName = "varchar(2000)")]
        public string? Notes { get; set; }

        public int DoctorId { get; set; }
        public Doctor doctor { get; set; }
        public int PatientId { get; set; }
        public Patient patient { get; set; }
    }
}
