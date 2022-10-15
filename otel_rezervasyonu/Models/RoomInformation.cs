namespace otel_rezervasyonu.Models
{
    public class RoomInformation
    {
        public int RoomNo { get; set; }
        public DateTime DateOfEntry { get; set; }
        public DateTime LeavingDate { get; set; }
        public int NumberOfCustomers { get; set; }
    }
}
