using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantNetCore.Controllers
{
    public class KitchenController : Controller
    {
        [Authorize(Roles = "Admin,Kitchen")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Kitchen")]
        public ActionResult Index2()
        {
            return View();
        }
    }
}