﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRA_1.DTOs;
using PRA_1.Security;
using PRA_1.Services;
using PRA_project.DataSaver;
using PRA_project.DTOs;
using PRA_project.Models;
using System.Diagnostics.Eventing.Reader;
using System.Net.Mail;
using System.Security.Claims;
using Twilio.Types;

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
                var user = _context.Users.FirstOrDefault(u => u.EmailAddress == userCreateDto.EmailAddress);

                if (user == null) {
                    var b64salt = PasswordHashProvider.GetSalt();
                    var b64hash = PasswordHashProvider.GetHash(userCreateDto.Password, b64salt);
                    user = new User()
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
                } else
                {
                    user.FirstName = userCreateDto.FirstName;
                    user.LastName = userCreateDto.LastName;
                 //   user.EmailAddress = userCreateDto.EmailAddress;
                    user.PhoneNumber = userCreateDto.PhoneNumber;
                    user.ProfilePictureUrl = GetProfilePicture.GetProfilePicturePath(userCreateDto.EmailAddress);
                }


                
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

                string tempEmail = "milanparadina83@gmail.com";
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

            if (userDto.AuthCode != user.Temp2Facode || user.Temp2FacodeExpires < userDto.AuthCodeTime)
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

        [HttpGet("[action]")]
        public ActionResult<UserDto> GetUserByMail(string email)
        {
            try
            {
                var user = _context.Users
                    .Include(u => u.StudyProgram)
                    .Include(u => u.Role)
                    .FirstOrDefault(u => u.EmailAddress == email);
                    
                if (user == null)
                    return NotFound("User not found.");

                var mappedResult = new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    PhoneNumber = user.PhoneNumber,
                    StudyProgramme = user.StudyProgram.Name,
                    Role = user.Role.Name,
                    ProfilePictureUrl = user.ProfilePictureUrl
                };

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}

