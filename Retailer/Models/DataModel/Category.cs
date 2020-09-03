using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }
    }
}