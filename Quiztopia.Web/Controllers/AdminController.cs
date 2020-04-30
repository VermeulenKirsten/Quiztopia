using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quiztopia.Web.ViewModels;

namespace Quiztopia.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> userMgr;
        private readonly RoleManager<IdentityRole> roleMgr;

        public AdminController(UserManager<IdentityUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            this.userMgr = userMgr;
            this.roleMgr = roleMgr;
        }

        // GET: Admin
        public ActionResult IndexUsers()
        {
            var users = userMgr.Users;
            return View(users); //De view ontvangt een @model IEnumerable<IdentityUser>
        }
        public ActionResult IndexRoles()
        {
            var roles = roleMgr.Roles;
            return View(roles); //De view ontvangt een @model IEnumerable<IdentityRole>
        }


        /* ----------------- Role Management ----------------- */


        [HttpGet]
        public IActionResult CreateRole() => View();

        [HttpPost]
        public async Task<IActionResult> CreateRole(AddRoleVM addRoleVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(addRoleVM);
                }

                var role = new IdentityRole
                {
                    Name = addRoleVM.RoleName
                };

                IdentityResult result = await roleMgr.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("IndexRoles", roleMgr.Roles);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(addRoleVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Create is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Create action failed: " + ex.Message);
                return View(addRoleVM);
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditRole(string roleName)
        {

            // TODO: Add update logic here

            roleName = (roleName == "") ? null : roleName;

            var result = await roleMgr.FindByNameAsync(roleName);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            return View(new AddRoleVM(roleName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditRole(AddRoleVM addRoleVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(addRoleVM);
                }

                var role = new IdentityRole
                {
                    Name = addRoleVM.RoleName
                };

                IdentityResult result = await roleMgr.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("IndexRoles", roleMgr.Roles);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(addRoleVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Delete is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Delete action failed: " + ex.Message);
                return View(addRoleVM);
            }
        }

        [HttpGet]
        public async Task<ActionResult> DeleteRole(string roleName)
        {
            // TODO: Add update logic here

            roleName = (roleName == "") ? null : roleName;

            var result = await roleMgr.FindByNameAsync(roleName);

            if (result == null)
            {
                ModelState.AddModelError("", "Not Found");
            }

            return View(new AddRoleVM(roleName));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRole(AddRoleVM addRoleVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(addRoleVM);
                }

                var role = new IdentityRole
                {
                    Name = addRoleVM.RoleName
                };

                IdentityResult result = await roleMgr.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("IndexRoles", roleMgr.Roles);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(addRoleVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Delete is giving an error: " + ex.Message);
                ModelState.AddModelError("", "Delete action failed: " + ex.Message);
                return View(addRoleVM);
            }
        }


        /* ----------------- User Management ----------------- */

        [HttpGet]
        public async Task<IActionResult> AddRoleToUser(string userId)
        {
            IdentityUser user = new IdentityUser();
            //Op basis van het userId halt de _userManager de volledige user op

            if (!String.IsNullOrEmpty(userId))
            {
                user = await userMgr.FindByIdAsync(userId);
            }
            if (user == null)
                return RedirectToAction("IndexRoles", roleMgr.Roles);
            //Reeds toegekende rollen
            var assignRolesToUserVM = new RolesForUserVM()
            {
                AssignedRoles = await userMgr.GetRolesAsync(user),
                UnAssignedRoles = new List<string>(),
                User = user,
                UserId = userId
            };
            //Nog niet toegekende rollen
            foreach (var identityRole in roleMgr.Roles)
            {
                if (!await userMgr.IsInRoleAsync(user, identityRole.Name))
                {
                    assignRolesToUserVM.UnAssignedRoles.Add(identityRole.Name);
                }
            }
            return View(assignRolesToUserVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(RolesForUserVM rolesForUserVM)
        {
            var user = await userMgr.FindByIdAsync(rolesForUserVM.UserId);
            var role = await roleMgr.FindByNameAsync(rolesForUserVM.RoleId);
            var result = await userMgr.AddToRoleAsync(user, role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("IndexRoles", roleMgr.Roles);
            }
            return View(rolesForUserVM);
        }
    }
}