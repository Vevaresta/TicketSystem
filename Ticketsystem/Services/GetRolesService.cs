using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class GetRolesService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public GetRolesService(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public List<Role> GetAllRolesFromDb()
        {
            return _roleManager.Roles.Include(r => r.Permissions).ToList();
        }

        public async Task<Role> GetRoleByNameAsync(string role)
        {
            var roleInDB = await _roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == role);

            if (roleInDB == null)
            {
                throw new Exception("Role not found in DB");
            }
            else
            {
                return roleInDB;
            }
        }

        public async Task<Role> GetUserRoleAsync(User user)
        {
            var userRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return await GetRoleByNameAsync(userRoleName);
        }
    }
}
