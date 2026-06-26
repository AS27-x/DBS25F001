namespace HospitalManagementSystem.Models
{
    public class Room
    {
        public int RoomID { get; set; }
        public string RoomNumber { get; set; } = "";
        public string RoomType { get; set; } = "";
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}