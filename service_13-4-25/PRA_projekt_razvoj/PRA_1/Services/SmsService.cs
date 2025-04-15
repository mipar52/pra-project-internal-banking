using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace PRA_1.Services
{
    public class SmsService
    {
        //2CMX4EJJ87HZ3XHH1F4B92NX Recovery code for Twilio
        public static void Send(string toPhoneNumber, string code, DateTime codeTimeExpiring)
        {
            const string accountSid = "ACedeef3336463a9aaebf48d863d6bbf19";
            const string authToken = "48c8e3355420394043497559e29efee5";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: "Your Algebra authentication code " + code + " lasts for 5 minutes and expires on " + codeTimeExpiring.ToString() + ".",
                from: new PhoneNumber("+14452754782"),
                to: new PhoneNumber(toPhoneNumber)
            );

            Console.WriteLine("SMS sent with SID: " + message.Sid);
        }
    }
}
