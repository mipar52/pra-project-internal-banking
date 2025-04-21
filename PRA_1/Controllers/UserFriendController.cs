using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFriendController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public UserFriendController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public ActionResult GetFriendsById(int id)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == id);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} was not found");
                }

                List<UserFriend> userFriends = _context.UserFriends.Where(x => x.UserId == user.Iduser).ToList();

                return Ok(userFriends);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CreateFriendById(UserFriendCreateDto userFriendDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == userFriendDto.IdUser);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} was not found");
                }

                User friend = _context.Users.FirstOrDefault(x => x.Email == userFriendDto.FriendEmail);

                if (friend == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} was not found");
                }

                UserFriend userFriend = new UserFriend()
                {
                    UserId = user.Iduser,
                    FriendId = friend.Iduser,
                };

                _context.UserFriends.Add(userFriend);
                _context.SaveChanges();

                return Ok($"User with IDUser {user.Iduser} successfully added a friend with IdUser {friend.Iduser}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("[action]")]
        public ActionResult DeleteFriendById(UserFriendDeleteDto userFriendDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == userFriendDto.IdUser);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} was not found");
                }

                User friend = _context.Users.FirstOrDefault(x => x.Email == userFriendDto.FriendEmail);

                if (friend == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} was not found");
                }



                UserFriend userFriend = _context.UserFriends.FirstOrDefault(x => x.UserId == user.Iduser && x.FriendId == friend.Iduser);

                if (userFriend == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} does not have a friend with IDUser {friend.Iduser}");
                }

                _context.UserFriends.Remove(userFriend);
                _context.SaveChanges();

                return Ok($"User with IDUser {user.Iduser} successfully deleted a friend with IdUser {friend.Iduser}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
