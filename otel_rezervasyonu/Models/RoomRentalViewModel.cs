namespace otel_rezervasyonu.Models
{
    public class RoomRentalViewModel
    {
        public List<Rooms> Rooms { get; set; }
        public RoomInformation RoomInformation { get; set; }
        public List<Rooms> ClosedRooms { get; set; }
    }
}
