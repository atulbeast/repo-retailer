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
using Microsoft.AspNet.Identity;
using Retailer.Models.DataModel;

namespace Retailer.Controllers
{
    [Authorize]
    public class ProfilesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Profiles
        public async Task<HttpResponseMessage> GetProfile()
        {
            string UserId = User.Identity.GetUserId();
            
            var user = await db.Users.Include(x=>x.Profile).FirstOrDefaultAsync(x => x.Id == UserId);
            if(user.Profile!=null)
                return Request.CreateResponse<ResponseModel<Profile>>(new ResponseModel<Profile> { Status = HttpStatusCode.OK, Data = user.Profile, Message = "data successfully loaded" });

            return Request.CreateResponse<ResponseModel<Profile>>(new ResponseModel<Profile> { Status = HttpStatusCode.NotFound, Data = null, Message = "Profile doesn't exist" });
        }

        // GET: api/Profiles/5
        [ResponseType(typeof(Profile))]
        public async Task<IHttpActionResult> GetProfile(long id)
        {
            Profile profile = await db.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        // PUT: api/Profiles/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProfile(long id, Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != profile.Id)
            {
                return BadRequest();
            }

            db.Entry(profile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        [ResponseType(typeof(Profile))]
        public async Task<IHttpActionResult> PostProfile(Profile profile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Profile.Add(profile);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = profile.Id }, profile);
        }

        // DELETE: api/Profiles/5
        [ResponseType(typeof(Profile))]
        public async Task<IHttpActionResult> DeleteProfile(long id)
        {
            Profile profile = await db.Profile.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            db.Profile.Remove(profile);
            await db.SaveChangesAsync();

            return Ok(profile);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProfileExists(long id)
        {
            return db.Profile.Count(e => e.Id == id) > 0;
        }
    }
}