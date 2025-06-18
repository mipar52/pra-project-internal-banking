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
    public class TransactionControllerTests
    {
        private readonly TransactionController _controller;
        private readonly PraDatabaseContext _context;

        public TransactionControllerTests()
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

            var testUser1 = new User
            {
                IdUser = 1,
                EmailAddress = "test@example.com",
                PasswordSalt = salt,
                PasswordHash = hash,
                FirstName = "Test",
                LastName = "User",
                PhoneNumber = "123456789",
            };


            _context.Users.Add(testUser1);
            _context.SaveChanges();

            _controller = new TransactionController(_context, configuration);
        }

        [Fact]
        public void TransactionCreate_ValidTransaction_ReturnsOk()
        {
            
            //var user = _context.Users.First(u => u.EmailAddress == "test@example.com");

            _context.BillingAccounts.Add(new BillingAccount
            {
                IdBillingAccount = 1,
                UserId = 1,
                Balance = 200.00m
            });

            _context.SaveChanges();

            var transDto = new TransactionCreateDto
            {
                UserId = 1,
                TransactionTypeId = 1, 
                Amount = 50.00m
            };

           
            var result = _controller.TransactionCreate(transDto);

            
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<TransactionCreateDto>(ok.Value);
            Assert.Equal(transDto.UserId, value.UserId);
            Assert.Equal(transDto.Amount, value.Amount);

            var updatedAccount = _context.BillingAccounts.First(b => b.UserId == 1);
            Assert.Equal(150.00m, updatedAccount.Balance);
        }

    }
}
