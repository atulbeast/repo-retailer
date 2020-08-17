using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class LineItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Product")]
        public long ProductId { get; set; }
        [ForeignKey("Order")]
        public long OrderId { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
        
    }
}