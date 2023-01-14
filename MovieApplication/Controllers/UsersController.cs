using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieApplication.Models;
using MovieApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;

        }

        public async Task<IActionResult> Index()
        {
            var Users = await _userManager.Users.Select(User => new UserViewModel
            {
                Id = User.Id,
                FirstName = User.FirstName,
                LastName = User.LastName,
                UserName = User.UserName,
                Email = User.Email,
                Roles = _userManager.GetRolesAsync(User).Result
            }
                ).ToListAsync();
            return View(Users);
        }

        public async Task<IActionResult> Add()
        {
            
            var roles = await _roleManager.Roles .Select(r=>new RoleViewModel { RoleId = r.Id,RoleName = r.Name}).ToListAsync();
            var ViewModel = new AddUserViewModel
            {
        
              Roles = roles
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if(model.Roles.Any(r=>r.IsSelected))
            {
                ModelState.AddModelError("Roles", "Please select at least one role");
                  return View(model);
            }

            if(await _userManager.FindByEmailAsync(model.Email) != null)
            {
                ModelState.AddModelError("Email", "Email is already exists");
                return View(model);
            }

            if (await _userManager.FindByNameAsync(model.UserName) != null)
            {
                ModelState.AddModelError("UserName", "UserName is already exists");
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName

            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if(!result.Succeeded)
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

        public async Task<IActionResult> Edit(string UserId)
        {
            var User = await _userManager.FindByIdAsync(UserId);
            if (User == null)
                return NotFound();
            var ViewModel = new ProfileFormViewModel
            {
               Id = UserId,
               FirstName = User.FirstName,
               LastName = User.LastName,
               UserName = User.UserName,
               Email = User.Email
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var User = await _userManager.FindByIdAsync(model.Id);

            if (User == null)
                return NotFound();

            var UserWithSomeEmail = await _userManager.FindByEmailAsync(model.Email);
            if(UserWithSomeEmail != null && UserWithSomeEmail .Id != model.Id)
            {
                ModelState.AddModelError("Email", "This email is already to another user");
                return View(model);
            }

            var UserWithSomeName = await _userManager.FindByNameAsync(model.UserName);
            if (UserWithSomeName != null && UserWithSomeName.Id != model.Id)
            {
                ModelState.AddModelError("UserName", "This email is already to another user");
                return View(model);
            }

            User.FirstName = model.FirstName;
            User.LastName = model.LastName;
            User.UserName = model.UserName;
            User.Email = model.Email;

            await _userManager.UpdateAsync(User);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ManageRoles(string UserId)
        {
            var User = await _userManager.FindByIdAsync(UserId);
            if (User == null)
                return NotFound();
            var Roles = await _roleManager.Roles.ToListAsync();
            var ViewModel = new UserRoleViewModel
            {
                UserId = User.Id,
                UserName = User.UserName,
                Roles = Roles.Select(role => new RoleViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(User ,role.Name).Result
                }).ToList()
            };
            return View(ViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRoleViewModel model)
        {
            var User = await _userManager.FindByIdAsync(model.UserId );
            if (User == null)
                return NotFound();
            var UserRoles = await _userManager.GetRolesAsync(User);

            foreach(var role in model.Roles)
            {
                if (UserRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(User, role.RoleName);

                if (!UserRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(User, role.RoleName);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
