using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
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

        public async Task AddPermissionToRole(EnhancedIdentityRole role, Permission permission)
        {
            // Check if the permission is already in the administrator's permission list
            if (!role.Permissions.Contains(permission))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Add(permission);
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task AddPermissionListToRole(EnhancedIdentityRole role, List<PermissionsEnum> permissions)
        {
            List<Permission> permissionObjects = new List<Permission>();

            foreach (var perm in permissions)
            {
                permissionObjects.Add(await _identityContext.Permissions.FirstOrDefaultAsync(p => p.Name == perm.ToString()));
            }


            List<Permission> tempPermissionList = new List<Permission>(role.Permissions);

            foreach (var permissionInDb in tempPermissionList)
            {
                if (!permissionObjects.Contains(permissionInDb))
                {
                    role.Permissions.Remove(permissionInDb);
                }
            }

            foreach (var permission in permissionObjects)
            {
                if (!role.Permissions.Contains(permission))
                {
                    await AddPermissionToRole(role, permission);
                }
            }

            await _roleManager.UpdateAsync(role);
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

        public async Task RemovePermissionFromRole(EnhancedIdentityRole role, Permission permission)
        {
            // Check if the permission is in the administrator's permission list
            if (role.Permissions.Contains(permission))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Remove(permission);
                await _roleManager.UpdateAsync(role);
            }
        }
    }
}
