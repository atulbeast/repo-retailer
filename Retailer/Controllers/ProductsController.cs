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
using Retailer.Models.ViewModel;
using Retailer.Common;

namespace Retailer.Controllers
{
    public class ProductsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Products
        public async Task<HttpResponseMessage> GetProduct()
        {
            var product = await db.Product.Include(x=>x.ProductImages).ToListAsync();
            
            return Request.CreateResponse<ResponseModel<List<Product>>>(new ResponseModel<List<Product>> { Status = HttpStatusCode.OK, Data = product, Message = "data loaded" });
        }

        // GET: api/Products/5
        
        public async Task<HttpResponseMessage> GetProduct(long id)
        {
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return Request.CreateResponse<ResponseModel<string>>(new ResponseModel<string> { Status = HttpStatusCode.OK, Message = "data unavailable" });
            }
            return Request.CreateResponse<ResponseModel<Product>>(new ResponseModel<Product> { Status = HttpStatusCode.OK, Data = product, Message = "data updated" });
            
        }


        // GET: api/Products
        [Route("api/ProductBySubCategory")]
        public async Task<HttpResponseMessage> GetProductBySubCategory(long id)
        {
            var product = await db.Product.Where(x=>x.SubCategoryId==id).ToListAsync();
            return Request.CreateResponse<ResponseModel<List<Product>>>(new ResponseModel<List<Product>> { Status = HttpStatusCode.OK, Data = product, Message = "data updated" });
        }




        // PUT: api/Products/5
        public async Task<HttpResponseMessage> PutProduct(long id, ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<ProductModel>>(HttpStatusCode.BadRequest, new ResponseModel<ProductModel> { Status = HttpStatusCode.BadRequest, Data = productModel, Message = Utils.getErrors(ModelState) });
            }

            if (id != productModel.Id)
            {
                return Request.CreateResponse<ResponseModel<ProductModel>>(HttpStatusCode.BadRequest, new ResponseModel<ProductModel> { Status = HttpStatusCode.BadRequest, Data = productModel, Message = "Id doesn't match" });
            }
            var product = new Product();
            product.Amount = productModel.Amount;
            product.Name = productModel.Name;
            product.Total = productModel.Total;
            product.ShopId = productModel.ShopId;
            product.SubCategoryId = productModel.SubCategoryId;
            if(productModel.imageChanged)
            {
                var previousImages=db.ProductImage.Where(x=>x.ProductId==id);
                db.ProductImage.RemoveRange(previousImages);
                var productImages = new List<ProductImage>();
                productImages.AddRange(productModel.ProductUrl.Select(img => new ProductImage { ImageUrl = img, ProductId = product.Id, IsDeleted = false }));
            }
            
            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                return Request.CreateResponse<ResponseModel<Product>>(new ResponseModel<Product> { Status = HttpStatusCode.OK, Data = product, Message = "data updated" });
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProductExists(id))
                {
                    return Request.CreateResponse<ResponseModel<ProductModel>>(HttpStatusCode.BadRequest, new ResponseModel<ProductModel> { Status = HttpStatusCode.BadRequest, Data = productModel, Message = "Problem with project" });
                }
                else
                {
                    return Request.CreateResponse<ResponseModel<ProductModel>>(new ResponseModel<ProductModel> { Status = HttpStatusCode.InternalServerError, Data = productModel, Message = ex.Message.ToString() });
                }
            }

            
        }

        // POST: api/Products
        
        public async Task<HttpResponseMessage> PostProduct(ProductModel productModel)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse<ResponseModel<ProductModel>>(HttpStatusCode.BadRequest, new ResponseModel<ProductModel> { Status = HttpStatusCode.BadRequest, Data = productModel, Message = Utils.getErrors(ModelState) });
            }
            var product = new Product();
            product.Amount=productModel.Amount;
            product.Name = productModel.Name;
            product.Total = productModel.Total;
            product.ShopId = productModel.ShopId;
            product.SubCategoryId = productModel.SubCategoryId;
            db.Product.Add(product);
            //productModel.ProductUrl;
            try
            {
                await db.SaveChangesAsync();
                var productImages = new List<ProductImage>();
                productImages.AddRange(productModel.ProductUrl.Select(img=> new ProductImage {ImageUrl=img,ProductId=product.Id,IsDeleted=false }));
                await db.SaveChangesAsync();
              
                return Request.CreateResponse<ResponseModel<Product>>( new ResponseModel<Product> { Status = HttpStatusCode.OK, Data = product, Message = "data retrieved" });
            }
            catch(Exception ex)
            {
                return Request.CreateResponse<ResponseModel<ProductModel>>(new ResponseModel<ProductModel> { Status = HttpStatusCode.InternalServerError, Data = productModel, Message = ex.Message.ToString() });
            }
            
        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Product))]
        public async Task<HttpResponseMessage> DeleteProduct(long id)
        {
            Product product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return Request.CreateResponse<ResponseModel<string>>(new ResponseModel<string> { Status = HttpStatusCode.NotFound, Message = "id doesn't belong to any product" });
            }
            try
            {
            var productImg = db.ProductImage.Where(x => x.ProductId == id);
            db.ProductImage.RemoveRange(productImg);
            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return Request.CreateResponse<ResponseModel<Product>>(new ResponseModel<Product> { Status = HttpStatusCode.OK, Data = product, Message = "Successfully deleted" });
            }
            catch(Exception ex)
            {
                return Request.CreateResponse<ResponseModel<Product>>(new ResponseModel<Product> { Status = HttpStatusCode.InternalServerError, Data = product, Message = ex.Message.ToString() });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(long id)
        {
            return db.Product.Count(e => e.Id == id) > 0;
        }
    }
}