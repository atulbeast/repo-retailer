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
using Microsoft.AspNet.Identity;
using Retailer.Common;
namespace Retailer.Controllers
{
    [Authorize]
    public class ShopsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Shops
        public async Task<HttpResponseMessage> GetShop()
        {
            var shop= await db.Shop.ToListAsync();
            return Request.CreateResponse<ResponseModel<List<Shop>>>(HttpStatusCode.OK, new ResponseModel<List<Shop>> { Status = HttpStatusCode.OK, Data = shop, Message = "data retrieved" });
        }

        // GET: api/Shops/5
        [ResponseType(typeof(Shop))]
        public async Task<HttpResponseMessage> GetShop(long id)
        {
            Shop shop = await db.Shop.FindAsync(id);
            if (shop == null)
            {
                return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.NotFound, new ResponseModel<Shop> { Status = HttpStatusCode.NotFound, Data = shop, Message = "record doesn't exist" });
            }

            return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.OK, new ResponseModel<Shop> { Status = HttpStatusCode.OK, Data = shop, Message = "data retrieved" });
        }

        [Route("UserShop")]
        public async Task<HttpResponseMessage> UserShop(long id)
        {
            string UserId = User.Identity.GetUserId();
            var shop = await db.Shop.Where(x=>x.UserId==UserId).ToListAsync();
            if (shop == null)
            {
                return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.NotFound, new ResponseModel<Shop> { Status = HttpStatusCode.NotFound,  Message = "record doesn't exist" });
            }

            return Request.CreateResponse<ResponseModel<List<Shop>>>(new ResponseModel<List<Shop>> { Status = HttpStatusCode.OK, Data = shop, Message = "data retrieved" });
        }

        // PUT: api/Shops/5
        public async Task<HttpResponseMessage> PutShop(long id, Shop shop)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.BadRequest, new ResponseModel<Shop> { Status = HttpStatusCode.BadRequest, Data = shop, Message = Utils.getErrors( ModelState) });
            }

            if (id != shop.Id)
            {
                return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.BadRequest, new ResponseModel<Shop> { Status = HttpStatusCode.BadRequest, Data = shop, Message = "Model id and id passed are not same" });
            }

            db.Entry(shop).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopExists(id))
                {
                    return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.InternalServerError, new ResponseModel<Shop> { Status = HttpStatusCode.OK, Data = shop, Message = "Error while saving the record" });
                }
                else
                {
                    throw;
                }
            }

            return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.OK, new ResponseModel<Shop> { Status = HttpStatusCode.OK, Data = shop, Message = "data successfully modified" });
        }

        // POST: api/Shops
        [ResponseType(typeof(Shop))]
        public async Task<HttpResponseMessage> PostShop(Shop shop)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.BadRequest, new ResponseModel<Shop> { Status = HttpStatusCode.BadRequest, Data = shop, Message = Utils.getErrors(ModelState) });
            }

            if(shop!=null)
            {
            shop.UserId = User.Identity.GetUserId();
            db.Shop.Add(shop);
            await db.SaveChangesAsync();
            return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.OK, new ResponseModel<Shop> { Status = HttpStatusCode.OK, Data = shop, Message = "data successfully created" });
                
            }

            return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.BadRequest, new ResponseModel<Shop> { Status = HttpStatusCode.BadRequest, Data = shop, Message = "Shop data is not valid" });

            
        }

        // DELETE: api/Shops/5
        [ResponseType(typeof(Shop))]
        public async Task<HttpResponseMessage> DeleteShop(long id)
        {
            Shop shop = await db.Shop.FindAsync(id);
            if (shop == null)
            {
                return Request.CreateResponse<ResponseModel<Shop>>(HttpStatusCode.NotFound, new ResponseModel<Shop> { Status = HttpStatusCode.NotFound, Data = shop, Message = "record doesn't exist" });
            }

            db.Shop.Remove(shop);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<Shop>>( new ResponseModel<Shop> { Status = HttpStatusCode.OK, Data = shop, Message = "data successfully deleted" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ShopExists(long id)
        {
            return db.Shop.Count(e => e.Id == id) > 0;
        }
    }
}