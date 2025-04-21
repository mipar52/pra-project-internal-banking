using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodPaymentController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public FoodPaymentController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("[action]")]
        public ActionResult GetFoodPayments()
        {

            try
            {
                List<FoodPayment> foodPayments = _context.FoodPayments.ToList();

                

                return Ok(foodPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetFoodPaymentsById(int id)
        {

            try
            {

                List<FoodPayment> foodPayments = _context.FoodPayments.Where(x => x.UserId == id).ToList();

                if (foodPayments == null)
                {
                    return BadRequest("Something is wrong with your fucking id, man");
                }

                return Ok(foodPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult FoodPaymentCreate(FoodPaymentCreateDto foodPaymentCreateDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == foodPaymentCreateDto.UserId);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {foodPaymentCreateDto.UserId} does not exist.");
                }

                FoodPayment foodPayment = new FoodPayment()
                {
                    UserId = foodPaymentCreateDto.UserId,
                    Amount = foodPaymentCreateDto.Amount,
                    PaymentDate = DateTime.Now
                };

                _context.FoodPayments.Add(foodPayment);
                _context.SaveChanges();

                BillingAccount billingAccount = _context.BillingAccounts.FirstOrDefault(x => x.UserId == foodPaymentCreateDto.UserId);

                if (billingAccount == null)
                {
                    return BadRequest($"Billing account with IDUser {foodPaymentCreateDto.UserId} does not exist.");
                }

                billingAccount.Balance -= foodPayment.Amount;

                _context.BillingAccounts.Update(billingAccount);
                _context.SaveChanges();

                UserAllTransaction userAllTransaction = new UserAllTransaction()
                {
                    UserId = user.Iduser,
                    TransactionId = foodPayment.IdFoodPayment,
                    TransactionTypeId = 5,
                    Amount = foodPayment.Amount,
                    TransactionDate = foodPayment.PaymentDate
                };

                _context.UserAllTransactions.Add(userAllTransaction);
                _context.SaveChanges();

                return Ok($"Food/drink payment was successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
