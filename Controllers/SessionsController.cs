using MedicalLaboratoryNumber20WebAPI.Models.Entities;
using MedicalLaboratoryNumber20WebAPI.Models.RequestModels;
using MedicalLaboratoryNumber20WebAPI.Models.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace MedicalLaboratoryNumber20WebAPI.Controllers
{
    public class SessionsController : ApiController
    {
        private readonly MedicalLaboratoryNumber20Entities db =
            new MedicalLaboratoryNumber20Entities();

        // PUT: api/Sessions/edit
        [Route("api/Sessions/edit")]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> EditProfile(RequestPatient requestPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (requestPatient.Credentials.Password == null || requestPatient.Credentials.Password.Length > 100)
            {
                return BadRequest("Пароль обязателен длиной не больше 100 символов");
            }
            if (requestPatient.Email == null || requestPatient.Email.Length > 100)
            {
                return BadRequest("Email обязателен длиной не больше 100 символов");
            }
            if (requestPatient.Phone == null || requestPatient.Phone.Length > 50)
            {
                return BadRequest("Телефон обязателен длиной не больше 50 символов");
            }

            Patient patient = await db.Patient
                .FirstOrDefaultAsync(p => p.PatientLogin.ToLower()
                                          == requestPatient.Credentials.Login.ToLower()
                                          && p.PatientPassword == requestPatient.Credentials.Password);
            if (patient == null)
            {
                return Unauthorized();
            }

            patient.PatientPhone = requestPatient.Phone;
            patient.PatientEmail = requestPatient.Email;
            patient.PatientPassword = requestPatient.Password;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (patient == null)
                {
                    return Unauthorized();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Sessions/login
        [Route("api/Sessions/login")]
        [ResponseType(typeof(ResponsePatient))]
        public async Task<IHttpActionResult> Login(RequestCredentials credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (credentials.Login == null || credentials.Login.Length > 100)
            {
                return BadRequest("Логин обязателен длиной не больше 100 символов");
            }
            if (credentials.Password == null || credentials.Password.Length > 100)
            {
                return BadRequest("Пароль обязателен длиной не больше 100 символов");
            }

            LoginHistory loginHistory = new LoginHistory
            {
                LoginDateTime = DateTime.Now,
                EnteredLogin = credentials.Login,
            };

            Patient patient = await db.Patient
                .FirstOrDefaultAsync(p => p.PatientLogin.ToLower()
                                          == credentials.Login.ToLower()
                                          && p.PatientPassword == credentials.Password);

            loginHistory.IsSuccessful = patient != null;
            db.LoginHistory.Add(loginHistory);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            if (patient == null)
            {
                return Unauthorized();
            }

            return Ok(new ResponsePatient(patient));
        }

        // POST: api/Sessions/register
        [Route("api/Sessions/register")]
        public async Task<IHttpActionResult> Register(RequestPatient requestPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (requestPatient.FullName == null || requestPatient.FullName.Length > 100)
            {
                return BadRequest("ФИО обязательно длиной не больше 100 символов");
            }
            if (requestPatient.Credentials.Login == null || requestPatient.Credentials.Login.Length > 100)
            {
                return BadRequest("Логин обязателен длиной не больше 100 символов");
            }
            if (requestPatient.Credentials.Password == null || requestPatient.Credentials.Password.Length > 100)
            {
                return BadRequest("Пароль обязателен длиной не больше 100 символов");
            }
            if (requestPatient.Email == null || requestPatient.Email.Length > 100)
            {
                return BadRequest("Email обязателен длиной не больше 100 символов");
            }
            if (requestPatient.SecurityNumber == null || requestPatient.SecurityNumber.Length > 50)
            {
                return BadRequest("Email обязателен длиной не больше 50 символов");
            }
            if (requestPatient.Phone == null || requestPatient.Phone.Length > 50)
            {
                return BadRequest("Телефон обязателен длиной не больше 50 символов");
            }
            if (requestPatient.PassportSeries == null || requestPatient.PassportSeries.Length > 4)
            {
                return BadRequest("Серия паспорта обязательна длиной не больше 4 символов");
            }
            if (requestPatient.PassportNumber == null || requestPatient.PassportNumber.Length > 6)
            {
                return BadRequest("Номер паспорта обязателен длиной не больше 6 символов");
            }
            if (requestPatient.BirthDate == null || requestPatient.BirthDate >= DateTime.Now)
            {
                return BadRequest("Дата рождения обязательна и не может быть больше текущей даты");
            }

            Patient patient = await db.Patient
                .FirstOrDefaultAsync(p => p.PatientLogin.ToLower()
                                          == requestPatient.Credentials.Login.ToLower());
            if (patient != null)
            {
                return Conflict();
            }

            Patient newPatient = new Patient
            {
                PatientFullName = requestPatient.FullName,
                PatientLogin = requestPatient.Credentials.Login,
                PatientPassword = requestPatient.Credentials.Password,
                PatientEmail = requestPatient.Email,
                SecurityNumber = requestPatient.SecurityNumber,
                PatientPhone = requestPatient.Phone,
                PassportSeries = requestPatient.PassportSeries,
                PassportNumber = requestPatient.PassportNumber,
                BirthDate = requestPatient.BirthDate,
            };

            db.Patient.Add(newPatient);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return Ok();
        }

        // GET: api/Sessions/research-results
        [Route("api/Sessions/research-results")]
        [ResponseType(typeof(List<ResponseResearchResult>))]
        public async Task<IHttpActionResult> GetResearchResults()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authorizationHeader = HttpContext.Current.Request.Headers["Authorization"];

            if (authorizationHeader == null)
            {
                return Unauthorized();
            }

            if (!Regex.IsMatch(authorizationHeader, @"Basic \w+:\w+"))
            {
                return BadRequest("Authorization token is invalid. " +
                    "It should be in format 'Basic login:password'");
            }

            RequestCredentials credentials;
            credentials = new RequestCredentials
            {
                Login = authorizationHeader.Split(' ')[1].Split(':')[0],
                Password = authorizationHeader.Split(' ')[1].Split(':')[1],
            };

            if (credentials.Login == null || credentials.Login.Length > 100)
            {
                return BadRequest("Логин обязателен длиной не больше 100 символов");
            }
            if (credentials.Password == null || credentials.Password.Length > 100)
            {
                return BadRequest("Пароль обязателен длиной не больше 100 символов");
            }

            Patient patient = await db.Patient
                .FirstOrDefaultAsync(p => p.PatientLogin.ToLower()
                                          == credentials.Login.ToLower()
                                          && p.PatientPassword == credentials.Password);

            if (patient == null)
            {
                return Unauthorized();
            }

            List<BloodServiceOfUser> results = await db.BloodServiceOfUser
                .Include(bs => bs.Service)
                .Include(bs => bs.Blood)
                .Where(bs => bs.Blood.PatientId == patient.PatientId)
                .Where(bs => bs.IsAccepted == true)
                .Where(bs => bs.Result != null)
                .ToListAsync();

            return Ok(results.ConvertAll(bs => new ResponseResearchResult(bs)));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}