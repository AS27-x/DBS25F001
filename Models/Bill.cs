namespace HospitalManagementSystem.Models
{
    public class Bill
    {
        public int BillID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; } = "";
        public int AdmissionID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Balance => TotalAmount - PaidAmount;
        public DateTime BillDate { get; set; }
        public string Status { get; set; } = "Unpaid";
    }
}