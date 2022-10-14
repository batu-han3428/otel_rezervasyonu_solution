using System.Net.Mail;

namespace otel_rezervasyonu.Models
{
    public class EmailHelper
    {
        public bool SendEmail(string email, string mesaj)
        {

            #region MailMessage tanimlamalari 

            MailMessage mailMessage = new MailMessage();

            mailMessage.From = new MailAddress("bticaret01@gmail.com");

            mailMessage.To.Add(email);

            mailMessage.Subject = "Üyelik Onaylama";

            mailMessage.Body = mesaj;

            mailMessage.IsBodyHtml = true;

            #endregion 

            #region Smtp Ayarlari 

            SmtpClient client = new SmtpClient();

            client.Credentials = new System.Net.NetworkCredential("bticaret01@gmail.com", "yfpmyrmxfiftlgoh");


            client.Host = "smtp.gmail.com"; 

            client.Port = 587;

            client.EnableSsl = true;
            #endregion


            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
