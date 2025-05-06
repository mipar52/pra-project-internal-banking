using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendRequestMoneyController : ControllerBase
    {
        private readonly PraDbContext _context;
        private readonly IConfiguration _configuration;

        public SendRequestMoneyController(PraDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet("[action]")]
        public ActionResult GetSendRequestPayments()
        {

            try
            {
                List<MoneyTransfer> sendRequestMoney = new List<MoneyTransfer>();

                sendRequestMoney = _context.SendRequestMoneys.ToList();

                return Ok(sendRequestMoney);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("[action]")]
        public ActionResult GetSendRequestPaymentsById(int id)
        {

            try
            {

                List<MoneyTransfer> sendRequestMoney = new List<MoneyTransfer>();

                sendRequestMoney 
                    = _context.SendRequestMoneys.Where(x => x.UserRecieverId == id || x.UserSenderId == id).ToList();

                if (sendRequestMoney == null)
                {
                    return BadRequest("Something is wrong with your fucking id, man");
                }

                return Ok(sendRequestMoney);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult SendRequestMoneyCreate(SendRequestMoneyCreateDto sendRequestMoneyCreateDto)
        {
            try
            {
                User userSender = _context.Users.FirstOrDefault(x => x.Iduser == sendRequestMoneyCreateDto.UserSenderId);
                User userReciever = _context.Users.FirstOrDefault(x => x.Iduser == sendRequestMoneyCreateDto.UserRecieverId);



                if (userSender == null)
                {
                    return BadRequest($"User Sender with IDUser {sendRequestMoneyCreateDto.UserSenderId} does not exist.");
                }

                if (userReciever == null)
                {
                    return BadRequest($"User Reciever with IDUser {sendRequestMoneyCreateDto.UserRecieverId} does not exist.");
                }

                MoneyTransfer sendRequestMoney = new MoneyTransfer()
                {
                    UserSenderId = sendRequestMoneyCreateDto.UserSenderId,
                    UserRecieverId = sendRequestMoneyCreateDto.UserRecieverId,
                    TransactionTypeId = sendRequestMoneyCreateDto.TransactionTypeId,
                    Amount = sendRequestMoneyCreateDto.Amount,
                    PaymentDate = DateTime.Now
                };

                _context.SendRequestMoneys.Add(sendRequestMoney);
                _context.SaveChanges();

                BillingAccount billingAccountSender = _context.BillingAccounts.FirstOrDefault(x => x.UserId == sendRequestMoney.UserSenderId);
                BillingAccount billingAccountReciever = _context.BillingAccounts.FirstOrDefault(x => x.UserId == sendRequestMoney.UserRecieverId);

                if (billingAccountSender == null)
                {
                    return BadRequest($"Billing account with IDUser {sendRequestMoneyCreateDto.UserSenderId} does not exist.");
                }

                if (billingAccountReciever == null)
                {
                    return BadRequest($"Billing account with IDUser {sendRequestMoneyCreateDto.UserRecieverId} does not exist.");
                }

                if (billingAccountSender.Balance < sendRequestMoney.Amount)
                {
                    return BadRequest($"User with IDUser {sendRequestMoneyCreateDto.UserRecieverId} does not have sufficient funds.");
                }

                billingAccountSender.Balance -= sendRequestMoney.Amount;
                billingAccountReciever.Balance += sendRequestMoney.Amount;

                _context.BillingAccounts.Update(billingAccountSender);
                _context.BillingAccounts.Update(billingAccountReciever);
                _context.SaveChanges();

                UserAllTransaction userSenderAllTransaction = new UserAllTransaction()
                {
                    UserId = userSender.Iduser,
                    TransactionId = sendRequestMoney.IdSendRequestMoney,
                    TransactionTypeId = 6,
                    Amount = sendRequestMoney.Amount,
                    TransactionDate = sendRequestMoney.PaymentDate
                };

                UserAllTransaction userRecieverAllTransaction = new UserAllTransaction()
                {
                    UserId = userReciever.Iduser,
                    TransactionId = sendRequestMoney.IdSendRequestMoney,
                    TransactionTypeId = 6,
                    Amount = sendRequestMoney.Amount,
                    TransactionDate = sendRequestMoney.PaymentDate
                };

                _context.UserAllTransactions.Add(userSenderAllTransaction);
                _context.UserAllTransactions.Add(userRecieverAllTransaction);
                _context.SaveChanges();

                return Ok($"Send/Request transfer was successful");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
