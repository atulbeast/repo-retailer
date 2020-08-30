using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public decimal Total { get; set; }
        public decimal Amount { get; set; }
        [ForeignKey("Shop")]
        public long ShopId { get; set; }
        [ForeignKey("SubCategory")]
        public long SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual Shop Shop{get;set;}
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        
    }

    public class ProductImage
    {
        public long Id { get; set; }
        public string ImageUrl { get; set; }
        //[ForeignKey("Product")]
        public long ProductId { get; set; }
        //public virtual Product Product { get; set; }
        public bool IsDeleted { get; set; }
    }

}