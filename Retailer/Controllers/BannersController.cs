using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Retailer.Models;
using Retailer.Models.DataModel;
using Retailer.Common;

namespace Retailer.Controllers
{
    public class BannersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Banners
        public async Task<HttpResponseMessage> GetBanner()
        {
            var banner=await db.Banner.ToListAsync(); ;
            return Request.CreateResponse<ResponseModel<List<Banner>>>(HttpStatusCode.OK, new ResponseModel<List<Banner>> { Status = HttpStatusCode.OK, Data = banner, Message = "data successfully loaded" });
        }

        // GET: api/Banners/5
        public async Task<HttpResponseMessage> GetBanner(long id)
        {
            Banner banner = await db.Banner.FindAsync(id);
            if (banner == null)
            {
                return Request.CreateResponse<ResponseModel<string>>(HttpStatusCode.NotFound, new ResponseModel<string> { Status = HttpStatusCode.NotFound,  Message = "category doesn't exist" });
            }
            return Request.CreateResponse<ResponseModel<Banner>>(HttpStatusCode.OK, new ResponseModel<Banner> { Status = HttpStatusCode.OK, Data = banner, Message = "Successfully retrieved" });
            
        }

        // PUT: api/Banners/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBanner(long id, Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != banner.Id)
            {
                return BadRequest();
            }

            db.Entry(banner).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BannerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Banners
        [ResponseType(typeof(Banner))]
        public async Task<HttpResponseMessage> PostBanner(Banner banner)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<Banner>>(new ResponseModel<Banner> { Status = HttpStatusCode.NotFound, Data = banner, Message = Utils.getErrors(ModelState) });
            }

            db.Banner.Add(banner);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<Banner>>(new ResponseModel<Banner> { Status = HttpStatusCode.OK, Data = banner, Message = "added successfully" });
        }

        // DELETE: api/Banners/5
        [ResponseType(typeof(Banner))]
        public async Task<IHttpActionResult> DeleteBanner(long id)
        {
            Banner banner = await db.Banner.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }

            db.Banner.Remove(banner);
            await db.SaveChangesAsync();

            return Ok(banner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BannerExists(long id)
        {
            return db.Banner.Count(e => e.Id == id) > 0;
        }
    }
}