namespace HospitalManagementSystem.Models
{
    public class Department
    {
        public int DeptID { get; set; }
        public string DeptName { get; set; } = "";
        public string Location { get; set; } = "";
        public string ContactNumber { get; set; } = "";
    }
}