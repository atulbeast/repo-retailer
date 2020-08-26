using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retailer.Models.ViewModel
{
    public class ProductModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public decimal Amount { get; set; }
        public long ShopId { get; set; }
        public long SubCategoryId { get; set; }
        public string[] ProductUrl { get; set; }
        public bool imageChanged { get; set; }
    }
}