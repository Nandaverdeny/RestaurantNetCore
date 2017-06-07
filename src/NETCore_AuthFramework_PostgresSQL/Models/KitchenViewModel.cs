using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class KitchenViewModel
    {
        public string Status { get; set; }
        public List<OrderItemViewModel> OrderItem { get; set; }
        public List<OrderViewModel> Order { get; set; }
    }
}
