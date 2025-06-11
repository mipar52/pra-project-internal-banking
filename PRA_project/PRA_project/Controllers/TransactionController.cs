using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRA_project.DTOs;
using PRA_project.Models;

namespace PRA_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly PraDatabaseContext _context;
        private readonly IConfiguration _configuration;
        public static int FkTransactionId = 0;
        public static DateTime CurrentDateTime => DateTime.UtcNow.AddHours(2);

        public TransactionController(PraDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{IdUser}")]
        public ActionResult GetTransactionsById(int IdUser)
        {
            User user = _context.Users
                        .FirstOrDefault(x => x.IdUser == IdUser);

            if (user == null)
            {
                return BadRequest($"User with IDUser {IdUser} was not found.");
            }

            List<Transaction> transactions = _context.Transactions
                .Include(x => x.TransactionType)
                .Where(x => x.UserId == IdUser)
                .ToList();

            List<TransactionGetDto> getTrans = new List<TransactionGetDto>();

            foreach(var t in  transactions)
            {
                getTrans.Add
                (
                    new TransactionGetDto
                    {
                        TypeName = t.TransactionType.TypeName,
                        Date = t.Date,
                        Amount = t.Amount
                    }
                );
            }

            return Ok(getTrans);
        }

        [HttpGet("[action]/{IdUser}/{month}")]
        public ActionResult GetTransactionsByMonthById(int IdUser, int month)
        {
            User user = _context.Users
                        .FirstOrDefault(x => x.IdUser == IdUser);

            if (user == null)
            {
                return BadRequest($"User with IDUser {IdUser} was not found.");
            }

            List<Transaction> transactions = _context.Transactions
                .Include(x => x.TransactionType)
                .Where(x => x.UserId == IdUser && x.Date.Month == month)
                .ToList();

            List<TransactionGetDto> getTrans = new List<TransactionGetDto>();

            foreach (var t in transactions)
            {
                getTrans.Add
                (
                    new TransactionGetDto
                    {
                        TypeName = t.TransactionType.TypeName,
                        Date = t.Date,
                        Amount = t.Amount
                    }
                );
            }

            return Ok(getTrans);
        }

        [HttpGet("[action]/{IdUser}")]
        public ActionResult GetTransactionsGraphById(int IdUser)
        {
            User user = _context.Users
                        .FirstOrDefault(x => x.IdUser == IdUser);

            if (user == null)
            {
                return BadRequest($"User with IDUser {IdUser} was not found.");
            }

            List<Transaction> transactions = _context.Transactions
                .Where(x => x.UserId == IdUser)
                .ToList();

            IDictionary<DateTime, int> maps = new Dictionary<DateTime, int>();

            foreach(var t in transactions)
            {
                DateTime date = t.Date.Date;

                if (maps.ContainsKey(date))
                {
                    maps[date]++;
                }
                else
                {
                    maps.Add(date, 1);    
                }
            }

            return Ok(maps);    
        }

        [HttpPost("[action]")]
        public ActionResult TransactionCreate(TransactionCreateDto transDto)
        {
            try
            {
                Transaction transaction = new Transaction()
                {
                    UserId = transDto.UserId,
                    TransactionTypeId = transDto.TransactionTypeId,
                    Amount = transDto.Amount,
                    Date = CurrentDateTime,
                };

                BillingAccount account = _context.BillingAccounts.FirstOrDefault(x => x.UserId == transDto.UserId);

                if (account == null)
                {
                    return BadRequest($"Billing account with UserID {transDto.UserId} was not found.");
                }

                if (transDto.TransactionTypeId != 5)
                {
                    if (account.Balance < transDto.Amount)
                    {
                        return BadRequest($"Insufficient funds.");
                    }

                    account.Balance -= transDto.Amount;
                }

               
                _context.Transactions.Add(transaction);
                _context.BillingAccounts.Update(account);
                _context.SaveChanges();

                FkTransactionId = transaction.IdTransaction;

                return Ok(transDto);
            }
            catch (Exception)
            {
                return BadRequest($"Transaction failed");
            }
        }

        [HttpPost("[action]")]
        public ActionResult ScholarshipTransactionCreate(ScholarshipTransactionCreateDto scholarshipDto)
        {
            try
            {
                User user = _context.Users
                        .Include(x => x.StudyProgram)
                        .FirstOrDefault(x => x.IdUser == scholarshipDto.UserId);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {scholarshipDto.UserId} was not found.");
                }

                BillingAccount account = _context.BillingAccounts.FirstOrDefault(x => x.UserId == scholarshipDto.UserId);

                if (account == null)
                {
                    return BadRequest($"Billing account with UserID {scholarshipDto.UserId} was not found.");
                }

                TransactionCreateDto transDto = new TransactionCreateDto()
                {
                    UserId = scholarshipDto.UserId,
                    TransactionTypeId = scholarshipDto.TransactionTypeId,
                    Amount = user.StudyProgram.Amount,
                    Date = scholarshipDto.Date,
                };

                try
                {
                    TransactionCreate(transDto);
                }
                catch (Exception)
                {

                    return BadRequest($"Transaction failed");
                }

                return Ok(scholarshipDto);
            }
            catch (Exception)
            {

                return BadRequest($"Transaction failed");
            }
        }

        [HttpPost("[action]")]
        public ActionResult ParkingTransactionCreate(ParkingTransactionCreateDto parkingDto)
        {
            try
            {
                User user = _context.Users
                        .FirstOrDefault(x => x.IdUser == parkingDto.UserId);

                if (user == null)
                {
                    return BadRequest($"User with IDUser {parkingDto.UserId} was not found.");
                }

                //BillingAccount account = _context.BillingAccounts.FirstOrDefault(x => x.UserId == parkingDto.UserId);

                //if (account == null)
                //{
                //    return BadRequest($"Billing account with UserID {parkingDto.UserId} was not found.");
                //}

                //if (account.Balance < parkingDto.Amount)
                //{
                //    return BadRequest($"Insufficient funds.");
                //}

                decimal parkingRate = 2.5m;

                TransactionCreateDto transDto = new TransactionCreateDto()
                {
                    UserId = parkingDto.UserId,
                    TransactionTypeId = parkingDto.TransactionTypeId,
                    Amount = parkingRate * parkingDto.DurationHours,
                    Date = parkingDto.Date

                };

                try
                {
                    TransactionCreate(transDto);
                }
                catch (Exception)
                {

                    return BadRequest($"Transaction failed");
                }

                ParkingPayment parking = new ParkingPayment()
                {
                    RegistrationCountryCode = parkingDto.RegistrationCountryCode,
                    RegistrationNumber = parkingDto.RegistrationNumber,
                    DurationHours = parkingDto.DurationHours,
                    StartTime = CurrentDateTime,
                    EndTime = CurrentDateTime.AddHours(parkingDto.DurationHours),
                    TransactionId = FkTransactionId
                };

                _context.ParkingPayments.Add(parking);
                _context.SaveChanges();

                return Ok(parkingDto);
            }
            catch (Exception)
            {
                return BadRequest($"Transaction failed");
            }
        }

        [HttpPost("[action]")]
        public ActionResult MoneyTransferCreate(MoneyTransferCreateDto dto)
        {
            try
            {
                User userReciever = _context.Users
                              .FirstOrDefault(x => x.IdUser == dto.UserRecieverId);

                if (userReciever == null)
                {
                    return BadRequest($"User with IDUser {dto.UserRecieverId} was not found.");
                }

                User userSender = _context.Users
                           .FirstOrDefault(x => x.IdUser == dto.UserSenderId);

                if (userReciever == null)
                {
                    return BadRequest($"User with IDUser {dto.UserSenderId} was not found.");
                }

               
                TransactionCreateDto transDto = new TransactionCreateDto()
                {
                    UserId = dto.UserSenderId,
                    TransactionTypeId = dto.TransactionTypeId,
                    Amount = dto.Amount,
                    Date = dto.Date
                };

                try
                {
                    TransactionCreate(transDto);
                }
                catch (Exception)
                {

                    return BadRequest($"Transaction failed");
                }

                MoneyTransfer moneyTransfer = new MoneyTransfer()
                {
                    UserRecieverId = dto.UserRecieverId,
                    TransactionId = FkTransactionId
                };

                BillingAccount billingAccount = _context.BillingAccounts.FirstOrDefault(x => x.UserId == dto.UserRecieverId);

                if (billingAccount == null)
                {
                    return BadRequest($"Billing account with UserID {dto.UserRecieverId} was not found.");
                }

                billingAccount.Balance += dto.Amount;
                

                _context.BillingAccounts.Update(billingAccount);
                _context.MoneyTransfers.Add(moneyTransfer);
                _context.SaveChanges();

                return Ok(dto);
            }
            catch (Exception)
            {
                return BadRequest($"Transaction failed");
            }
        }

        [HttpPost("[action]")]
        public ActionResult RequestTransferCreate(RequestTransferDto dto)
        {
            try
            {
                User userReciever = _context.Users
                              .FirstOrDefault(x => x.IdUser == dto.UserRecieverId);

                if (userReciever == null)
                {
                    return BadRequest($"User with IDUser {dto.UserRecieverId} was not found.");
                }

                User userSender = _context.Users
                           .FirstOrDefault(x => x.IdUser == dto.UserSenderId);

                if (userReciever == null)
                {
                    return BadRequest($"User with IDUser {dto.UserSenderId} was not found.");
                }


                RequestTransfer reqTransfer = new RequestTransfer()
                {
                    UserSenderId = dto.UserSenderId,
                    UserRecieverId = dto.UserRecieverId,
                    Amount = dto.Amount,
                    Date = dto.Date
                };

                _context.RequestTransfers.Add(reqTransfer);
                _context.SaveChanges();

                return Ok(dto);
            }
            catch (Exception)
            {
                return BadRequest($"Transaction failed");
            }
        }

        [HttpPut("[action]/{idUser}")]
        public ActionResult AddBalance(AddBalanceDto dto)
        {
            User user = _context.Users
                        .FirstOrDefault(x => x.IdUser == dto.IdUser);

            if (user == null)
            {
                return BadRequest($"User with IDUser {dto.IdUser} was not found.");
            }

            BillingAccount account = _context.BillingAccounts.FirstOrDefault(x => x.UserId == dto.IdUser);

            if (account == null)
            {
                return BadRequest($"Billing account with UserID {dto.IdUser} was not found.");
            }

            TransactionCreateDto transDto = new TransactionCreateDto()
            {
                UserId = dto.IdUser,
                TransactionTypeId = dto.TransactionTypeId,
                Amount = dto.Amount,
                Date = dto.Date,
            };

            try
            {
                TransactionCreate(transDto);
            }
            catch (Exception)
            {

                return BadRequest($"Transaction failed");
            }

            account.Balance += dto.Amount;

            _context.BillingAccounts.Update(account);
            _context.SaveChanges();

            return Ok(dto);
        }


    }
}
