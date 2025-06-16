using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;


namespace PRA_1.Services
{
    public class InfobipSmsService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://d9zgrg.api.infobip.com";

        private readonly string _apiKey = "8a2923717c4903b01202b6e0d1af1146-871aba2d-19bf-4e04-b53d-50aaf065ffec";
        //private readonly string _apiKey = "d758a3890e9f69ad7f94575d1ed344b7-5a42dd11-b36e-4529-a9d0-34275d882258";

        public InfobipSmsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("App", _apiKey);
        }

        public void SendSms(string toPhoneNumber, string code, DateTime codeTimeExpiring)
        {
            var requestBody = new
            {
                messages = new[]
                {
                new
                {
                    destinations = new[] { new { to = toPhoneNumber } },
                    from = "AlgebraCode", 
                    text = "Your Algebra authentication code " + code + " lasts for 5 minutes and expires on " + codeTimeExpiring.ToString() + "."
                }
            }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/sms/2/text/advanced")
            {
                Content = content
            };

            var response = _httpClient.Send(request);

            if (!response.IsSuccessStatusCode)
            {
                string error = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"SMS failed: {error}");
            }
            else
            {
                Console.WriteLine("SMS sent successfully!");
            }
        }
    }
}
