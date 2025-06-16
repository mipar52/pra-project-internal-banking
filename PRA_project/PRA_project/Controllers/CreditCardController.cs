using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.Security;
using PRA_project.DTOs;
using PRA_project.Models;

namespace PRA_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditCardController : ControllerBase
    {
        private readonly PraDatabaseContext _context;
        private readonly IConfiguration _configuration;

        public CreditCardController(PraDatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpGet("[action]/{idUser}")]
        public ActionResult GetCardsById(int idUser)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.IdUser == idUser);


                if (user == null)
                {
                    return BadRequest($"User with IDUser {idUser} was not found.");
                }

                List<int?> creditCardIds = _context.UserCreditCards
                    .Where(x => x.UserId == user.IdUser)
                    .Select(x => x.CreditCardId)
                    .ToList();

                List<CreditCardGetDto> creditCards = _context.CreditCards
                    .Where(cc => creditCardIds.Contains(cc.IdCreditCard))
                    .Select(cc => new CreditCardGetDto
                    {
                        FirstName = cc.FirstName,
                        Lastname = cc.Lastname,
                        CreditCardNumber = cc.CreditCardNumber,
                        ExpiryDate = cc.ExpiryDate
                    })
                    .ToList();

                return Ok(creditCards);
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }

        }

        [HttpGet("[action]/{email}")]
        public ActionResult GetCardsByMail(string email)
        {
            try
            {
                var user = _context.Users.FirstOrDefault(x => x.EmailAddress == email);


                if (user == null)
                {
                    return BadRequest($"User with email {email} was not found.");
                }

                List<int?> creditCardIds = _context.UserCreditCards
                    .Where(x => x.UserId == user.IdUser)
                    .Select(x => x.CreditCardId)
                    .ToList();

                List<CreditCardGetDto> creditCards = _context.CreditCards
                    .Where(cc => creditCardIds.Contains(cc.IdCreditCard))
                    .Select(cc => new CreditCardGetDto
                    {
                        FirstName = cc.FirstName,
                        Lastname = cc.Lastname,
                        CreditCardNumber = cc.CreditCardNumber,
                        ExpiryDate = cc.ExpiryDate
                    })
                    .ToList();

                return Ok(creditCards);
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }

        }

        [HttpPost("[action]")]
        public ActionResult CreateCard(CreditCardCreateDto dto)
        {
            try
            {
                try
                {
                    User user = _context.Users.FirstOrDefault(x => x.IdUser == dto.UserId);

                    if (User == null)
                    {
                        return BadRequest($"User with {dto.UserId} was not found.");
                    }

                    var b64salt = CvvHashProvider.GetSalt();
                    var b64hash = CvvHashProvider.GetHash(dto.Cvv, b64salt);

                    CreditCard creditCard = new CreditCard()
                    {
                        FirstName = dto.FirstName,
                        Lastname = dto.LastName,
                        CreditCardNumber = dto.CreditCardNumber,
                        ExpiryDate = dto.ExpiryDate,
                        Cvvsalt = b64salt,
                        Cvvhash = b64hash,
                    };

                    _context.CreditCards.Add(creditCard);
                    _context.SaveChanges();

                    UserCreditCard userCreditCard = new UserCreditCard()
                    {
                        UserId = user.IdUser,
                        CreditCardId = creditCard.IdCreditCard
                    };

                    _context.UserCreditCards.Add(userCreditCard);
                    _context.SaveChanges();

                    return Ok(dto);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }

        }

        [HttpPost("[action]")]
        public ActionResult CreateCardByMail(CreditCardCreateMailDto dto)
        {
            try
            {
                try
                {
                    var user = _context.Users.FirstOrDefault(x => x.EmailAddress == dto.UserMail);

                    if (user == null)
                    {
                        return BadRequest($"User with email {dto.UserMail} was not found.");
                    }

                    var b64salt = CvvHashProvider.GetSalt();
                    var b64hash = CvvHashProvider.GetHash(dto.Cvv, b64salt);

                    CreditCard creditCard = new CreditCard()
                    {
                        FirstName = dto.FirstName,
                        Lastname = dto.LastName,
                        CreditCardNumber = dto.CreditCardNumber,
                        ExpiryDate = dto.ExpiryDate,
                        Cvvsalt = b64salt,
                        Cvvhash = b64hash,
                    };

                    _context.CreditCards.Add(creditCard);
                    _context.SaveChanges();

                    UserCreditCard userCreditCard = new UserCreditCard()
                    {
                        UserId = user.IdUser,
                        CreditCardId = creditCard.IdCreditCard
                    };

                    _context.UserCreditCards.Add(userCreditCard);
                    _context.SaveChanges();

                    return Ok(dto);

                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            catch (Exception)
            {

                return BadRequest($"Friend failed");
            }

        }

        [HttpDelete("[action]/{UserId}/{CreditCardId}")]
        public ActionResult DeleteCard(CreditCardDeleteDto dto)
        {
            User user = _context.Users.FirstOrDefault(x => x.IdUser == dto.UserId);

            if (User == null)
            {
                return BadRequest($"User with {dto.UserId} was not found.");
            }

            CreditCard cc = _context.CreditCards.FirstOrDefault(x => x.IdCreditCard  == dto.CreditCardId);

            if (cc == null)
            {
                return BadRequest($"Credit card with IDCreditCard {dto.CreditCardId} was not found."); 
            }

            UserCreditCard userCreditCard = _context.UserCreditCards.FirstOrDefault(x => x.CreditCardId == cc.IdCreditCard && x.UserId == user.IdUser);

            if (userCreditCard == null)
            {
                return BadRequest($"NEMA DALJE");
            }

            _context.UserCreditCards.Remove(userCreditCard);
            _context.SaveChanges();

            return Ok(dto);
        }

        [HttpDelete("[action]/{email}/{CreditCardNumber}")]
        public ActionResult DeleteCardByNumberAndMail(CreditCardStringDeleteDto dto)
        {
            var user = _context.Users.FirstOrDefault(x => x.EmailAddress == dto.UserEmail);

            if (User == null)
            {
                return BadRequest($"User with email {dto.UserEmail} was not found.");
            }

            var cc = _context.CreditCards.FirstOrDefault(x => x.CreditCardNumber == dto.CreditCardNumber);

            if (cc == null)
            {
                return BadRequest($"Credit card with number {dto.CreditCardNumber} was not found.");
            }

            UserCreditCard userCreditCard = _context.UserCreditCards.FirstOrDefault(x => x.CreditCardId == cc.IdCreditCard && x.UserId == user.IdUser);

            if (userCreditCard == null)
            {
                return BadRequest($"NEMA DALJE");
            }

            _context.UserCreditCards.Remove(userCreditCard);
            _context.SaveChanges();

            return Ok(dto);
        }

    }
}
