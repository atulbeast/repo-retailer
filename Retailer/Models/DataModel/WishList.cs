using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class WishList
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("Product")]
        public long ProductId{ get; set; }
        public virtual Product Product { get; set; }
    }
}