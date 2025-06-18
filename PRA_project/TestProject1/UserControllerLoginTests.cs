using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRA_1.DTOs;
using PRA_1.Security;
using PRA_project.Controllers;
using PRA_project.Models;

namespace TestProject1
{
    public class UserControllerLoginTests
    {
        private readonly UserController _controller;
        private readonly PraDatabaseContext _context;

        public UserControllerLoginTests()
        {
            // Konfiguracija za JWT
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                { "JWT:SecureKey", "2a46799c7b00bbf432f7c2d1ce1df4ccbef53f0fdd4e33f831867712b4f35e1e" }
                })
                .Build();

            // InMemory EF baza
            var options = new DbContextOptionsBuilder<PraDatabaseContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PraDatabaseContext(options);

            // 1. Priprema test korisnika
            var password = "Test123!";
            var salt = PasswordHashProvider.GetSalt();
            var hash = PasswordHashProvider.GetHash(password, salt);

            var testUser = new User
            {
                IdUser = 1,
                EmailAddress = "test@example.com",
                PasswordSalt = salt,
                PasswordHash = hash,
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
                Temp2Facode = "123456",
                Temp2FacodeExpires = DateTime.Now.AddMinutes(5)
            };

            _context.Users.Add(testUser);
            _context.SaveChanges();

            _controller = new UserController(_context, configuration);
        }

        [Fact]
        public void LoginUser_WithValidCredentials_ReturnsOk()
        {
            // Arrange
            var loginDto = new UserLoginDto
            {
                Email = "test@example.com",
                UserPassword = "Test123!",
                CodeSenderOption = "email"
            };

            // Act
            var result = _controller.LoginUser(loginDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<string>(okResult.Value);
            Assert.Contains("2FA code has been sent", value);
        }

        [Fact]
        public void LoginUser_WithInvalidPassword_ReturnsBadRequest()
        {
            var loginDto = new UserLoginDto
            {
                Email = "test@example.com",
                UserPassword = "WrongPass",
                CodeSenderOption = "email"
            };

            var result = _controller.LoginUser(loginDto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Incorrect username or password", badResult.Value);
        }

        [Fact]
        public void LoginUser_WithNonExistingEmail_ReturnsBadRequest()
        {
            var loginDto = new UserLoginDto
            {
                Email = "nonexisting@example.com",
                UserPassword = "Anything",
                CodeSenderOption = "email"
            };

            var result = _controller.LoginUser(loginDto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Incorrect username or password", badResult.Value);
        }

        [Fact]
        public void Verify2FA_WithCorrectCode_ReturnsOk()
        { 
            var verify2FAdto = new Verify2FADto
            { 
                Email = "test@example.com",
                AuthCode = "123456"
            };

            var result = _controller.Verify2FA(verify2FAdto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<string>(okResult.Value);
            Assert.NotNull(value);
        } 
        
        [Fact]
        public void Verify2FA_WithIncorrectCode_ReturnsBadRequest()
        { 
            var verify2FAdto = new Verify2FADto
            { 
                Email = "test@example.com",
                AuthCode = "12345"
            };

            var result = _controller.Verify2FA(verify2FAdto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(badResult.Value);
            Assert.NotNull(value);
        }

        [Fact]
        public void Verify2FA_WithExpiredTime_ReturnsBadRequest()
        {
            

            var verify2FAdto = new Verify2FADto
            {
                Email = "test@example.com",
                AuthCode = "123456",
                AuthCodeTime = DateTime.Now.AddMinutes(10)
            };

            var result = _controller.Verify2FA(verify2FAdto);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(badResult.Value);
            Assert.NotNull(value);
        }
    }
}
