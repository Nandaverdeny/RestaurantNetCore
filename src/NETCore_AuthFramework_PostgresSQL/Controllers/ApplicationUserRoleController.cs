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
using RestaurantNetCore.Data;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantNetCore.Controllers
{
    [Authorize]
    // to specify certain role
    //[Authorize(Roles = "SuperAdmin")]
    public class ApplicationUserRoleController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;
        private AuthFrameworkDbContext context;

        public ApplicationUserRoleController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr, AuthFrameworkDbContext _context)
        {
            roleManager = roleMgr;
            userManager = userMgr;
            context = _context;
        }

        public IActionResult Index()
        {
            var roles = roleManager.Roles;
            var users = userManager.Users;

            List<ApplicationUserRoleViewModel> listUserRole = new List<ApplicationUserRoleViewModel>();

            var userRoles = context.ApplicationUserRoles;

            foreach (var item in userRoles)
            {
                listUserRole.Add(new ApplicationUserRoleViewModel
                {
                    RoleId = item.RoleId,
                    RoleName = roles.Where(o => o.Id == item.RoleId).FirstOrDefault().Name,
                    UserId = item.UserId,
                    UserName = users.Where(o => o.Id == item.UserId).FirstOrDefault().UserName
                });
            }    

            return View(listUserRole);
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
        public async Task<IActionResult> Create([Required] ApplicationUserRoleViewModel obj)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(obj.UserName);
                IList<string> listRoles = await userManager.GetRolesAsync(user);

                if (!await userManager.IsInRoleAsync(user, obj.RoleName))
                {
                    IdentityResult result = await userManager.AddToRoleAsync(user, obj.RoleName);

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
                    ModelState.AddModelError("", "User already assigned to this role");
                }   
         
            }

            return View(obj);
        }

        //public async Task<IActionResult> Edit(string id)
        //{
        //    IdentityRole role = await roleManager.FindByIdAsync(id);

        //    //Task<IList<ApplicationUser>> members = userManager.GetUsersInRoleAsync(role.Name);

        //    return View(new ApplicationRoleViewModel
        //    {
        //        Id = role.Id,
        //        Name = role.Name
        //    });
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(ApplicationRoleViewModel modifyRole)
        //{
        //    IdentityResult result;

        //    if (ModelState.IsValid)
        //    {
        //        IdentityRole role = await roleManager.FindByIdAsync(modifyRole.Id);
        //        role.Name = modifyRole.Name;
        //        result = await roleManager.UpdateAsync(role);
        //        if (!result.Succeeded)
        //        {
        //            AddErrors(result);                                                                                      
        //        }

        //    }

        //    if (ModelState.IsValid)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View(modifyRole.Id);
        //}

        public async Task<IActionResult> Delete(string userId, string roleId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            IdentityRole role = await roleManager.FindByIdAsync(roleId);

            return View(new ApplicationUserRoleViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                UserId = user.Id, 
                UserName = user.UserName
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string userId, string roleId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId);
            IdentityRole role = await roleManager.FindByIdAsync(roleId);
          
            if (await userManager.IsInRoleAsync(user, role.Name))
            {
                IdentityResult result = await userManager.RemoveFromRoleAsync(user, role.Name);

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
                ModelState.AddModelError("", "User doesn't have this role");
            }

            return View("Index");
        }
    }
}