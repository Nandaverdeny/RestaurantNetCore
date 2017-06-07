using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class OrderItem
    {
        
        [Key]
        [Required]
        public int OrderItemID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int MenuID { get; set; }
        [Required]
        public int Qty { get; set; }
        public string Notes { get; set; }
        [Required]
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual Order Order { get; set; }
        public virtual Menu Menu { get; set; }
    }
}
