namespace HospitalManagementSystem.Models
{
    public class Appointment
    {
        public int AppointmentID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = "";
        public int DoctorID { get; set; }
        public string DoctorName { get; set; } = "";
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Scheduled";
        public string Notes { get; set; } = "";
    }
}