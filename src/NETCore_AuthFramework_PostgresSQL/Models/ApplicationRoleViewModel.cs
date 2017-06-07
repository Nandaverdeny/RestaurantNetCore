using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantNetCore.Models
{
    public class ApplicationRoleViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
