using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Security;
using PRA_1.Services;
using PRA_project.DataSaver;
using PRA_project.Models;

namespace PRA_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private Verify2FADto userExtra = new Verify2FADto();
        private readonly PraDatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UserController(PraDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public ActionResult GetToken()
        {
            try
            {
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtTokenProvider.CreateToken(secureKey, 10);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CreateUser(UserCreateDto userCreateDto)
        {
            try
            {
               
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userCreateDto.Password, b64salt);

                User user = new User()
                {
                    FirstName = userCreateDto.FirstName,
                    LastName = userCreateDto.LastName,
                    EmailAddress = userCreateDto.EmailAddress,
                    PhoneNumber = userCreateDto.PhoneNumber,
                    PasswordHash = b64hash,
                    PasswordSalt = b64salt,
                    RoleId = userCreateDto.RoleId,
                    StudyProgramId = userCreateDto.StudyProgramId,
                    ProfilePictureUrl = GetProfilePicture.GetProfilePicturePath(userCreateDto.EmailAddress)
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                BillingAccount account = new BillingAccount()
                {
                    UserId = user.IdUser,
                    Balance = 0
                };

                _context.BillingAccounts.Add(account);
                _context.SaveChanges();

                return Ok(userCreateDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult LoginUser(UserLoginDto UserDto)
        {
            try
            {
                var loginFailMessage = "Incorrect username or password";

                var existingUser = _context.Users.FirstOrDefault(u => u.EmailAddress == UserDto.Email);
                if (existingUser == null)
                {
                    return BadRequest(loginFailMessage);
                }

                var b64hash = PasswordHashProvider.GetHash(UserDto.UserPassword, existingUser.PasswordSalt);
                if (b64hash != existingUser.PasswordHash)
                    return BadRequest(loginFailMessage);

                Console.WriteLine("Succesful Login");

                var secureKey = _configuration["JWT:SecureKey"];
                var SerializedToken = JwtTokenProvider.CreateToken(secureKey, 120, UserDto.Email);

                var code = _2FACodeProvider.GenerateCode();
                var codeTimeExpiring = _2FACodeProvider.GenerateCodeTimeExpiring();

                existingUser.Temp2Facode = code;
                existingUser.Temp2FacodeExpires = codeTimeExpiring;

                _context.Users.Update(existingUser);
                _context.SaveChanges();

                string tempEmail = "pra-projekt@outlook.com";
                string tempPhone = "+385955199757";

                if (UserDto.CodeSenderOption == "email")
                {
                    EmailService.Send(tempEmail, code, codeTimeExpiring);
                }
                else if (UserDto.CodeSenderOption == "phone")
                {
                    //SmsService.Send(tempPhone, code, codeTimeExpiring);
                    InfobipSmsService newSms = new InfobipSmsService();
                    newSms.SendSms(tempPhone, code, codeTimeExpiring);
                }

                userExtra.Email = existingUser.EmailAddress;
                //userExtra.AuthCode = existingUser.Temp2Facode;
                userExtra.AuthCodeTime = codeTimeExpiring;

                _context.SaveChanges();

                return Ok("2FA code has been sent to " + UserDto.Email);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("[action]")]
        public ActionResult Verify2FA(Verify2FADto userDto)
        {

            var user = _context.Users.FirstOrDefault(u => u.EmailAddress == userDto.Email);

            if (user == null) return BadRequest("Invalid user.");

            if (userDto.AuthCode != user.Temp2Facode || user.Temp2FacodeExpires < DateTime.Now)
            {
                userExtra.Email = null;
                user.Temp2Facode = null;
                user.Temp2FacodeExpires = null;
                user.Temp2Facode = null;
                user.Temp2FacodeExpires = null;
                _context.SaveChanges();

                return BadRequest("Invalid or expired 2FA code.");
            }

            userExtra.Email = null;
            user.Temp2Facode = null;
            user.Temp2FacodeExpires = null;
            user.Temp2Facode = null;
            user.Temp2FacodeExpires = null;
            _context.SaveChanges();

            var secureKey = _configuration["JWT:SecureKey"];
            var seralizedToken = JwtTokenProvider.CreateToken(secureKey, 120, userDto.Email);

            return Ok(seralizedToken);
        }
 
    }
}
