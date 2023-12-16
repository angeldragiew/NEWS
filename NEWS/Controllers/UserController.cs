using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NEWS.Core.Constants;
using NEWS.Core.Services.Interfaces;
using NEWS.Infrastructure.Data.Models;
using System.Data;

namespace NEWS.Controllers
{
    [Authorize(Roles = RolesConstants.AdminRoleName)]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _userService.GetUsers();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> AddRemoveRole(string userId, bool shouldAddAdminRole = false)
        {
            try
            {
                await _userService.AddRemoveRoleAsync(userId, shouldAddAdminRole);
            }
            catch (Exception ex)
            {
                TempData[MessageConstant.ErrorMessage] = ex.Message;
            }

            return RedirectToAction("ManageUsers", "User");
        }
    }
}
