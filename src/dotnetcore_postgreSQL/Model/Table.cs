using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class Table
    {
        [Key]
        [Required]
        public int TableID { get; set; }
        [Required]
        public string TableName { get; set; }
        [Required]
        public string TableStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public ICollection<Track> Track { get; set; }
    }
}
