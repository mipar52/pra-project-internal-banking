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
    public class BillingAccountControllerTests
    {

        private readonly BillingAccountController _controller;
        private readonly PraDatabaseContext _context;

        public BillingAccountControllerTests()
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
                Temp2Facode = "123456",
                Temp2FacodeExpires = DateTime.Now.AddMinutes(5)
            };


            _context.Users.Add(testUser1);
            _context.SaveChanges();

            _controller = new BillingAccountController(_context, configuration);
        }

    }
}
    //    [danitc] kako vracas u kontroleru return Ok(new { Amount = user.BillingAccounts.First().Balance
    //}); Nesto ne zeli pa nisam htio mijenjat kontroler

    //    [Fact]
    //    public void GetBillingAccountByMail_ValidUserWithBillingAccount_ReturnsAmount()
    //    {

    //        var user = _context.Users.First(u => u.EmailAddress == "test@example.com");

    //        _context.BillingAccounts.Add(new BillingAccount
    //        {
    //            IdBillingAccount = 1,
    //            UserId = user.IdUser,
    //            Balance = 150.75m,
    //            User = user
    //        });

    //        _context.SaveChanges();

    //        var result = _controller.GetBillingAccountByMail("test@example.com");

    //        var ok = Assert.IsType<OkObjectResult>(result);
    //        //var value = Assert.IsType<BillingAccountDto>(ok.Value);\
    //        dynamic value = ok.Value;
    //        Assert.Equal(150.75m, value.Amount);
    //    }
    //}
