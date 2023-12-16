using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NEWS.Core.Constants;
using NEWS.Core.Dtos.User;
using NEWS.Core.Services.Interfaces;
using NEWS.Infrastructure.Data.Models;
using NEWS.Infrastructure.Data.Repo;

namespace NEWS.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<ApplicationUser> _userRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(IRepository<ApplicationUser> userRepo,
            UserManager<ApplicationUser> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            var users = await _userRepo.All().ToListAsync();

            var userDtos = new List<UserDto>();
            foreach (var user in users)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Name = $"{user.FirstName} {user.LastName}",
                    IsAdmin = userRoles.Any(r => r == "Admin")
                });
            }
            return userDtos;
        }

        public async Task AddRemoveRoleAsync(string userId, bool shouldAddAdminRole)
        {
            var user = await _userRepo.All()
                 .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new ArgumentNullException("Unknown user!");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (shouldAddAdminRole)
            {
                await _userManager.AddToRolesAsync(user, new List<string>() { RolesConstants.AdminRoleName });
            }
        }
    }
}
