using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PRA_1.Security;
using PRA_project.Controllers;
using PRA_project.DTOs;
using PRA_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class CreditCardControllerTests
    {
        private readonly CreditCardController _controller;
        private readonly PraDatabaseContext _context;

        public CreditCardControllerTests()
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

            _controller = new CreditCardController(_context, configuration);
        }

        [Fact]
        public void GetCreditCardsById_AllValid_ReturnsEmptyList()
        {
           
            var result = _controller.GetCardsById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<List<CreditCardGetDto>>(okResult.Value);
            Assert.Empty(value);

        }

        [Fact]
        public void GetCreditCardsById_InvalidUser_ReturnsBadRequest()
        {

            var result = _controller.GetCardsById(100);

            // Assert
            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(okResult.Value);
            Assert.Contains("User with", value);

        }

        [Fact]
        public void GetCreditCardsById_InvalidCvv_ReturnsBadRequest()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "1234567856781234",
                Cvv = "1234",
                ExpiryDate = DateTime.UtcNow.AddMonths(5),
                UserId = 1
            };

            var result = _controller.CreateCard(creditCardDto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(okResult.Value);
            Assert.Contains("User with", value);

        }

        [Fact]
        public void GetCreditCardsById_InvalidCreditCardNumber_ReturnsBadRequest()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "12345678567812341234",
                Cvv = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(5),
                UserId = 1
            };

            var result = _controller.CreateCard(creditCardDto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(okResult.Value);
            Assert.Contains("User with", value);

        }

        [Fact]
        public void GetCreditCardsById_InvalidExpiryDate_ReturnsBadRequest()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "1234567856781234",
                Cvv = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(-5),
                UserId = 1
            };

            var result = _controller.CreateCard(creditCardDto);

            var okResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(okResult.Value);
            Assert.Contains("User with", value);

        }

        [Fact]
        public void DeleteCard_AllValid_ReturnsOk()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "1234567856781234",
                Cvv = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(5),
                UserId = 1
            };

            CreditCard creditCard = new CreditCard
            {
                FirstName = creditCardDto.FirstName,
                Lastname = creditCardDto.LastName,
                CreditCardNumber = creditCardDto.CreditCardNumber,
                ExpiryDate = creditCardDto.ExpiryDate,
                Cvvsalt = "1234567890",
                Cvvhash = "0987654321",
            };

            _context.CreditCards.Add(creditCard);
            _context.SaveChanges();

            _context.UserCreditCards.Add(new UserCreditCard() { CreditCardId = creditCard.IdCreditCard, UserId = 1 });
            _context.SaveChanges();

            var result = _controller.DeleteCard(new CreditCardDeleteDto() { CreditCardId = creditCard.IdCreditCard, UserId = 1 });

            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<CreditCardDeleteDto>(okResult.Value);
            
        }

        [Fact]
        public void DeleteCard_InvalidUser_ReturnsBadResult()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "1234567856781234",
                Cvv = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(5),
                UserId = 1
            };

            CreditCard creditCard = new CreditCard
            {
                FirstName = creditCardDto.FirstName,
                Lastname = creditCardDto.LastName,
                CreditCardNumber = creditCardDto.CreditCardNumber,
                ExpiryDate = creditCardDto.ExpiryDate,
                Cvvsalt = "1234567890",
                Cvvhash = "0987654321",
            };

            _context.CreditCards.Add(creditCard);
            _context.SaveChanges();

            _context.UserCreditCards.Add(new UserCreditCard() { CreditCardId = creditCard.IdCreditCard, UserId = 1 });
            _context.SaveChanges();

            var result = _controller.DeleteCard(new CreditCardDeleteDto() { CreditCardId = creditCard.IdCreditCard, UserId = 100 });

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(badResult.Value);
            Assert.Contains("was not found", value);

        }

        [Fact]
        public void DeleteCard_InvalidCreditCard_ReturnsBadResult()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "1234567856781234",
                Cvv = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(5),
                UserId = 1
            };

            CreditCard creditCard = new CreditCard
            {
                FirstName = creditCardDto.FirstName,
                Lastname = creditCardDto.LastName,
                CreditCardNumber = creditCardDto.CreditCardNumber,
                ExpiryDate = creditCardDto.ExpiryDate,
                Cvvsalt = "1234567890",
                Cvvhash = "0987654321",
            };

            _context.CreditCards.Add(creditCard);
            _context.SaveChanges();

            _context.UserCreditCards.Add(new UserCreditCard() { CreditCardId = creditCard.IdCreditCard, UserId = 1 });
            _context.SaveChanges();

            var result = _controller.DeleteCard(new CreditCardDeleteDto() { CreditCardId = 123, UserId = 1 });

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(badResult.Value);
            Assert.Contains("Credit card with IDCreditCard", value);

        }

        [Fact]
        public void DeleteCard_InvalidUserCreditCard_ReturnsBadResult()
        {

            CreditCardCreateDto creditCardDto = new CreditCardCreateDto
            {
                FirstName = "Denis",
                LastName = "Denisic",
                CreditCardNumber = "1234567856781234",
                Cvv = "123",
                ExpiryDate = DateTime.UtcNow.AddMonths(5),
                UserId = 1
            };

            CreditCard creditCard = new CreditCard
            {
                FirstName = creditCardDto.FirstName,
                Lastname = creditCardDto.LastName,
                CreditCardNumber = creditCardDto.CreditCardNumber,
                ExpiryDate = creditCardDto.ExpiryDate,
                Cvvsalt = "1234567890",
                Cvvhash = "0987654321",
            };

            _context.CreditCards.Add(creditCard);
            _context.SaveChanges();

            var result = _controller.DeleteCard(new CreditCardDeleteDto() { CreditCardId = creditCard.IdCreditCard, UserId = 1 });

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(badResult.Value);
            Assert.Contains("User does not have Credit Card saved in the system.", value);

        }


    }
}
