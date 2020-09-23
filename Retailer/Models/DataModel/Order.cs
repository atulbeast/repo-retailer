using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public enum Status {
       CHECKOUT=1,ADDRESS=2,PAYMENT=3,COMPLETE=4
    }
    public class Order
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public decimal Amount { get; set; }
        public string OrderNumber { get; set; }
        public Status status { get; set; }
        [ForeignKey("Address")]
        public long? AddressId { get; set; }
        [ForeignKey("Payment")]
        public long? PaymentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Address Address { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual ICollection<LineItem> LineItems { get; set; }
        

    }
}