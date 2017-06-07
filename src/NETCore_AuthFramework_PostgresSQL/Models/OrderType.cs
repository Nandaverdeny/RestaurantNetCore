using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantNetCore.Model
{
    public class OrderType
    {
        public int TypeID { get; set; }
        public string TypeName { get; set; }
        public List<OrderViewModel> Order { get; set; }
    }
}
