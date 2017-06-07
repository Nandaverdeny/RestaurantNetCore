using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class Track
    {
        [Key]
        [Required]
        public int TrackID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int TableID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public virtual Order Order{get;set;}
        public virtual Table Table { get; set; }
    }
}
