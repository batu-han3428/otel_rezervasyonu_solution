

using otel_rezervasyonu.Identity;
using System.ComponentModel.DataAnnotations;

namespace otel_rezervasyonu.Models
{
    public class Rooms
    {
        [Key]      
        public int no { get; set; }  
        public bool situation { get; set; }
        public string? renterid { get; set; }
        public int numberofbeds { get; set; }
        public int numberofactivebeds { get; set; }      
        public int price { get; set; }
        public int activeprice { get; set; }
    }
}
