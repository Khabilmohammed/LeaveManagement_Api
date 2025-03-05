using LeaveManagement.Models.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Data.Repository.AuthRepo
{
    public interface IAuthRepository
    {
        Task<ApplicationUser> FindUserByNameAsync(string username);
        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password);
        Task<bool> RoleExistsAsync(string role);
        Task CreateRoleAsync(string role);
        Task AddUserToRoleAsync(ApplicationUser user, string role);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    }
}
