using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;
using PRA_1.DTOs;
using PRA_1.Models;
using PRA_1.Services;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingPaymentController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public ParkingPaymentController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("[action]")]
        public ActionResult GetParkingPayments()
        {

            try
            {

                List<ParkingPayment> parkingPayments = new List<ParkingPayment>();

                parkingPayments = _context.ParkingPayments.ToList();

                return Ok(parkingPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetParkingPaymentsById(int id)
        {

            try
            {

                List<ParkingPayment> parkingPayments = new List<ParkingPayment>();

                parkingPayments = _context.ParkingPayments.Where(x => x.UserId == id).ToList();

                if(parkingPayments == null)
                {
                    return BadRequest("Something is wrong with your fucking id, man");
                }

                return Ok(parkingPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult ParkingPaymentCreate(ParkingPaymentCreateDto parkingPaymentCreateDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == parkingPaymentCreateDto.IdUser);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {parkingPaymentCreateDto.IdUser} does not exist.");
                }

                decimal parkingPrice = 2.5m;

                ParkingPayment parkingPayment = new ParkingPayment()
                {
                    UserId = parkingPaymentCreateDto.IdUser,
                    RegistrationCountryCode = parkingPaymentCreateDto.RegistrationCountryCode.ToUpper(),
                    RegistrationNumber = parkingPaymentCreateDto.RegistrationNumber.ToUpper(),                   
                    Amount = parkingPaymentCreateDto.DurationHours * parkingPrice,
                    StartTime = DateTime.Now.AddHours(2),
                    DurationHours = parkingPaymentCreateDto.DurationHours
                };

                _context.ParkingPayments.Add(parkingPayment);
                _context.SaveChanges();

                BillingAccount billingAccount = _context.BillingAccounts.FirstOrDefault(x => x.UserId == parkingPaymentCreateDto.IdUser);

                if (billingAccount == null)
                {
                    return BadRequest($"Billing account with IDUser {parkingPaymentCreateDto.IdUser} does not exist.");
                }

                billingAccount.Balance -= parkingPayment.Amount;

                _context.BillingAccounts.Update(billingAccount);
                _context.SaveChanges();

                return Ok($"Parking payment was successful");
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex.Message);
            }

        }
    }
}



