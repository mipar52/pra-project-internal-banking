using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRA_project.DTOs;
using PRA_project.Models;

namespace PRA_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BillingAccountController : ControllerBase
    {
        private readonly PraDatabaseContext _context;
        private readonly IConfiguration _configuration;

        public BillingAccountController(PraDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{email}")]
        public ActionResult GetBillingAccountByMail(string email)
        {
            try
            {
                var user = _context.Users
                    .Include(u => u.BillingAccounts)
                    .FirstOrDefault(x => x.EmailAddress == email);

                if (user == null)
                {
                    return BadRequest($"User with email {email} was not found.");
                }
                if (user.BillingAccounts.Count == 0)
                {
                    return BadRequest($"User with email {email} has no billing accounts.");
                }

                return Ok(new { Amount = user.BillingAccounts.First().Balance });
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }
        }
    }
}
