using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Services
{
    public class RolesService
    {
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;

        public RolesService(RoleManager<EnhancedIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public List<EnhancedIdentityRole> GetAllRolesFromDb()
        {
            return _roleManager.Roles.Include(r => r.Permissions).ToList();
        }

        public async Task<EnhancedIdentityRole> GetRoleByName(string role)
        {
            EnhancedIdentityRole roleInDB = await _roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == role);
            if (roleInDB == null)
            {
                throw new Exception("Role not found in DB");
            }
            else
            {
                return roleInDB;
            }
        }
    }
}
