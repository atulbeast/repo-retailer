using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Profile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string ContactNo { get; set; }
        public string Address { get; set; }
        public string PinCode { get; set; }
        public string Email { get; set; }
        public string DOB { get; set; }
        //public int Age { get; set; }
        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }


    }
}