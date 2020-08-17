using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Shop
    {
        public long Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        [ForeignKey("Document")]
        public long DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }
}