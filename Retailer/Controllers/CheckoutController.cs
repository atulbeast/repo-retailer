using Retailer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Retailer.Models.DataModel;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace Retailer.Controllers
{
    public class CheckoutController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public async Task<HttpResponseMessage> Index()
        {
            string UserId = User.Identity.GetUserId();
            var Order = db.Order.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == UserId && x.status == Status.CHECKOUT);
            return Request.CreateResponse<ResponseModel<Order>>(new ResponseModel<Order> { Status = HttpStatusCode.OK, Data = Order, Message = "record loaded successfully" });
        }

        
        public async Task<HttpResponseMessage> Address(long Id)
        {
            if(Id==0)
                return Request.CreateResponse<ResponseModel<string>>(new ResponseModel<string> { Status = HttpStatusCode.OK, Data = null, Message = "Address is not valid" });
            
            string UserId = User.Identity.GetUserId();
            var order=db.Order.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == UserId && x.status == Status.ADDRESS);
            order.status = Status.PAYMENT;
            order.AddressId = Id;
            await db.SaveChangesAsync();
            return Request.CreateResponse<ResponseModel<Order>>(new ResponseModel<Order> { Status = HttpStatusCode.OK, Data = order, Message = "record loaded successfully" });
        }

        
        public async Task<HttpResponseMessage> Payment()
        {
            var Payment = new Payment();
            Payment.PaymentId = "XXX";
            Payment.TransactionId = "XXX";
            db.Payment.Add(Payment);
            await db.SaveChangesAsync();
            string UserId = User.Identity.GetUserId();
            var Order = db.Order.OrderByDescending(x => x.Id).FirstOrDefault(x => x.UserId == UserId && x.status == Status.PAYMENT);
            Order.PaymentId = Payment.Id;
            await db.SaveChangesAsync();
            return Request.CreateResponse<ResponseModel<Order>>(new ResponseModel<Order> { Status = HttpStatusCode.OK, Data = Order, Message = "record loaded successfully" });
        }

        //public async Task<ActionResult> Order()
        //{
        //    //int UserId = Convert.ToInt32(User.Identity.GetUserId());
        //    //var Order = db.tblOrder.OrderByDescending(x => x.Id).FirstOrDefault(x => x.CustomerId == UserId && x.OrderStatus == 2 && x.PaymentId != null);
        //    //var user = db.UserMasters.FirstOrDefault(x => x.UserId == UserId);
        //    //await Utils.SendEmailAsync(user.EmailId, "Your order is successfully placed. Check your status using Order No: " + Order.OrderNumber, null, "Orylab: Order Placed");
        //    //return View(Order);
        //}

        //public ActionResult BackToAddress()
        //{
        //    int UserId = Convert.ToInt32(User.Identity.GetUserId());
        //    var Order = db.tblOrder.FirstOrDefault(x => x.CustomerId == UserId && x.OrderStatus == 1);
        //    Order.AddressId = null;
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

    }
}
