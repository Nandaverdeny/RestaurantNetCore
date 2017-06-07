using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RestaurantNetCore.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUserViewModel
    {
        public ApplicationUserViewModel()
        { }

        /// <summary>
        /// Create a new Application User View Model object.
        /// </summary>
        /// <param name="_applicationUserViewModel"></param>
        public ApplicationUserViewModel(ApplicationUser _applicationUser)
        {
            Id = _applicationUser.Id;
            UserName = _applicationUser.UserName;
            Email = _applicationUser.Email;
            Password = _applicationUser.PasswordHash;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
