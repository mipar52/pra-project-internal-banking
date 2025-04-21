using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphController : ControllerBase
    {
        
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public GraphController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public ActionResult GetGraphDataById(int id)
        {


            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == id);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {user.Iduser} was not found");
                }

                List<UserAllTransaction> userAllTransactions = new List<UserAllTransaction>();

                userAllTransactions = _context.UserAllTransactions.ToList().Where(x => x.UserId == id).ToList();

                IDictionary<DateTime, int> userAllTransactionsMap = new Dictionary<DateTime, int>();

                foreach (var transaction in userAllTransactions)
                {

                    DateTime date = transaction.TransactionDate;
                    DateTime onlyDate = date.Date;
                    
                    if (userAllTransactionsMap.ContainsKey(onlyDate))
                    {
                        userAllTransactionsMap[onlyDate]++;
                    }
                    else
                    {
                        userAllTransactionsMap.Add(onlyDate, 1);
                    }

                }

                return Ok(userAllTransactionsMap);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
