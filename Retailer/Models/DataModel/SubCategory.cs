using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class SubCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        [ForeignKey("Category")]
        public long CategoryId { get;set; }
        public virtual Category Category { get; set; }
    }
}