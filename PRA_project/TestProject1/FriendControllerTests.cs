using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Bcpg;
using PRA_1.DTOs;
using PRA_1.Security;
using PRA_project.Controllers;
using PRA_project.DTOs;
using PRA_project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class FriendControllerTests
    {
        private readonly FriendController _controller;
        private readonly PraDatabaseContext _context;

        public FriendControllerTests()
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

            var testUser2 = new User
            {
                IdUser = 2,
                EmailAddress = "test2@example.com",
                PasswordSalt = salt,
                PasswordHash = hash,
                FirstName = "Test2",
                LastName = "User2",
                PhoneNumber = "987654321",
                Temp2Facode = "654321",
                Temp2FacodeExpires = DateTime.Now.AddMinutes(5)
            };

            _context.Users.Add(testUser1);
            _context.Users.Add(testUser2);
            _context.SaveChanges();

            _controller = new FriendController(_context, configuration);
        }



        //GENERICNI TEST
        //[Fact]
        //public void GetFriendsById_WithValidUserID_ReturnsOk()
        //{
        //    var result = _controller.GetFriendsById(1);


        //    // Assert
        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var value = Assert.IsAssignableFrom<List<FriendGetDto>>(okResult.Value);

        //}

        //KADA NEMA PRIJATELJA VRACA PRAZNU LISTU - TO JE OK
        [Fact]
        public void GetFriendsById_UserExistsWithoutFriends_ReturnsEmptyList()
        {
            var result = _controller.GetFriendsById(1);


            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<List<FriendGetDto>>(okResult.Value);
            Assert.Empty(value);

        }

        [Fact]
        public void GetFriendsById_UserWithOneFriend_ReturnsFriendInList()
        {

            var expectedUsers = new List<FriendGetDto>
            {
                new FriendGetDto { EmailAddress = "test2@example.com" }
            };

            _context.Friends.Add(new Friend { UserId = 1, FriendId = 2 });
            /*_context.Friends.Add(new Friend { UserId = 2, FriendId = 1 });*/

            _context.SaveChanges();

            var result = _controller.GetFriendsById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<List<FriendGetDto>>(ok.Value);


            for (int i = 0; i < expectedUsers.Count; i++)
            {
                Assert.Equal(expectedUsers[i].EmailAddress, value[i].EmailAddress);
            }

        }

        [Fact]
        public void CreateFriendById_ALlValid_ReturnsOk()
        {

            var friends = new FriendCreateDto()
            {
                UserId = 1,
                FriendId = 2
            };

            var result = _controller.CreateFriendById(friends);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<FriendCreateDto>(ok.Value);
            Assert.Equal(friends.FriendId, value.FriendId);
            
        }

        [Fact]
        public void CreateFriendById_InvalidUser_ReturnsUserBadRequest()
        {
            var friends = new FriendCreateDto()
            {
                UserId = 100,
                FriendId = 2
            };

            var result = _controller.CreateFriendById(friends);

            var ok = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(ok.Value);
            Assert.Contains("was not found", value);
        }

        [Fact]
        public void CreateFriendById_InvalidFriend_ReturnsFriendBadRequest()
        {

            var friends = new FriendCreateDto()
            {
                UserId = 1,
                FriendId = 200
            };

            var result = _controller.CreateFriendById(friends);

            var ok = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(ok.Value);
            Assert.Contains("was not found", value);


        }

        [Fact]
        public void CreateFriendById_ExistingFriendship_ReturnsFriendBadRequest()
        {
          
            _context.Friends.Add(new Friend { UserId = 1, FriendId = 2 });
            _context.Friends.Add(new Friend { UserId = 2, FriendId = 1 });

            _context.SaveChanges();

            var friends = new FriendCreateDto()
            {
                UserId = 1,
                FriendId = 2
            };

            var result = _controller.CreateFriendById(friends);

            var ok = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(ok.Value);
            Assert.Contains("You are already a friend with", value);

        }

        [Fact]
        public void DeleteFriendById_ValidDelete_ReturnsOk()
        {

            _context.Friends.Add(new Friend { UserId = 1, FriendId = 2 });
            _context.Friends.Add(new Friend { UserId = 2, FriendId = 1 });

            _context.SaveChanges();

            var friends = new FriendCreateDto()
            {
                UserId = 1,
                FriendId = 2
            };

            var result = _controller.DeleteFriendById(friends);

            var ok = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<FriendCreateDto>(ok.Value);
            Assert.Equal(friends.UserId, value.UserId);
            Assert.Equal(friends.FriendId, value.FriendId);

        }

        [Fact]
        public void DeleteFriendById_InvalidUser_ReturnsUserBadRequest()
        {

            var friends = new FriendCreateDto()
            {
                UserId = 100,
                FriendId = 2
            };

            var result = _controller.DeleteFriendById(friends);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(bad.Value);
            Assert.Contains("was not found", value);

        }

        [Fact]
        public void DeleteFriendById_InvalidFriend_ReturnsFriendBadRequest()
        {

            var friends = new FriendCreateDto()
            {
                UserId = 1,
                FriendId = 200
            };

            var result = _controller.DeleteFriendById(friends);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(bad.Value);
            Assert.Contains("was not found", value);

        }

        [Fact]
        public void DeleteFriendById_InvalidUserFriend_ReturnsBadRequest()
        {

            var friends = new FriendCreateDto()
            {
                UserId = 1,
                FriendId = 2
            };

            var result = _controller.DeleteFriendById(friends);

            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var value = Assert.IsType<string>(bad.Value);
            Assert.Contains("These friendship does not exist!", value);

        }

    }
}
