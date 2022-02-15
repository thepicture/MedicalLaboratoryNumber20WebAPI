using MedicalLaboratoryNumber20WebAPI.Models.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace MedicalLaboratoryNumber20WebAPI.Controllers
{
    public class NewsController : ApiController
    {
        private MedicalLaboratoryNumber20Entities db = new MedicalLaboratoryNumber20Entities();

        // GET: api/News
        public IQueryable<News> GetNews()
        {
            return db.News;
        }

        // GET: api/News/5
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> GetNews(int id)
        {
            News news = await db.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            return Ok(news);
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