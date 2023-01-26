using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.ViewModels;

namespace UserManagementSystem.Controllers
{
	[Authorize(Roles ="Admin")]
	public class RolesController : Controller
	{
		private RoleManager<IdentityRole> _roleManager;

		public RolesController(RoleManager<IdentityRole> roleManager)
		{
			_roleManager = roleManager;
		}
		public async Task<IActionResult> Index()
		{
			return View(await _roleManager.Roles.ToListAsync());
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Add(RoleFormViewModel model)
		{
			if (!ModelState.IsValid)
				return View("Index", await _roleManager.Roles.ToListAsync());

			if(await _roleManager.RoleExistsAsync(model.Name))
			{
				ModelState.AddModelError("Name", "Role Is Exist");
				return View("Index", await _roleManager.Roles.ToListAsync());
			}
			await _roleManager.CreateAsync(new IdentityRole { Name = model.Name.Trim() });
			return RedirectToAction(nameof(Index));	
		}
	}
}
