using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class SubCategory
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long CategoryId { get;set; }
    }
}