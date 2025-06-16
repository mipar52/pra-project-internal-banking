using MimeKit;
using MailKit.Net.Smtp;
using System.Diagnostics;

namespace PRA_1.Services
{
    public class EmailService
    {
        public static void Send(string toEmail, string code, DateTime codeTimeExpiring)
        {

            string SenderEmail = "franjo3003@gmail.com";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Algebra Login Code", SenderEmail)); // Your sender email
            message.To.Add(new MailboxAddress("AlgebraMail", toEmail));
            message.Subject = "Algebra Authentaction Code";

            message.Body = new TextPart("plain")
            {
                Text = "Your Algebra authentication code " + code + " lasts for 5 minutes and expires on " + codeTimeExpiring.ToString() + "."           
            };

            Debug.WriteLine("Sent!!!");
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Auth: app password if using Gmail with 2FA
                client.Authenticate("milanparadina83@gmail.com", "yves tprs mnav fffa");

                client.Send(message);
                client.Disconnect(true);
            }

            //using (var client = new SmtpClient())
            //{
            //    client.Connect("smtp.office365.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            //    // Auth: app password if using Gmail with 2FA
            //    client.Authenticate("pra-projekt@outlook.com", "hlecgtexpgdcqdqn");

            //    client.Send(message);
            //    client.Disconnect(true);
            //}
        }
    }
}
