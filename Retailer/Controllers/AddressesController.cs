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
using Retailer.Common;

namespace Retailer.Controllers
{
    [Authorize]
    public class AddressesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Addresses
        public async Task<HttpResponseMessage> GetAddress()
        {
            string UserId=User.Identity.GetUserId();
            var addresses = await db.Address.Where(x=>x.UserId==UserId).ToListAsync(); 
            return Request.CreateResponse<ResponseModel<List<Address>>>(new ResponseModel<List<Address>> { Status = HttpStatusCode.OK, Data = addresses, Message = "data successfully loaded" });
        }

        // GET: api/Addresses/5
        [ResponseType(typeof(Address))]
        public async Task<HttpResponseMessage> GetAddress(long id)
        {
            string UserId = User.Identity.GetUserId();
            Address address = await db.Address.FirstOrDefaultAsync(x=>x.UserId==UserId && x.Id==id);
            if (address == null)
            {
                return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.NotFound, Data = address, Message = "invalid address or user" });
            }

            return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.OK, Data = address, Message = "record loaded successfully" });
        }

        // PUT: api/Addresses/5
        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> PutAddress(long id, Address address)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.BadRequest, Data = address, Message = Utils.getErrors(ModelState) });
            }

            if (id != address.Id)
            {
                return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.BadRequest, Data = address, Message = "Id and addressid passed are not same" });
            }

            db.Entry(address).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
                {
                    return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.NotFound, Data = address, Message = "invalid address or user" });
                }
                else
                {
                    return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.InternalServerError, Data = address, Message = "Something went wrong!" });
                }
            }

            return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.OK, Data = address, Message = "record updated successfully" });
        }

        // POST: api/Addresses
        [ResponseType(typeof(Address))]
        public async Task<HttpResponseMessage> PostAddress(Address address)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.BadRequest, Data = address, Message = Utils.getErrors(ModelState) });
            }
            if(address!=null)
            {
                string UserId = User.Identity.GetUserId();
                address.UserId = UserId;
            }
            db.Address.Add(address);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.OK, Data = address, Message = "record updated successfully" });
        }

        // DELETE: api/Addresses/5
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteAddress(long id)
        {
            string UserId = User.Identity.GetUserId();
            Address address = await db.Address.FirstOrDefaultAsync(x => x.UserId == UserId && x.Id == id);
            if (address == null)
            {
                return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.NotFound, Data = address, Message = "invalid address or user" });
            }

            db.Address.Remove(address);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<Address>>(new ResponseModel<Address> { Status = HttpStatusCode.OK, Data = address, Message = "record deleted successfully" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AddressExists(long id)
        {
            return db.Address.Count(e => e.Id == id) > 0;
        }
    }
}