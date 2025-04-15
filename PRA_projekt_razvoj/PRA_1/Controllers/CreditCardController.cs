using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;
using PRA_1.Security;
using Twilio.Rest.Chat.V1.Service;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        public readonly PraDbContext _context;

        public CreditCardController (PraDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public ActionResult<List<CreditCard>> GetCreditCards()
        {
            try
            {
                List<CreditCard> creditCards = new List<CreditCard>();

                creditCards = _context.CreditCards.ToList();

                return Ok(creditCards);
            } 
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("[action]/{id}")]
        //public ActionResult<List<CreditCard>> GetCreditCardsById(int idUser)
        //{
        //    try
        //    {
        //        List<CreditCard> creditCards = new List<CreditCard>();

        //        creditCards = _context.CreditCards.Where(cc => cc.UserId == idUser).ToList();

        //        return Ok(creditCards);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpGet("[action]/{email}")]
        public ActionResult<List<CreditCard>> GetCreditCardsByEmail(string email)
        {
            try
            {
                User user = new User();

                user = _context.Users.FirstOrDefault(x => x.Email == email);

                if (user == null)
                {
                    return BadRequest($"User with E-mail {email} does not exist.");
                }

                List<int> creditCardsId = _context.UserCreditCards.Where(x => x.UserId == user.Iduser).Select(x => x.CreditCardId).ToList();

                List<CreditCard> creditCards = new List<CreditCard>();

                foreach(int creditCardId in creditCardsId)
                {
                    CreditCard card = _context.CreditCards.FirstOrDefault(x => x.IdcreditCard == creditCardId);

                    if(card == null)
                    {
                        return BadRequest($"Credit card with IDCard {creditCardId} was not found.");
                    }

                    creditCards.Add(card);
                }
            
                return Ok(creditCards);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult CreateCreditCard(CreditCardCreateDto creditcardDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Email == creditcardDto.Email);

                if(User == null)
                {
                    return BadRequest($"User with {creditcardDto.Email} was not found.");
                }

                var b64salt = CvvHashProvider.GetSalt();
                var b64hash = CvvHashProvider.GetHash(creditcardDto.CvvHash, b64salt);

                CreditCard creditCard = new CreditCard()
                {
                    FirstName = creditcardDto.FirstName,
                    LastName = creditcardDto.LastName,
                    CardNumber = creditcardDto.CardNumber,
                    ExpiryDate = creditcardDto.ExpiryDate,
                    CvvSalt = b64salt,
                    CvvHash = b64hash,
                };

                List<CreditCardDataBase> creditCardDb = new List<CreditCardDataBase>();
                creditCardDb = _context.CreditCardDatabases.ToList();

                foreach (var creditCardDbEntry in creditCardDb)
                {
                 
                    if (creditCard.FirstName == creditCardDbEntry.FirstName && creditCard.LastName == creditCardDbEntry.LastName && creditCard.ExpiryDate == creditCardDbEntry.ExpiryDate && creditcardDto.CardNumber == creditCardDbEntry.CardNumber)
                    {

                        var b64hashdb = PasswordHashProvider.GetHash(creditcardDto.CvvHash, creditCardDbEntry.CvvSalt);
                        if (b64hashdb != creditCardDbEntry.CvvHash)
                        return BadRequest("Wrong CVV");

                        if (!_context.CreditCards.Any(x => x.CardNumber == creditcardDto.CardNumber))
                        {
                            _context.CreditCards.Add(creditCard);
                            _context.SaveChanges();
                        }

                        CreditCard savedCreditCard = _context.CreditCards.FirstOrDefault(cc => cc.CardNumber == creditCard.CardNumber);
                        User savedUser = _context.Users.FirstOrDefault(u => u.Email == creditcardDto.Email);

                        if (savedCreditCard == null || savedUser == null)
                        {
                            return BadRequest("IDCreditCard or IDUser were not found");
                        }

                        UserCreditCard ucc = new UserCreditCard()
                        {
                            UserId = savedUser.Iduser,
                            CreditCardId = savedCreditCard.IdcreditCard
                        };

                        if(_context.UserCreditCards.Any(x => x.UserId == savedUser.Iduser && x.CreditCardId == savedCreditCard.IdcreditCard))
                        {
                            return BadRequest($"User {savedUser.FirstName} {savedUser.LastName} already has a credit card ending with {creditCard.CardNumber.Substring(creditCard.CardNumber.Length - 4)} saved.");
                        }

                        _context.UserCreditCards.Add(ucc);
                        _context.SaveChanges();

                        return Ok($"Credit card for a person {user.FirstName} {user.LastName} has been saved");
                    }

                    
                }
                
                return BadRequest("Credit card data is wrong. Credit Card does not exist.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpDelete("[action]")]
        public ActionResult DeleteCreditCard(CreditCardDeleteDto creditcardDto) 
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Email == creditcardDto.Email);
                
                if (User == null)
                {
                    return BadRequest($"User with {creditcardDto.Email} was not found.");
                }

                CreditCard creditCard = _context.CreditCards.FirstOrDefault(cc => cc.IdcreditCard == creditcardDto.IdCreditCard);

                if(creditCard == null)
                {
                    return BadRequest($"Credit Card with card number {creditcardDto.IdCreditCard} was not found.");
                }

                UserCreditCard ucc = _context.UserCreditCards.FirstOrDefault(x => x.CreditCardId == creditCard.IdcreditCard && x.UserId == user.Iduser);

                if (ucc == null)
                {
                    return BadRequest($"Credit card does not exists in M:N table");
                }

                _context.UserCreditCards.Remove(ucc);
                _context.SaveChanges();

                if(!_context.UserCreditCards.Any(x => x.CreditCardId == creditCard.IdcreditCard)) 
                {
                    _context.CreditCards.Remove(creditCard);
                    _context.SaveChanges();
                }
                
                return Ok($"Credit card ending with {creditCard.CardNumber.Substring(creditCard.CardNumber.Length - 4)} has been deleted for a user {user.FirstName} {user.LastName}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
