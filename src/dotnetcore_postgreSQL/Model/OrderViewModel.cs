using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public string Name { get; set; }
        public int? TableID { get; set; }
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public DateTime? OrderDate { get; set; }
        public string TableName { get; set; }
        public string OrderServed { get; set; }
        public int Status { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
    }
}
