using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Retailer.Models;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Globalization;
using Retailer.Models.DataModel;
using System.Data.Entity;
namespace Retailer.Controllers
{
    public class CartController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/AddToCart")]
        public async Task<HttpResponseMessage> AddToCart(int id)
        {
            //int cart = 1;
            string UserId = User.Identity.GetUserId();// Convert.ToInt32(Session["UserId"]);
            var order = db.Order.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == UserId && x.status == 1);

            if (order == null)
            {
                //var cp = SignInManager.AuthenticationManager.AuthenticationResponseGrant.Identity.GetUserId();
                //order.
                var prevOrder = db.Order.OrderByDescending(x => x.Id).FirstOrDefault();
                string OrderNo = "RET00001";
                if (prevOrder != null)
                {
                    OrderNo = "RET" + String.Format("{00:00000000}", int.Parse(Regex.Match(prevOrder.OrderNumber, @"\d+").Value, NumberFormatInfo.InvariantInfo) + 1);
                }
                order = db.Order.Add(new Order() { status = 1, UserId = UserId, OrderNumber = OrderNo, IsDeleted = false,CreatedAt=DateTime.Now,UpdatedAt=DateTime.Now,Total=0,Amount=0 });
                db.SaveChanges();
                var lineItem = getItemDetail(id, order.Id);
                lineItem.Quantity = 1;
                db.LineItem.Add(lineItem);
                order.Total = lineItem.Total;
                await db.SaveChangesAsync();
            }
            else
            {
                var matchedItem = db.LineItem.FirstOrDefault(x => x.OrderId == order.Id && x.Id == id);

                if (matchedItem != null)
                {
                    matchedItem.Quantity = matchedItem.Quantity + 1;
                    db.LineItem.Attach(matchedItem);
                    db.Entry(matchedItem).State = EntityState.Modified;
                }
                else
                {
                    var lineItemNew = getItemDetail(id,  order.Id);
                    lineItemNew.Quantity = 1;
                    db.LineItem.Add(lineItemNew);

                }
                await db.SaveChangesAsync();
                order.Total = db.LineItem.Where(x => x.OrderId == order.Id).Sum(x => x.Quantity * x.Amount);
                await  db.SaveChangesAsync();
            }

            return Request.CreateResponse<ResponseModel<Order>>(new ResponseModel<Order> { Status = HttpStatusCode.OK, Data = order, Message = "Product Added to cart" });
        }

        [HttpGet]
        [Route("api/UpdateCart")]
        public async Task<HttpResponseMessage> UpdateCart(int id, int quantity)
        {
            string UserId =User.Identity.GetUserId();
            var Order = db.Order.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == UserId && x.status == 1);
            var Item = Order.LineItems.FirstOrDefault(x => x.Id == id);
            decimal Total = 0;
            int Quantity = 0;
            if (Item != null)
            {
                if (quantity == 0)
                {
                    db.LineItem.Remove(Item);
                }
                else
                {
                    Item.Quantity = quantity;
                    Item.Total = Item.Quantity * Item.Amount;
                }
                db.SaveChanges();
                if (Order.LineItems.Any())
                {
                    Total = Order.LineItems.Sum(x => x.Quantity * x.Amount);
                    Quantity = db.LineItem.Where(x => x.OrderId == Item.OrderId).Sum(x => x.Quantity);
                }

            }
            //if (Order.tblPromoCode != null)
            //{
            //    Order.Total = ((decimal)(100 - Order.tblPromoCode.Discount) * Total / 100);
            //}
            //else
            Order.Total = Total;
            await db.SaveChangesAsync();
            return Request.CreateResponse<ResponseModel<Order>>(new ResponseModel<Order> { Status = HttpStatusCode.OK, Data = Order, Message = "Cart Updated" });
        }

        [HttpGet]
        public async Task<HttpResponseMessage> Index()
        {
            string UserId = User.Identity.GetUserId();
            //var item = new CartView();
            var order = db.Order.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == UserId && x.status == 1);
            //if (order != null)
            //{
            //    item.CartModels = db.tblLineItem.Where(x => x.OrderId == order.Id).Select(x => new CartModel { Discount = (double)(x.Discount), Name = x.Name, Price = x.Price, Quantity = x.Quantity, Category = x.Category, Id = x.Id, Metadata = x.Metadata }).ToList();
            //    if (item.CartModels != null)
            //    {
            //        item.Total = (double)item.CartModels.Sum(x => x.Price * x.Quantity);
            //        item.TotalPrice = item.Total;
            //        item.Count = (int)item.CartModels.Sum(x => x.Quantity);
            //    }
            //    if (order.tblPromoCode != null)
            //    {
            //        item.Total = (100 - order.tblPromoCode.Discount) * item.Total / 100;
            //        item.PromoCode = order.tblPromoCode.Code + "(" + order.tblPromoCode.Discount.ToString() + "%)";
            //    }
            //    item.DeliveryFee = "Free";
            //}
            return Request.CreateResponse<ResponseModel<Order>>(new ResponseModel<Order> { Status = HttpStatusCode.OK, Data = order, Message = "Order" });
        }

        public LineItem getItemDetail(long productId, long orderId)
        {
            var item = db.Product.FirstOrDefault(x => x.Id == productId);
            if (item!= null)
            {
                var lineItem = new LineItem();
                lineItem.OrderId = orderId;
                lineItem.Name = item.Name;
                lineItem.Amount = item.Amount;
                lineItem.ProductId = item.Id;
                lineItem.Total = item.Total;
                return lineItem;
            }
            return null;
        }

    }
}
