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

        //[HttpGet("[action]/{email}")]
        //public ActionResult<List<CreditCard>> GetCreditCardsByEmail(string email)
        //{
        //    try
        //    {
        //        User user = new User();

        //        user = _context.Users.FirstOrDefault(u => u.Email == email);
                
        //        if(user == null) 
        //        {
        //            return BadRequest($"User with E-mail {email} does not exist.");
        //        }

        //        List<CreditCard> creditCards = _context.CreditCards.Where(cc => cc.UserId == user.Iduser).ToList();
              
        //        return Ok(creditCards);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //MOJA (FRANJO) ZAMISAO JE, UKOLIKO NEĆEMO IMPLEMENTIRATI PRAVU KONTROLU KARTICA, DA SE NAPRAVI SIMULIRANA BAZA PODATAKA KARTICA (KOJA ĆE GLUMITI SVJETSKU BAZU SVIH KARTICA) I POMOĆU KOJE ĆE SE PROVJERAVATI JE LI UNESENA KARTICA STVARNA ILI NIJE.
        //RUČNO SE UNOSE SVI PODACI O KARTICI (IME I PREZIME NE MORAJU BITI IDENTIČNI IMENU I PREZIMENU KORISNIKA), A DO USERID-A SE DOLAZI POMOĆU E-MAILA UNESENOG TIJEKOM PRIJAVE
        [HttpPost("[action]")]
        public ActionResult CreateCreditCard(CreditCardCreateDto creditcardDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(u => u.Email == creditcardDto.Email);

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

        //ZA BRISANJE KARTICE POTREBNA SU DVA PODATKA: BROJ KREDITNE KARTICE (JEDINSTVENI PODATAK KOJIM SE LAKO DOLAZI DO TRAŽENE KARTICE) TE EMAIL (JER JEDNU KARTICU MOŽE KORISTITI VIŠE KORISNIKA PA JE POTREBNO ODREDITI KOJEM KORISNIKU ĆE SE KARTICA OBRISATI
        [HttpDelete("[action]")]
        public ActionResult DeleteCreditCard(CreditCardDeleteDto creditcardDto) 
        {
            try
            {
                int IdUser = _context.Users.FirstOrDefault(u => u.Email == creditcardDto.Email).Iduser;
                
                if (IdUser == 0)
                {
                    return BadRequest($"User with {creditcardDto.Email} was not found.");
                }

                CreditCard creditCard = _context.CreditCards.FirstOrDefault(cc => cc.CardNumber == creditcardDto.CardNumber);

                if(creditCard == null)
                {
                    return BadRequest($"Credit Card with card number {creditcardDto.CardNumber} was not found.");
                }

                //UserCreditCard ucc = _context.UserCreditCards.FirstOrDefault(x => x.CreditCardId == creditCard.IdcreditCard && x.UserId == IdUser);

                //if (ucc == null)
                //{
                //    return BadRequest($"Credit card does not exists in M:N table");
                //}

                _context.CreditCards.Remove(creditCard);
                //_context.UserCreditCards.Remove(ucc);
                _context.SaveChanges();
                
                return Ok("Credit card has been deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
