using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantNetCore.Models
{
    [Table("AspNetUserRoles")]
    public class ApplicationUserRole
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

    }
}
