using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using RestaurantNetCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantNetCore.Controllers
{
    [Authorize]
    // to specify certain role
    //[Authorize(Roles = "SuperAdmin")]
    public class ApplicationUserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IUserValidator<ApplicationUser> userValidator;
        private IPasswordValidator<ApplicationUser> passwordValidator;
        private IPasswordHasher<ApplicationUser> passwordHasher;

        private ApplicationUser testUser = new ApplicationUser
        {
            UserName = "TestTestForPassword",
            Email = "testForPassword@test.test"
        };

        public ApplicationUserController(
            UserManager<ApplicationUser> userMgr,
            IUserValidator<ApplicationUser> userValid, 
            IPasswordValidator<ApplicationUser> passValid,
            IPasswordHasher<ApplicationUser> passHasher)
        {
            userManager = userMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passHasher;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Required] ApplicationUserViewModel obj)
        {
          
            if (ModelState.IsValid)
            {
                IdentityResult result = await userManager.CreateAsync(new ApplicationUser(obj), obj.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                return View(new ApplicationUserViewModel(user));
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUserViewModel obj)
        {
            ApplicationUser user = await userManager.FindByIdAsync(obj.Id);

            if (user != null)
            {
                bool validPassword = false;

                // Validate UserName and Email  
                user.UserName = obj.UserName; // UserName won't be changed in the database until UpdateAsync is executed successfully 
                user.Email = obj.Email;
                IdentityResult validUseResult = await userValidator.ValidateAsync(userManager, user);
                if (!validUseResult.Succeeded)
                {
                    AddErrors(validUseResult);
                }

                // Validate password 
                if (!string.IsNullOrEmpty(obj.Password) && !string.IsNullOrWhiteSpace(obj.Password))
                {

                    // Step 1: using built in validations 
                    IdentityResult passwordResult = await userManager.CreateAsync(testUser, obj.Password);
                    if (passwordResult.Succeeded)
                    {
                        await userManager.DeleteAsync(testUser);
                    }
                    else
                    {
                        AddErrors(passwordResult);
                    }

                    /* Step 2: Because of DI, IPasswordValidator<ApplicationUser> is injected into the custom password validator.  
                       So the built in password validation stop working here */
                    IdentityResult validPasswordResult = await passwordValidator.ValidateAsync(userManager, user, obj.Password);
                    if (validPasswordResult.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, obj.Password);
                    }
                    else
                    {
                        AddErrors(validPasswordResult);
                    }

                    validPassword = passwordResult.Succeeded && validPasswordResult.Succeeded ? true : false;

                }
                else
                {
                    validPassword = true;
                }

                // Update user info 
                if (validUseResult.Succeeded && validPassword)
                {
                    // UpdateAsync validates user info such as UserName and Email except password since it's been hashed  
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "ApplicationUser");
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            return View(new ApplicationUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }

            return View("Index", userManager.Users);
        }

    }
}