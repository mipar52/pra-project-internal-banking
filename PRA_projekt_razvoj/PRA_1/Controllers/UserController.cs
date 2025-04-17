using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Extras;
using PRA_1.Models;
using PRA_1.Security;
using PRA_1.Services;
using System.Diagnostics.Metrics;
using System.Linq.Expressions;
using Twilio.Rest.Chat.V1;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static Verify2FADto userExtra = new Verify2FADto();
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(PraDbContext context, IConfiguration configuration)
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

        [HttpGet("[action]")]
        public ActionResult GetUsers()
        {
            try
            {
                IEnumerable<User> users = _context.Users.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetUserById(int id)
        {

            try
            {
                User user = _context.Users.ToList().Find(x => x.Iduser == id);

                if (user == null)
                {
                    return BadRequest("User with that username does not exist!");
                }

                return Ok(user);
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
                //string emailTest = userCreateDto.Email.Trim();

                //if (_context.Users.Any(x => x.Email.Equals(emailTest)))
                //{
                //    return BadRequest($"User with an e-mail {userCreateDto.Email} already exists!");
                //}
                
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userCreateDto.UserPassword, b64salt);

                //string usernameTest = userCreateDto.FirstName.ToLower()[0] + userCreateDto.LastName.ToLower();

                userCreateDto.UserName = userCreateDto.FirstName.ToLower()[0] + userCreateDto.LastName.ToLower();
                userCreateDto.Email = userCreateDto.FirstName.ToLower()[0] + userCreateDto.LastName.ToLower() + "@algebra.hr";

                if (_context.Users.Any(x => x.UserName.Equals(userCreateDto.UserName)))
                {
                    int counter = _context.Users.Count(x => x.UserName.Contains(userCreateDto.UserName));
                    counter++;

                    userCreateDto.UserName += counter;
                    userCreateDto.Email = userCreateDto.FirstName.ToLower()[0] + userCreateDto.LastName.ToLower() + counter + "@algebra.hr";
                }

                User user = new User()
                {
                    FirstName = userCreateDto.FirstName,
                    LastName = userCreateDto.LastName,
                    Email = RemoveDiacritics.RemoveDiacriticsMethod(userCreateDto.Email),
                    Phone = userCreateDto.Phone,
                    UserPassword = b64hash,
                    TestPassword = b64salt,
                    UserName = RemoveDiacritics.RemoveDiacriticsMethod(userCreateDto.UserName)
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }   
        }

        // POBRINUTI SE ZA EXCEPTIONE VEZANO ZA UNIQNESS PODATAKA
        [HttpPut("[action]")]
        public IActionResult UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Email == userUpdateDto.Email);

                if (user == null)
                {
                    return BadRequest("User with that username does not exist!");
                }

                if (userUpdateDto.FirstName != "string" && userUpdateDto.FirstName != null) user.FirstName = userUpdateDto.FirstName;
                if (userUpdateDto.LastName != "string" && userUpdateDto.LastName != null) user.LastName = userUpdateDto.LastName;
                if (userUpdateDto.Password != "string" && userUpdateDto.Password != null) user.UserPassword = userUpdateDto.Password;
                if (userUpdateDto.Phone != "string" && userUpdateDto.Phone != null) user.Phone = userUpdateDto.Phone;

                _context.Users.Update(user);
                _context.SaveChanges();

                return Ok("User has been updated!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("[action]")]
        public ActionResult DeleteUser(UserDeleteDto userDeleteDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.UserName == userDeleteDto.Email || u.Iduser == userDeleteDto.Id);

                if (user == null)
                {
                    return BadRequest("User with that username does not exist!");
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                return Ok($"A user with an IDUser {user.Iduser} has been succesfully deleted.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            };
        }

        [HttpPost("[action]")]
        public ActionResult LoginUser(UserLoginDto UserDto)
        {
            try
            {
                var loginFailMessage = "Incorrect username or password";

                var existingUser = _context.Users.FirstOrDefault(u =>u.Email == UserDto.Email);
                if(existingUser == null)
                {
                    return BadRequest(loginFailMessage);
                }

                var b64hash = PasswordHashProvider.GetHash(UserDto.UserPassword, existingUser.TestPassword);
                if(b64hash != existingUser.UserPassword)
                    return BadRequest(loginFailMessage);

                Console.WriteLine("Succesful Login");

                var secureKey = _configuration["JWT:SecureKey"];
                var SerializedToken = JwtTokenProvider.CreateToken(secureKey, 120, UserDto.Email);

                var code = _2FACodeProvider.GenerateCode();
                var codeTimeExpiring = _2FACodeProvider.GenerateCodeTimeExpiring();

                existingUser.Temp2Facode = code;
                existingUser.Temp2FacodeExpires = codeTimeExpiring;

                string tempEmail = "pra-projekt@outlook.com";
                string tempPhone = "+385955199757";

                if(UserDto.CodeSenderOption == "email")
                {
                    EmailService.Send(tempEmail, code, codeTimeExpiring);
                }
                else if (UserDto.CodeSenderOption == "phone")
                {
                    //SmsService.Send(tempPhone, code, codeTimeExpiring);
                    InfobipSmsService newSms = new InfobipSmsService();
                    newSms.SendSms(tempPhone, code, codeTimeExpiring);
                }
                
                userExtra.Email = existingUser.Email;
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

            var user = _context.Users.FirstOrDefault(u => u.Email == userExtra.Email);

            if (user == null) return BadRequest("Invalid user.");

            if (userDto.AuthCode != user.Temp2Facode || userExtra.AuthCodeTime < DateTime.UtcNow.ToLocalTime())
            {
                return BadRequest("Invalid or expired 2FA code.");

                userExtra.Email = null;
                user.Temp2Facode = null;
                user.Temp2FacodeExpires = null;
                user.Temp2Facode = null;
                user.Temp2FacodeExpires = null;
                _context.SaveChanges();
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
