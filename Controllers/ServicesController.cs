using MedicalLaboratoryNumber20WebAPI.Models.Entities;
using MedicalLaboratoryNumber20WebAPI.Models.ResponseModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MedicalLaboratoryNumber20WebAPI.Controllers
{
    public class ServicesController : ApiController
    {
        private readonly MedicalLaboratoryNumber20Entities db =
            new MedicalLaboratoryNumber20Entities();

        // GET: api/Services
        public IEnumerable<ResponseService> GetService()
        {
            return db.Service.ToList()
                .ConvertAll(s => new ResponseService(s));
        }

        // GET: api/Services/5
        [ResponseType(typeof(Service))]
        public async Task<IHttpActionResult> GetService(int id)
        {
            Service service = await db.Service.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            return Ok(new ResponseService(service));
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