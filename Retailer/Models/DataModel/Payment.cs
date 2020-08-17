using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Retailer.Models.DataModel
{
    public class Payment
    {
        public long Id { get; set; }
        public string TransactionId { get; set; }
        public string PaymentId { get; set; }
        [ForeignKey("Appointment")]
        public long AppointmentId { get; set; }
        
    }
}