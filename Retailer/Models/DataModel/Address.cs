using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Address
    {
        public long Id{get;set;}
        public string Name {get;set;}
        public string MobileNumber {get;set;}
        public string Address1  {get;set;}
        public string Address2 {get;set;}
        public long StateId {get;set;}
        public long CityId {get;set;}
        public string Country {get;set;}
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}