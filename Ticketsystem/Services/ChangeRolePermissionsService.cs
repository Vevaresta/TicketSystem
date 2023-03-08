using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class ChangeRolePermissionsService
    {
        private readonly TicketsystemContext _identityContext;
        private readonly RoleManager<Role> _roleManager;

        public ChangeRolePermissionsService(TicketsystemContext identityContext, RoleManager<Role> roleManager)
        {
            _identityContext = identityContext;
            _roleManager = roleManager;
        }

        public async Task AddPermissionToRole(Role role, RolePermissions permission)
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

        public async Task AddPermissionToRole(Role role, Permission permission)
        {
            // Check if the permission is already in the administrator's permission list
            if (!role.Permissions.Contains(permission))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Add(permission);
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task AddPermissionListToRole(Role role, List<RolePermissions> permissions)
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

        public async Task RemovePermissionFromRole(Role role, RolePermissions permission)
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

        public async Task RemovePermissionFromRole(Role role, Permission permission)
        {
            // Check if the permission is in the administrator's permission list
            if (role.Permissions.Contains(permission))
            {
                // Add the permission to the administrator's permission list
                role.Permissions.Remove(permission);
                await _roleManager.UpdateAsync(role);
            }
        }

        public async Task RemoveAllPermissionsFromRole(Role role)
        {
            foreach (var permission in role.Permissions)
            {
                role.Permissions.Remove(permission);
                await _roleManager.UpdateAsync(role);
            }
        }
    }
}
