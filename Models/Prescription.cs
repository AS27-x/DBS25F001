namespace HospitalManagementSystem.Models
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int RecordID { get; set; }
        public int MedicineID { get; set; }
        public string MedicineName { get; set; } = "";
        public string Dosage { get; set; } = "";
        public string Duration { get; set; } = "";
        public int Quantity { get; set; }
    }
}