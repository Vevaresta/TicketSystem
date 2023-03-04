using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Enums;
using Ticketsystem.Areas.Identity.Models;

namespace Ticketsystem.Areas.Identity.Services
{
    public class ChangeRolePermissionsService
    {
        private readonly UsersContext _identityContext;
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;

        public ChangeRolePermissionsService(UsersContext identityContext, RoleManager<EnhancedIdentityRole> roleManager)
        {
            _identityContext = identityContext;
            _roleManager = roleManager;
        }

        public async Task AddPermissionToRole(EnhancedIdentityRole role, RolePermissions permission)
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

        public async Task AddPermissionListToRole(EnhancedIdentityRole role, List<RolePermissions> permissions)
        {
            List<Permission> permissionObjects = new();

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

        public async Task RemovePermissionFromRole(EnhancedIdentityRole role, RolePermissions permission)
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

        public async Task RemoveAllPermissionsFromRole(EnhancedIdentityRole role)
        {
            foreach (var permission in role.Permissions)
            {
                role.Permissions.Remove(permission);
                await _roleManager.UpdateAsync(role);
            }
        }
    }
}
