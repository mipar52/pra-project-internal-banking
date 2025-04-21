using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;
using PRA_1.Security;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardDatabaseController : ControllerBase
    {

        public readonly PraDbContext _context;

        public CreditCardDatabaseController(PraDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public ActionResult GetCreditCardsDB()
        {
            try
            {
                List<CreditCardDataBase> creditCards = new List<CreditCardDataBase>();

                creditCards = _context.CreditCardDatabases.ToList();    

                return Ok(creditCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CreateCreditCardForDatabase(CreditCardDataBase creditcardDto)
        {
            try
            {
                var b64salt = CvvHashProvider.GetSalt();
                var b64hash = CvvHashProvider.GetHash(creditcardDto.CvvHash, b64salt);

                CreditCardDataBase creditCard = new CreditCardDataBase()
                {
                    FirstName = creditcardDto.FirstName,
                    LastName = creditcardDto.LastName,
                    CardNumber = creditcardDto.CardNumber,
                    ExpiryDate = creditcardDto.ExpiryDate,
                    CvvSalt = b64salt,
                    CvvHash = b64hash,
                };

                if(_context.CreditCardDatabases.Any(x => x.CardNumber == creditcardDto.CardNumber))
                {
                    return BadRequest($"Credit card with a card number {creditcardDto.CardNumber} already exists in credit card world database.");
                }

                if (creditcardDto.CardNumber.Length != 16 || !creditcardDto.CardNumber.All(char.IsDigit))
                {
                    return BadRequest("Caredit card number needs to be 16 digit long and only digits.");
                }

                _context.CreditCardDatabases.Add(creditCard);
                _context.SaveChanges();

                return Ok(creditCard);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpDelete("[action]")]
        public ActionResult DeleteCreditCardFromDatabase(CreditCardDeleteDto creditcardDto)
        {
            try
            {
             
                CreditCardDataBase creditCard = _context.CreditCardDatabases.FirstOrDefault(cc => cc.IdcreditCardDataBase == creditcardDto.IdCreditCard);

                if (creditCard == null)
                {
                    return BadRequest($"Credit Card with card number {creditcardDto.IdCreditCard} was not found.");
                }

                _context.CreditCardDatabases.Remove(creditCard);
                _context.SaveChanges();

                return Ok($"Credit card ending with {creditCard.CardNumber.Substring(creditCard.CardNumber.Length - 4)} has been deleted from world database.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
