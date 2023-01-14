using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApplication.Controllers.API
{

    [Route("api/[Controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

        }

        [HttpDelete]
        public async Task <IActionResult> DeleteUser(string userId)
        {
            var User = await _userManager.FindByIdAsync(userId);
            if (User == null)
                return NotFound();

            var resuit = await _userManager.DeleteAsync(User);
            if (resuit.Succeeded)
                throw new Exception();
            return Ok();
                
        }
    }
}
