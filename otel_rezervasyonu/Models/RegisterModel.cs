using System.ComponentModel.DataAnnotations;

namespace otel_rezervasyonu.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        
        [Required]
        public string Tckimlik { get; set; }

        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string RePassword { get; set; }

        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
