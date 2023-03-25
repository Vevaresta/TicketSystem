using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class RolesDbAccess
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RolesDbAccess(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<Role> GetAllRolesFromDb()
        {
            return _roleManager.Roles.Include(r => r.Permissions).ToList();
        }

        public async Task<Role> GetRoleByName(string role)
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

        public async Task<Role> GetUserRole(User user)
        {
            var userRoleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return await GetRoleByName(userRoleName);
        }
    }
}
