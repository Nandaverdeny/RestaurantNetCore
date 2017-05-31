using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantNetCore.Controllers
{
    public class KitchenController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}