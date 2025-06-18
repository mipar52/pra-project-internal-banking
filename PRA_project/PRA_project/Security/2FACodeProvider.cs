using Microsoft.AspNetCore.Components;

namespace PRA_1.Security
{
    public class _2FACodeProvider
    {
        public static string GenerateCode()
        {
            string code = new Random().Next(100000, 999999).ToString();
            return code;
        }

        public static DateTime GenerateCodeTimeExpiring()
        {
            DateTime codeExpiring = DateTime.UtcNow.ToLocalTime().AddMinutes(5);
            return codeExpiring;
        }
        

        
    }
}
