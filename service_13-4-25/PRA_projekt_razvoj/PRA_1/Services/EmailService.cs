using MimeKit;
using MailKit.Net.Smtp;

namespace PRA_1.Services
{
    public class EmailService
    {
        public static void Send(string toEmail, string code, DateTime codeTimeExpiring)
        {
            string SenderEmail = "franjo0330@gmail.com";

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Algebra Login Code", SenderEmail)); // Your sender email
            message.To.Add(new MailboxAddress("AlgebraMail", toEmail));
            message.Subject = "Algebra Authentaction Code";

            message.Body = new TextPart("plain")
            {
                Text = "Your Algebra authentication code " + code + " lasts for 5 minutes and expires on " + codeTimeExpiring.ToString() + "."           
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

                // Auth: app password if using Gmail with 2FA
                client.Authenticate("franjo0330@gmail.com", "suvu ybhr nurx wcmg");

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
