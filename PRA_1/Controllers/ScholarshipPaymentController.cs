using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScholarshipPaymentController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public ScholarshipPaymentController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("[action]")]
        public ActionResult GetScholarshipPayments()
        {

            try
            {
                List<ScholarshipPayment> scholarshipPayments = new List<ScholarshipPayment>();

                scholarshipPayments = _context.ScholarshipPayments.ToList();

                return Ok(scholarshipPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetScholarshipPaymentsById(int id)
        {

            try
            {

                List<ScholarshipPayment> scholarshipPayments = new List<ScholarshipPayment>();

                scholarshipPayments = _context.ScholarshipPayments.Where(x => x.UserId == id).ToList();

                if (scholarshipPayments == null)
                {
                    return BadRequest("Something is wrong with your fucking id, man");
                }

                return Ok(scholarshipPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult ScholarshipPaymentCreate(ScholarshipPaymentCreateDto scholarshipPaymentCreateDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Iduser == scholarshipPaymentCreateDto.UserId);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {scholarshipPaymentCreateDto.UserId} does not exist.");
                }

                ScholarshipPayment scholarshipPayment = new ScholarshipPayment()
                {
                    UserId = scholarshipPaymentCreateDto.UserId,
                    StudyProgramId = scholarshipPaymentCreateDto.StudyProgramId,
                    InstallmentNumber = scholarshipPaymentCreateDto.InstallmentNumber,
                    TotalInstallments = scholarshipPaymentCreateDto.TotalInstallments,
                    PaymentPlan = scholarshipPaymentCreateDto.PaymentPlan,
                    Amount = scholarshipPaymentCreateDto.Amount,
                    PaymentDate = DateTime.Now,
                };

                _context.ScholarshipPayments.Add(scholarshipPayment);
                _context.SaveChanges();

                BillingAccount billingAccount = _context.BillingAccounts.FirstOrDefault(x => x.UserId == scholarshipPaymentCreateDto.UserId);

                if (billingAccount == null)
                {
                    return BadRequest($"Billing account with IDUser {scholarshipPaymentCreateDto.UserId} does not exist.");
                }

                billingAccount.Balance -= scholarshipPayment.Amount;

                _context.BillingAccounts.Update(billingAccount);
                _context.SaveChanges();

                UserAllTransaction userAllTransaction = new UserAllTransaction()
                {
                    UserId = user.Iduser,
                    TransactionId = scholarshipPayment.IdScholarshipPayment,
                    TransactionTypeId = 2,
                    Amount = scholarshipPayment.Amount,
                    TransactionDate = scholarshipPayment.PaymentDate
                };

                _context.UserAllTransactions.Add(userAllTransaction);
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
