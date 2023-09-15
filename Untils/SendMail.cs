using System.Net;
using System.Net.Mail;
namespace Car_rental.Untils
{

    public class Send
    {
        public void SendEmail(string email, string subject, string body)
        {
            var SendMail = new MailMessage();
            SendMail.From = new MailAddress("verifyemail8888@gmail.com");
            SendMail.To.Add(new MailAddress(email));
            Console.WriteLine(email);
            SendMail.Subject = subject;
            SendMail.Body = body;
            SmtpClient stmp = new SmtpClient("smtp.gmail.com");
            stmp.EnableSsl = true;
            stmp.Port = 587;
            stmp.DeliveryMethod = SmtpDeliveryMethod.Network;
            stmp.Credentials = new NetworkCredential("verifyemail8888@gmail.com", "otsetrxdbskdebbi");
            stmp.Send(SendMail);
        }
        public void SendEmailRegister(string email, string subject, string body)
        {
            var SendMail = new MailMessage();
            SendMail.From = new MailAddress(email);
            SendMail.To.Add(new MailAddress("verifyemail8888@gmail.com"));
            SendMail.Subject = subject;
            SendMail.Body = body;
            SmtpClient stmp = new SmtpClient("smtp.gmail.com");
            stmp.EnableSsl = true;
            stmp.Port = 587;
            stmp.DeliveryMethod = SmtpDeliveryMethod.Network;
            stmp.Credentials = new NetworkCredential("verifyemail8888@gmail.com", "otsetrxdbskdebbi");
            stmp.Send(SendMail);
        }
    }
}