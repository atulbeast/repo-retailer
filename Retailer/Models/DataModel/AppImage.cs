using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DoctorKind.Models.DbEntities
{
    public class ItemType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AppImage
    {
        public long Id { get; set; }
        public string Url { get; set; }
        [ForeignKey("Type")]
        public int ItemType { get; set; }
        public long ItemId { get; set; }
        public virtual ItemType Type { get; set; }

    }
}