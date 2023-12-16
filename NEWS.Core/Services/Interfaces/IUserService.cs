using NEWS.Core.Dtos.User;
using NEWS.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEWS.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsers();
        Task AddRemoveRoleAsync(string userId, bool shouldAddAdminRole);
    }
}
