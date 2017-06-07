using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class Bill
    {
        [Key]
        [Required]
        public int BillID { get; set; }
        [Required]
        public int OrderID { get; set; }
        public DateTime BillDate { get; set; }
        public string TotalPrice { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Order Order { get; set; }
    }
}
