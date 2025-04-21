using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CanteenPaymentController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public CanteenPaymentController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("[action]")]
        public ActionResult GetCanteenPayments()
        {

            try
            {
                List<CanteenPayment> canteenPayments = new List<CanteenPayment>();

                canteenPayments = _context.CanteenPayments.ToList();

                return Ok(canteenPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetCanteenPaymentsById(int id)
        {

            try
            {

                List<CanteenPayment> canteenPayments = new List<CanteenPayment>();

                canteenPayments = _context.CanteenPayments.Where(x => x.UserId == id).ToList();

                if (canteenPayments == null)
                {
                    return BadRequest("Something is wrong with your fucking id, man");
                }

                return Ok(canteenPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CanteenPaymentCreate(CanteenPaymentCreateDto canteenPaymentCreateDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == canteenPaymentCreateDto.UserId);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {canteenPaymentCreateDto.UserId} does not exist.");
                }

                decimal parkingPrice = 2.5m;

                CanteenPayment canteenPayment = new CanteenPayment()
                {
                    UserId = canteenPaymentCreateDto.UserId,
                    Amount = canteenPaymentCreateDto.Amount,
                    PaymentDate = DateTime.Now
                };

                _context.CanteenPayments.Add(canteenPayment);
                _context.SaveChanges();

                BillingAccount billingAccount = _context.BillingAccounts.FirstOrDefault(x => x.UserId == canteenPaymentCreateDto.UserId);

                if (billingAccount == null)
                {
                    return BadRequest($"Billing account with IDUser {canteenPaymentCreateDto.UserId} does not exist.");
                }

                billingAccount.Balance -= canteenPayment.Amount;

                _context.BillingAccounts.Update(billingAccount);
                _context.SaveChanges();

                return Ok($"Canteen payment was successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
