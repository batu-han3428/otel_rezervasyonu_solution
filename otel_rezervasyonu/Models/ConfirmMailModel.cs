namespace otel_rezervasyonu.Models
{
    public class ConfirmMailModel
    {
        public string Email { get; set; }
        public string ErrorDescription { get; set; }
        public bool hasError { get; set; }
    }
}
