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
using Newtonsoft.Json;
using Retailer.Common;

namespace Retailer.Controllers
{
    public class SubCategoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/SubCategories
        public async Task<HttpResponseMessage> GetSubCategory()
        {
            var subCategories = await db.SubCategory.ToListAsync();
            return Request.CreateResponse<ResponseModel<List<SubCategory>>>(HttpStatusCode.OK, new ResponseModel<List<SubCategory>> { Status = HttpStatusCode.OK, Data = subCategories, Message = "Successfully retrieved" });
            
        }

        // GET: api/SubCategories/5
        [ResponseType(typeof(SubCategory))]
        public async Task<HttpResponseMessage> GetSubCategory(long id)
        {
            SubCategory subCategory = await db.SubCategory.FindAsync(id);
            if (subCategory == null)
            {
                return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.NotFound, new ResponseModel<SubCategory> { Status = HttpStatusCode.NotFound, Data = subCategory, Message = "sub category doesn't exist" });
            }
            return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.OK, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = "Successfully retrieved" });
                
        }

        // PUT: api/SubCategories/5
        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> PutSubCategory(long id, SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
             return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.BadRequest, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = Utils.getErrors(ModelState) });
            }

            if (id != subCategory.Id)
            {
                return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.BadRequest, new ResponseModel<SubCategory> { Status = HttpStatusCode.BadRequest, Data = subCategory, Message = "Passed Id and Model Id are not same" });
            }

            db.Entry(subCategory).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryExists(id))
                {
                    return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.NotFound, new ResponseModel<SubCategory> { Status = HttpStatusCode.NotFound, Data = subCategory, Message = "sub category doesn't exist" });
                }
                else
                {
                    throw;
                }
            }

            return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.OK, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = "Updated successfully" });
        }

        // POST: api/SubCategories
        //[ResponseType(typeof(SubCategory))]
        public async Task<HttpResponseMessage> PostSubCategory(SubCategory subCategory)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.BadRequest, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = Utils.getErrors(ModelState) });
            }

            db.SubCategory.Add(subCategory);
            await db.SaveChangesAsync();
            return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.OK, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = "Added successfully" });
            
        }

        // DELETE: api/SubCategories/5
        [ResponseType(typeof(SubCategory))]
        public async Task<HttpResponseMessage> DeleteSubCategory(long id)
        {
            SubCategory subCategory = await db.SubCategory.FindAsync(id);
            if (subCategory == null)
            {
                return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.NotFound, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = "Record doesn't exist" });
            }

            db.SubCategory.Remove(subCategory);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<SubCategory>>(HttpStatusCode.OK, new ResponseModel<SubCategory> { Status = HttpStatusCode.OK, Data = subCategory, Message = "Data successfully deleted" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SubCategoryExists(long id)
        {
            return db.SubCategory.Count(e => e.Id == id) > 0;
        }
    }
}