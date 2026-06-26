namespace HospitalManagementSystem.Models
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Specialization { get; set; } = "";
        public int DeptID { get; set; }
        public string DepartmentName { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal Salary { get; set; }
    }
}