using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RestaurantNetCore.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {}

        /// <summary>
        /// Create a new Application User object.
        /// </summary>
        /// <param name="_applicationUserViewModel"></param>
        public ApplicationUser (ApplicationUserViewModel _applicationUserViewModel)
        {
            UserName = _applicationUserViewModel.UserName;
            Email = _applicationUserViewModel.Email;
        }
    }
}
