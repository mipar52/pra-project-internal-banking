using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRA_project.DTOs;
using PRA_project.Models;
using System.Linq.Expressions;

namespace PRA_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly PraDatabaseContext _context;
        private readonly IConfiguration _configuration;

        public FriendController(PraDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{idUser}")]
        public ActionResult GetFriendsById(int idUser)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.IdUser == idUser);


                if (user == null)
                {
                    return BadRequest($"User with IDUser {idUser} was not found.");
                }

                //var getFriends = _context.Friends
                //     .Where(x => x.UserId == user.IdUser)
                //     .Include(x => x.Friend)
                //     .Select(f => new FriendGetDto
                //     {
                //         FirstName = f.Friend.FirstName,
                //         LastName = f.Friend.LastName,
                //         PhoneNumber = f.Friend.PhoneNumber,
                //         EmailAddress = f.Friend.EmailAddress,
                //         ProfilePictureUrl = f.Friend.ProfilePictureUrl,
                //     })
                //     .ToList();

                //List<int?> getFriendIds = _context.Friends
                //    .Where(x => x.UserId == user.IdUser)
                //    .Select(x => x.FriendId)
                //    .ToList();

                //if (getFriendIds.Count == 0)
                //{
                //    return BadRequest($"NIJE DOBRO");
                //}

                //List<FriendGetDto> getFriends = new List<FriendGetDto>();

                //foreach (var id in getFriendIds)
                //{
                //    User friend = _context.Users.FirstOrDefault(x => x.IdUser == id);

                //    if (user == null)
                //    {
                //        return BadRequest($"User with IDUser {id} was not found.");
                //    }

                //    getFriends.Add(new FriendGetDto
                //    {
                //        FirstName = friend.FirstName,
                //        LastName = friend.LastName,
                //        PhoneNumber = friend.PhoneNumber,
                //        EmailAddress = friend.EmailAddress,
                //        ProfilePictureUrl = friend.ProfilePictureUrl,
                //    }
                //    );
                //}

                //return Ok(getFriends);

                var friendIds = _context.Friends
                    .Where(x => x.UserId == user.IdUser)
                    .Select(x => x.FriendId)
                    .ToList();

                if (!friendIds.Any())
                {
                    return BadRequest("NIJE DOBRO");
                }

                var getFriends = _context.Users
                    .Where(u => friendIds.Contains(u.IdUser))
                    .Select(u => new FriendGetDto
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        PhoneNumber = u.PhoneNumber,
                        EmailAddress = u.EmailAddress,
                        ProfilePictureUrl = u.ProfilePictureUrl
                    })
                    .ToList();

                return Ok(getFriends);
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }

        }

        [HttpPost("[action]/{idUser}")]
        public ActionResult CreateFriendById(FriendCreateDto dto) 
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.IdUser == dto.UserId);


                if (user == null)
                {
                    return BadRequest($"User with IDUser {dto.UserId} was not found.");
                }

                User friend = _context.Users.FirstOrDefault(x => x.IdUser == dto.FriendId);


                if (user == null)
                {
                    return BadRequest($"User with IDUser {dto.FriendId} was not found.");
                }

                if (_context.Friends.Any(x => x.UserId == user.IdUser && x.FriendId == friend.IdUser))
                {
                    return BadRequest($"You are already a friend with {friend.FirstName} {friend.LastName}");
                }

                Friend friends = new Friend()
                {
                    UserId = user.IdUser,
                    FriendId = friend.IdUser
                };

                Friend viceVersaFriends = new Friend()
                {
                    UserId = friend.IdUser,
                    FriendId = user.IdUser
                };

                _context.Friends.Add(friends);
                _context.Friends.Add(viceVersaFriends);
                _context.SaveChanges();

                return Ok(dto);
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }
        }

        [HttpDelete("[action]/{idUser}/{FriendId}")]
        public ActionResult DeleteFriendById(FriendCreateDto dto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.IdUser == dto.UserId);


                if (user == null)
                {
                    return BadRequest($"User with IDUser {dto.UserId} was not found.");
                }

                User friend = _context.Users.FirstOrDefault(x => x.IdUser == dto.FriendId);


                if (user == null)
                {
                    return BadRequest($"User with IDUser {dto.FriendId} was not found.");
                }

                Friend friends = _context.Friends.FirstOrDefault(x => x.UserId == user.IdUser && x.FriendId == friend.IdUser);

                if (friend == null)
                {
                    return BadRequest($"These friendship does not exist!");
                }

                Friend viceVersaFriends = _context.Friends.FirstOrDefault(x => x.UserId == friend.IdUser && x.FriendId == user.IdUser);

                if (viceVersaFriends == null)
                {
                    return BadRequest($"These friendship does not exist!");
                }

                _context.Friends.Remove(friends);
                _context.Friends.Remove(viceVersaFriends);
                _context.SaveChanges();

                return Ok(dto);
            }
            catch (Exception)
            {
                return BadRequest($"Friend failed");
            }
        }
    }
}
