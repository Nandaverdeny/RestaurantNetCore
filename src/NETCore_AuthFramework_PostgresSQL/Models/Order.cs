using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class Order
    {
        [Key]
        [Required]
        public int OrderID { get; set; }
        public string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public int TypeID { get; set; }
        public bool? Finish { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool? IsDeleted { get; set; }
        public ICollection<Bill> Bill { get; set; }
        public ICollection<OrderItem> OrderItem { get; set; }
        public ICollection<Track> Track { get; set; }
        public virtual Type Type { get; set; }
        
    }
}
