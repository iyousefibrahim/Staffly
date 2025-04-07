using System.Net;
using System.Net.Mail;

namespace Staffly.PL.Helpers
{
    public static class EmailSettings
    {
        public static bool SendEmail(Email email)
        {
            try
            {
                // SMTP settings
                var client = new SmtpClient("smtp@gmail.com", 587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("test@gmail.com", "password");
                client.Send("test@gmail.com", email.To, email.Subject, email.Body);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
