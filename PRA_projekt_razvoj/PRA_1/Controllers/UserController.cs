using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;
using PRA_1.Security;
using PRA_1.Services;
using System.Linq.Expressions;

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
                // The same secure key must be used here to create JWT,
                // as the one that is used by middleware to verify JWT
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
        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users = _context.Users.ToList();

            return users;
        }

        [HttpGet("[action]/{username}")]
        public IActionResult GetUserById(string username)
        {

           User user = _context.Users.ToList().Find(u => u.UserName == username);

            if(user == null)
            {
                return BadRequest("User with that username does not exist!");
            }

            return Ok(user);

        }

        [HttpPost("[action]")]
        public ActionResult<UserCreateDto> CreateUser([FromBody] UserCreateDto UserDto)
        {
            try
            {
                string UsernameTest = UserDto.UserName.Trim();

                if (_context.Users.Any(u => u.UserName.Equals(UsernameTest)))
                    return BadRequest("Username already exists!");

                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(UserDto.UserPassword, b64salt);

                User user = new User()
                {
                    FirstName = UserDto.FirstName,
                    LastName = UserDto.LastName,
                    Email = UserDto.FirstName.ToLower()[0] + UserDto.LastName.ToLower() + "@algebra.hr",
                    Phone = UserDto.Phone,
                    UserPassword = b64hash,
                    TestPassword = b64salt,
                    UserName = UserDto.UserName,
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                UserDto.IDUser = user.Iduser;

                return Ok(UserDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
            
            
        }

        // POBRINUTI SE ZA EXCEPTIONE VEZANO ZA UNIQNESS PODATAKA
        [HttpPut("[action]/{username})")]
        public IActionResult UpdateUser(string username, [FromBody] UserUpdateDto UserDto)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if(user == null)
            {
                return BadRequest("User with that username does not exist!");
            }

            if(UserDto.FirstName != "string" && UserDto.FirstName != null) user.FirstName = UserDto.FirstName;
            if(UserDto.LastName != "string" && UserDto.LastName != null) user.LastName = UserDto.LastName;
            if(UserDto.UserName != "string" && UserDto.UserName != null) user.UserName = UserDto.UserName;
            if(UserDto.Password != "string" && UserDto.Password != null) user.UserPassword = UserDto.Password;
            if(UserDto.Email != "user@example.com" && UserDto.Email != null) user.Email = UserDto.Email;
            if(UserDto.Phone != "string" && UserDto.Phone != null) user.Phone = UserDto.Phone;

            _context.SaveChanges();

            return Ok("User has been updated!");
        }

        [HttpDelete("[action]/{username}")]
        public IActionResult DeleteUser(string username)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserName == username);

            if(user == null)
            {
                return BadRequest("User with that username does not exist!");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok();
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

                if(UserDto.CodeSenderOption == "email")
                {
                    EmailService.Send(tempEmail, code, codeTimeExpiring);
                }
                else if (UserDto.CodeSenderOption == "phone")
                {
                    SmsService.Send("+385955199757", code, codeTimeExpiring);
                }
                
                userExtra.Email = existingUser.Email;
                //userExtra.AuthCode = existingUser.Temp2Facode;
                userExtra.AuthCodeTime = codeTimeExpiring;

                _context.SaveChanges();

                return Ok("2FA code has been sent to " + UserDto.Email);

                //do
                //{
                //        if(code != UserDto.AuthCode && codeTimeExpiring < DateTime.UtcNow)
                //    {
                //        return BadRequest("Your code is either wrong or expired!");
                //    }

                //} while(DateTime.UtcNow < codeTimeExpiring);

  
                //return Ok(SerializedToken);
                
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
