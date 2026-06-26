namespace HospitalManagementSystem.Models
{
    public class MedicalRecord
    {
        public int RecordID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = "";
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = "";
        public string Diagnosis { get; set; } = "";
        public string Treatment { get; set; } = "";
        public DateTime RecordDate { get; set; }
    }
}