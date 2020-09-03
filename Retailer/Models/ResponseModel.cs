using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Retailer.Models
{
    public class ResponseModel<T> 
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public T Data{ get; set; }
    }
}