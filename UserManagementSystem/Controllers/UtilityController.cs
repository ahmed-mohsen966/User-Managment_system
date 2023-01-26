using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserManagementSystem.Models;


namespace UserManagementSystem.Controllers
{
    public class UtilityController : Controller
    {
        private UserManager<ApplicationUser> usermanager;
        private RoleManager<IdentityRole> roleManager;

        public UtilityController(UserManager<ApplicationUser> _usermanager , RoleManager<IdentityRole> _roleManager)
        {
            usermanager = _usermanager;
            roleManager = _roleManager;
        }

        [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            var users = usermanager.Users.ToList();
            ViewBag.users = new SelectList(users, "Id", "UserName");
            return View();
        }
        public async Task<IActionResult> Showroles(string userid) // show roles in and notIn
        {
			var users = usermanager.Users.ToList();
			ViewBag.users = new SelectList(users, "Id", "UserName",userid);

			var user = await usermanager.FindByIdAsync(userid);
            var rolesIn = await usermanager.GetRolesAsync(user);
            var allRoles = roleManager.Roles.Select(r => r.Name).ToList();
            var rolesNotIn = allRoles.Except(rolesIn);
            ViewBag.userid= userid;

            ViewBag.rolesIn = new SelectList(rolesIn);

            ViewBag.rolesNotIn = new SelectList(rolesNotIn);
            return View();
        }
        public async Task<IActionResult> ChangeRoles(string userid,string[] rolesToRemoveFrom, string[] rolesToAdd)
        {
           var user =await usermanager.FindByIdAsync(userid);

           await usermanager.RemoveFromRolesAsync(user, rolesToRemoveFrom);
           await usermanager.AddToRolesAsync(user, rolesToAdd);

           return RedirectToAction("index");
        }
	}
}
