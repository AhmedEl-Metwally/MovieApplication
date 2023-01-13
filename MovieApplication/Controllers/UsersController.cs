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
    }
}
