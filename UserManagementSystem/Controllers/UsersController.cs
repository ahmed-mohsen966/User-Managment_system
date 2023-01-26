using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UserManagementSystem.Models;
using UserManagementSystem.Models.viewModel;

namespace UserManagementSystem.Controllers
{
	[Authorize(Roles = "Admin")]
	public class UsersController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index()
		{
			var users = await _userManager.Users.Select(user => new UserViewModel
			{
				Id = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				Fname = user.Fname,
				Lname = user.Lname,
				Roles = _userManager.GetRolesAsync(user).GetAwaiter().GetResult() // GetAwaiter returns an Awaiter instance which without it may cause multi-thread exception ,,,
																				  //Roles = _userManager.GetRolesAsync(user).Result
			}).ToListAsync();
			// use .Result to make the function awaitable instead of using "await" 
			// which is unavailable here 
			return View(users);
		}

		public async Task<IActionResult> Add()
		{
			var roles = await _roleManager.Roles.Select(r => new RoleViewModel { RoleId = r.Id , RoleName = r.Name}).ToListAsync();
			var viewModel = new AddUserViewModel
			{
				Roles = roles
			};
			return View(viewModel);
		}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
			if (!ModelState.IsValid)
				return View(model);
			if(!model.Roles.Any(r => r.IsSelected))
			{
				ModelState.AddModelError("roles", "Please Select At Least One Role!");
				return View(model);
			}
			if(await _userManager.FindByEmailAsync(model.Email) != null)
			{
				ModelState.AddModelError("Email", "Email Is Allready Exist , please Sign In With Another Email.");
				return View(model);
			}
			if(await _userManager.FindByNameAsync(model.UserName) != null)
			{
				ModelState.AddModelError("UserName", "This User Name Is taken");
				return View(model);
			}

			var user = new ApplicationUser
			{
				UserName = model.UserName,
				Email = model.Email,
				Fname = model.Fname,
				Lname = model.Lname
			};
			var result = await _userManager.CreateAsync(user,model.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("Roles", error.Description);
				}
				return View(model);
			}

			await _userManager.AddToRolesAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.RoleName));

            return RedirectToAction(nameof(Index));
        }

		public async Task<IActionResult> Edit(string userid)
		{
			var user = await _userManager.FindByIdAsync(userid);
			if (user == null)
				return NotFound();

			var viewModel = new ProfileFormViewModel
			{
				Id=user.Id,
				Fname=user.Fname,
				Lname = user.Lname,
				Email=user.Email
			};

			return View(viewModel);
		}


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null) // check if userid is a user in DataBase 
                return NotFound();

			var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
			if(userWithSameEmail !=null && userWithSameEmail.Id != model.Id)
			{
				ModelState.AddModelError("Email", "This Email Is Allready Assigned To Another User");
				return View(model);
			}

            var userWithSameUserName = await _userManager.FindByNameAsync(model.Email);
            if (userWithSameUserName != null && userWithSameUserName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This UserName Is Allready Assigned To Another User");
                return View(model);
            }

			user.Fname = model.Fname;
			user.Lname = model.Lname;
			user.Email = model.Email;
			user.UserName = model.UserName;
			await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManageRoles(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
				return NotFound();

			var roles =await _roleManager.Roles.ToListAsync();

			var viewModel = new UserRoleViewModel
			{
				UserID = user.Id,
				UserName =user.UserName,
				Roles = roles.Select(role => new RoleViewModel
				{
					RoleId = role.Id,
					RoleName =role.Name,
					IsSelected = _userManager.IsInRoleAsync(user,role.Name).Result
				}).ToList()
			};
			return View(viewModel);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ManageRoles(UserRoleViewModel model)
		{
			var user = await _userManager.FindByIdAsync(model.UserID); 

			if (user == null) // check if userid is a user in DataBase 
				return NotFound();

			var userRoles = await _userManager.GetRolesAsync(user); // get all roles in user to compara with roles in "Model" DB

			foreach (var role in model.Roles)
			{
				if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected) // any role in user == role name and not selected => remove
					await _userManager.RemoveFromRoleAsync(user, role.RoleName);

				if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected) // any role not in user == role name and selected => add to user roles 
					await _userManager.AddToRoleAsync(user, role.RoleName);
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
