using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url{get;set;}
        
    }
}