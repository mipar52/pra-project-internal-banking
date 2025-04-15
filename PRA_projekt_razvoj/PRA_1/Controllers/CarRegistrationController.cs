using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.DTOs;
using PRA_1.Models;

namespace PRA_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarRegistrationController : ControllerBase
    {
        public readonly PraDbContext _context;

        public CarRegistrationController(PraDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public ActionResult GetCarRegistrations()
        {
            try
            {
                List<CarRegistration> carregistrations = new List<CarRegistration>();

                carregistrations = _context.CarRegistrations.ToList();

                return Ok(carregistrations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            
        }

        [HttpGet("[action]/{email}/{id}")]
        public ActionResult GetCarRegistrationsById(string email, int id)
        {
            try
            {
                User user = new User();

                user = _context.Users.FirstOrDefault(x => x.Email == email);

                if (user == null)
                {
                    return BadRequest($"User with E-mail {email} does not exist.");
                }

                CarRegistration carRegistration = new CarRegistration();

                carRegistration = _context.CarRegistrations.FirstOrDefault(x => x.IdcarRegistration == id);

                //List<int> carRegistrationsId = _context.UserCarRegistrations.Where(x => x.UserId == user.Iduser).Select(x => x.CarRegistrationId).ToList();

                //List<CarRegistration> carRegistrations = new List<CarRegistration>();

                //CarRegistration carreg = new CarRegistration();

                //foreach (var carRegistrationId in carRegistrationsId)
                //{

                //    carreg = _context.CarRegistrations.FirstOrDefault(x => x.IdcarRegistration == carRegistrationId);

                //    if (carreg == null)
                //    {
                //        return BadRequest($"Car registration with {carRegistrationId} was not found.");
                //    }

                //    carRegistrations.Add(carreg);
                //}

                return Ok(carRegistration);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpGet("[action]/{email}")]
        public ActionResult GetCarRegistrationsByEmail(string email)
        {
            try
            {
                User user = new User();

                user = _context.Users.FirstOrDefault(x => x.Email == email);

                if (user == null)
                {
                    return BadRequest($"User with E-mail {email} does not exist.");
                }

                List<int> carRegistrationsId = _context.UserCarRegistrations.Where(x => x.UserId == user.Iduser).Select(x => x.CarRegistrationId).ToList();
                
                List<CarRegistration> carRegistrations = new List<CarRegistration>();

                CarRegistration carreg = new CarRegistration();

                foreach(var carRegistrationId in carRegistrationsId)
                {
                    
                    carreg = _context.CarRegistrations.FirstOrDefault(x => x.IdcarRegistration == carRegistrationId);

                    if(carreg == null)
                    {
                        return BadRequest($"Car registration with {carRegistrationId} was not found.");
                    }

                    carRegistrations.Add(carreg);
                }

                return Ok(carRegistrations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }

        [HttpPost("[action]")]
        public ActionResult CreateCarRegistration(CarRegistrationCreateDto carRegistrationDto)
        {
            try
            {
                User user = new User();

                user = _context.Users.FirstOrDefault(x => x.Email == carRegistrationDto.Email);

                if (user == null)
                {
                    return BadRequest($"User with E-mail {carRegistrationDto.Email} does not exist.");
                }

                CarRegistration carRegistration = new CarRegistration()
                {
                    CarBrand = carRegistrationDto.CarBrand,
                    CarModel = carRegistrationDto.CarModel,
                    RegistrationCountry = carRegistrationDto.RegistrationCountry.ToUpper(),
                    RegistrationNumber = carRegistrationDto.RegistrationNumber.ToUpper()
                };

                _context.CarRegistrations.Add(carRegistration);
                _context.SaveChanges();


                //int idCarRegistration = _context.CarRegistrations.FirstOrDefault(x => x.RegistrationCountry == carRegistrationDto.RegistrationCountry.ToUpper() && x.RegistrationNumber == carRegistrationDto.RegistrationNumber.ToUpper()).IdcarRegistration;

                if (carRegistration.IdcarRegistration == 0)
                {
                    return BadRequest("No id found");
                }

                UserCarRegistration userCarRegistration = new UserCarRegistration()
                {
                    UserId = user.Iduser,
                    CarRegistrationId = carRegistration.IdcarRegistration
                };

                if (!_context.UserCarRegistrations.Any(x => x.UserId == user.Iduser && x.CarRegistrationId == carRegistration.IdcarRegistration))
                {

                    _context.UserCarRegistrations.Add(userCarRegistration);
                    _context.SaveChanges();

                    return Ok($"A vehicle with a car plate {carRegistration.RegistrationNumber} has been successfully added to a user {user.FirstName} {user.LastName}");
                }

                return Ok($"The vehicle with a car registration {carRegistration.RegistrationCountry} {carRegistration.RegistrationNumber} has already been added to a user with an e-mail {user.Email}");
            } 
            catch (Exception ex) {

                return StatusCode(500, ex.Message);
            }
        }
            

        [HttpPut("[action]")]
        public ActionResult UpdateCarRegistration(CarRegistrationUpdateDto carRegistrationUpdateDto)
        {
            try
            {

                User user = _context.Users.FirstOrDefault(x => x.Email == carRegistrationUpdateDto.Email);

                if (user == null)
                {
                    return BadRequest($"User with E-mail {carRegistrationUpdateDto.Email} does not exist.");
                }

                CarRegistration carRegistration = _context.CarRegistrations.FirstOrDefault(x => x.IdcarRegistration == carRegistrationUpdateDto.IDCarRegistration);

                if ((carRegistration == null))
                {
                    return BadRequest($"Car with an IDCarRegistration {carRegistrationUpdateDto.IDCarRegistration} does not exist.");
                }

                //if (_context.CarRegistrations.Any(x => x.RegistrationCountry == carRegistrationUpdateDto.RegistrationCountry.ToUpper() && x.RegistrationCountry == carRegistrationUpdateDto.RegistrationNumber.ToUpper()))
                //{
                //    return BadRequest($"User {user.FirstName} {user.LastName} already has a vehicle with a country code {carRegistrationUpdateDto.RegistrationCountry} and registration number {carRegistrationUpdateDto.RegistrationNumber} in the database.");
                //}

                if (carRegistrationUpdateDto.CarBrand != "string") carRegistration.CarBrand = carRegistrationUpdateDto.CarBrand;
                if (carRegistrationUpdateDto.CarModel != "string") carRegistration.CarModel = carRegistrationUpdateDto.CarModel;
                if (carRegistrationUpdateDto.RegistrationCountry != "string") carRegistration.RegistrationCountry = carRegistrationUpdateDto.RegistrationCountry.ToUpper();
                if (carRegistrationUpdateDto.RegistrationNumber != "string") carRegistration.RegistrationNumber = carRegistrationUpdateDto.RegistrationNumber.ToUpper();

                _context.CarRegistrations.Update(carRegistration);
                _context.SaveChanges();

                return Ok($"Vehicle with IDCarRegistration {carRegistration.IdcarRegistration} was succesfully updated");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("[action]")]
        public ActionResult DeleteCarRegistration(CarRegistrationDeleteDto carRegistrationDeleteDto)
        {
            try
            {
                User user = _context.Users.FirstOrDefault(x => x.Email == carRegistrationDeleteDto.Email);

                if (user == null)
                {
                    return BadRequest($"User with E-mail {carRegistrationDeleteDto.Email} does not exist.");
                }

                CarRegistration carRegistration = _context.CarRegistrations.FirstOrDefault(x => x.IdcarRegistration == carRegistrationDeleteDto.IDCarRegistration);

                if (carRegistration == null)
                {
                    return BadRequest($"Car with an IDCarRegistration {carRegistrationDeleteDto.IDCarRegistration} does not exist.");
                }

                UserCarRegistration userCarRegistration = _context.UserCarRegistrations.FirstOrDefault(x => x.UserId == user.Iduser && x.CarRegistrationId == carRegistration.IdcarRegistration);

                if (userCarRegistration == null)
                {
                    return BadRequest($"A user {user.FirstName} {user.LastName} does not have a car with a car registration {carRegistration.RegistrationCountry} {carRegistration.RegistrationNumber} in the database.");
                }

                _context.UserCarRegistrations.Remove(userCarRegistration);
                _context.SaveChanges();

                List<UserCarRegistration> usersCarRegistrations = _context.UserCarRegistrations.ToList();

                if (!usersCarRegistrations.Any(x => x.CarRegistrationId == carRegistration.IdcarRegistration))
                {
                    _context.CarRegistrations.Remove(carRegistration);
                    _context.SaveChanges();
                }

                return Ok($"A car with a {carRegistration.RegistrationCountry} {carRegistration.RegistrationNumber} of a user {user.FirstName} {user.LastName} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
