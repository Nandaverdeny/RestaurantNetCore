using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class Menu
    {
        [Key]
        [Required]
        public int MenuID { get; set; }
        [Required]
        public string MenuName { get; set; }
        [Required]
        public string MenuPrice { get; set; }
        public string MenuDescription { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public int StatusID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsDeleted { get; set; }
        public virtual Category Category { get; set; }
        public virtual Status Status { get; set; } 
        public ICollection<OrderItem> OrderItem { get; set; }
        

    }
}
