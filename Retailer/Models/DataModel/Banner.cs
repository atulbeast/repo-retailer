using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Banner
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}