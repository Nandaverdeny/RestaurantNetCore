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
    public class ApplicationRoleController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public ApplicationRoleController(RoleManager<IdentityRole> roleMgr, UserManager<ApplicationUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;
        }

        public IActionResult Index()
        {
            return View(roleManager.Roles);
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
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }

            return View(name);
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            //Task<IList<ApplicationUser>> members = userManager.GetUsersInRoleAsync(role.Name);

            return View(new ApplicationRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationRoleViewModel modifyRole)
        {
            IdentityResult result;

            if (ModelState.IsValid)
            {
                IdentityRole role = await roleManager.FindByIdAsync(modifyRole.Id);
                role.Name = modifyRole.Name;
                result = await roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    AddErrors(result);                                                                                      
                }

            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            return View(modifyRole.Id);
        }

        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            //Task<IList<ApplicationUser>> members = userManager.GetUsersInRoleAsync(role.Name);

            return View(new ApplicationRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role != null)
            {
                //TODO: Add validation if there any user assigned to this role

                IdentityResult result = await roleManager.DeleteAsync(role);
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
                ModelState.AddModelError("", "No role found");
            }

            return View("Index", roleManager.Roles);
        }
    }
}