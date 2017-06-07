using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantNetCore.Controllers
{
    public class WaiterController : Controller
    {

        [Authorize(Roles = "Admin,Waiter,Cashier")]
        public ActionResult Index()
        {
            return View();
        }
    }
}