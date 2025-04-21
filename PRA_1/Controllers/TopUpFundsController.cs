using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;
using PRA_1.Security;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopUpFundsController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public TopUpFundsController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPut("[action]")]
        public ActionResult UpdateBalanceById(UpdateBalanceDto updateBalanceDto)
        {
            User user = _context.Users.FirstOrDefault(x => x.Iduser == updateBalanceDto.IdUser);

            if (user == null)
            {
                return BadRequest($"User with IDUser {user.Iduser} was not found");
            }

            List<CreditCardDataBase> creditCardDb = new List<CreditCardDataBase>();
            creditCardDb = _context.CreditCardDatabases.ToList();

            foreach (var creditCardDbEntry in creditCardDb)
            {

                if (updateBalanceDto.CreditCardFirstName == creditCardDbEntry.FirstName && updateBalanceDto.CreditCardLastName == creditCardDbEntry.LastName && updateBalanceDto.CreditCardExpiryDate == creditCardDbEntry.ExpiryDate && updateBalanceDto.CreditCardNumber == creditCardDbEntry.CardNumber)
                {

                    var b64hashdb = PasswordHashProvider.GetHash(updateBalanceDto.CreditCardCvv, creditCardDbEntry.CvvSalt);
                    if (b64hashdb != creditCardDbEntry.CvvHash)
                        return BadRequest("Wrong CVV");
                }

            }

            BillingAccount billingAccount = _context.BillingAccounts.FirstOrDefault(x => x.UserId == updateBalanceDto.IdUser);

            if (billingAccount == null)
            {
                BadRequest($"Billing account of a user with IdUser {user.Iduser} was not found");
            }

            billingAccount.Balance += updateBalanceDto.Balance;

            _context.BillingAccounts.Update(billingAccount);
            _context.SaveChanges();

            return Ok($"Billing account of a user with IDUser {user.Iduser} was topped up");

        }
    }
}
