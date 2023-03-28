using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;

namespace Ticketsystem.DbAccess
{
    public class RolePermissionsDbAccess
    {
        private readonly TicketsystemContext _ticketsystemContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public RolePermissionsDbAccess(TicketsystemContext ticketsystemContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _ticketsystemContext = ticketsystemContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> HasPermission(User loggedInUser, RolePermissions permission)
        {
            RolesDbAccess getRolesService = new(_userManager, _roleManager);

            var userRole = await getRolesService.GetUserRole(loggedInUser);

            var permissionInDb = (from p in _ticketsystemContext.Permissions
                                  where p.Name == permission.ToString()
                                  select p).FirstOrDefault();

            if (userRole.Permissions.Contains(permissionInDb))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task AddPermissionToRole(Role role, RolePermissions permission)
        {
            var permObject = await _ticketsystemContext.Permissions.FirstOrDefaultAsync(p => p.Name == permission.ToString());

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
                permissionObjects.Add(await _ticketsystemContext.Permissions.FirstOrDefaultAsync(p => p.Name == perm.ToString()));
            }


            List<Permission> tempPermissionList = new(role.Permissions);

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
            var permObject = await _ticketsystemContext.Permissions.FirstOrDefaultAsync(p => p.Name == permission.ToString());

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
            role.Permissions.Clear();
            await _roleManager.UpdateAsync(role);
        }
    }
}
