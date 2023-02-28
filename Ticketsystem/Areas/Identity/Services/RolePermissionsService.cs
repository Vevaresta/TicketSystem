using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Services
{
    public class RolePermissionsService
    {
        private IdentityContext _identityContext;
        private RoleManager<EnhancedIdentityRole> _roleManager;

        public RolePermissionsService(IdentityContext identityContext, RoleManager<EnhancedIdentityRole> roleManager)
        {
            _identityContext = identityContext;
            _roleManager = roleManager;
        }

        public async Task<EnhancedIdentityRole> GetRole(RolesEnum role)
        {
            EnhancedIdentityRole roleInDB = await _roleManager.Roles.Include(r => r.Permissions).FirstOrDefaultAsync(r => r.Name == role.ToString());
            if (roleInDB == null)
            {
                throw new Exception("Role not found in DB");
            }
            else
            {
                return roleInDB;
            }
        }

        public async Task AddPermissionToRole(EnhancedIdentityRole role, PermissionsEnum permission)
        {
            var permObject = await _identityContext.Permissions.FirstOrDefaultAsync(p => p.Name == permission.ToString());

            // Check if the permission is already in the administrator's permission list
            if (!role.Permissions.Contains(permObject))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Add(permObject);
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task RemovePermissionFromRole(EnhancedIdentityRole role, PermissionsEnum permission)
        {
            var permObject = await _identityContext.Permissions.FirstOrDefaultAsync(p => p.Name == permission.ToString());

            // Check if the permission is in the administrator's permission list
            if (role.Permissions.Contains(permObject))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Remove(permObject);
                await _roleManager.UpdateAsync(role);
            }
        }
    }
}
