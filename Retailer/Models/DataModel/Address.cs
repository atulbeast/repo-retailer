using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Retailer.Models.DbEntities
{
    public class Address
    {
        public long  Id{get;set;}
        public string Name {get;set;}
        public string MobileNumber {get;set;}
        public string Address1  {get;set;}
        public string Address2 {get;set;}
        public long StateId {get;set;}
        public long CityId {get;set;}
        public string Country {get;set;}
        
    }
}