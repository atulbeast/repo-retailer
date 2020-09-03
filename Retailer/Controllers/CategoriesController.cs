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
using System.Web;
using System.IO;
using Retailer.Common;

namespace Retailer.Controllers
{
    public class CategoriesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Categories
        public async Task<HttpResponseMessage> GetCategory()
        {
            var categories =await db.Category.ToListAsync();
            return Request.CreateResponse<ResponseModel<List<Category>>>(HttpStatusCode.OK, new ResponseModel<List<Category>> { Status = HttpStatusCode.OK, Data = categories, Message = "data successfully loaded" });
            
        }

        // GET: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<HttpResponseMessage> GetCategory(long id)
        {
            Category category = await db.Category.FindAsync(id);
            if (category == null)
            {
                return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.NotFound, new ResponseModel<Category> { Status = HttpStatusCode.NotFound, Data = category, Message = "category doesn't exist" });
            }

            return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.OK, new ResponseModel<Category> { Status = HttpStatusCode.OK, Data = category, Message = "Successfully retrieved" });
        }

        // PUT: api/Categories/5
        [ResponseType(typeof(void))]
        public async Task<HttpResponseMessage> PutCategory(long id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.BadRequest, new ResponseModel<Category> { Status = HttpStatusCode.NotFound, Data = category, Message = Utils.getErrors(ModelState) });
            }

            if (id != category.Id)
            {
                return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.BadRequest, new ResponseModel<Category> { Status = HttpStatusCode.NotFound, Data = category, Message = "Id and model id doesn't match" });
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.NotFound, new ResponseModel<Category> { Status = HttpStatusCode.NotFound, Data = category, Message = "Record doesn't exist" });
                }
                else
                {
                    throw;
                }
            }

            return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.OK, new ResponseModel<Category> { Status = HttpStatusCode.OK, Data = category, Message = "Successfully updated" });
        }

        // POST: api/Categories
        [ResponseType(typeof(Category))]
        public async Task<HttpResponseMessage> PostCategory(Category category)
        {
            
            var httpRequest = HttpContext.Current.Request;
            if(httpRequest.Form["Name"]!=null)
            {
                string ImgUrl = Utils.AddImage("Category");
                if(ImgUrl.IndexOf("Images")==-1)
                {
                    return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.BadRequest, new ResponseModel<Category> { Status = HttpStatusCode.BadRequest, Data = category, Message = ImgUrl });
                }
                category = new Category();
                category.Url = ImgUrl;
                category.IsDeleted = false;
                category.Name=(string)httpRequest.Form["Name"];
            }
            else 
            {
                return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.BadRequest, new ResponseModel<Category> { Status = HttpStatusCode.BadRequest, Data = category, Message = Utils.getErrors(ModelState)});
            }

            db.Category.Add(category);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.OK, new ResponseModel<Category> { Status = HttpStatusCode.OK, Data = category, Message = "Added Successfully"});
        }

        // DELETE: api/Categories/5
        [ResponseType(typeof(Category))]
        public async Task<HttpResponseMessage> DeleteCategory(long id)
        {
            Category category = await db.Category.FindAsync(id);
            if (category == null)
            {
                return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.NotFound, new ResponseModel<Category> { Status = HttpStatusCode.NotFound, Data = category, Message = "Record doesn't exist" });
            }

            db.Category.Remove(category);
            await db.SaveChangesAsync();

            return Request.CreateResponse<ResponseModel<Category>>(HttpStatusCode.OK, new ResponseModel<Category> { Status = HttpStatusCode.OK, Data = category, Message = "Successfully Deleted" });
        }



        //public async Task<string> AddImage(string folderName)
        //{
        //    var httpRequest = HttpContext.Current.Request;
        //    if (httpRequest.Files.Count > 0)
        //    {
        //        try
        //        {
        //            //  Get all files from Request object  
        //            HttpFileCollection files = httpRequest.Files;
        //            var imgUrl = new List<string>();
        //            for (int i = 0; i < files.Count; i++)
        //            {

        //                HttpPostedFile file = files[i];
        //                string fname= file.FileName;
                        
        //                string subPath = @"/Images/"+folderName;

        //                bool exists = System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

        //                if (!exists)
        //                    System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath(subPath));

        //                // Get the complete folder path and store the file inside it.  
        //                fname = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(subPath), DateTime.Now.ToFileTime() + "-" + fname);
        //                file.SaveAs(fname);
        //                int pos = fname.IndexOf("\\Images");
        //                imgUrl.Add(fname.Substring(pos));
        //            }
        //            return string.Join("", imgUrl);
        //            //return Json("Something went wrong in upload");
        //            // Returns message that successfully uploaded  

        //        }
        //        catch (Exception ex)
        //        {
        //            return "Error occurred. Error details: " + ex.Message;
        //        }
        //    }
        //    else
        //    {
        //        return "No files selected.";
        //    }

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(long id)
        {
            return db.Category.Count(e => e.Id == id) > 0;
        }
    }
}