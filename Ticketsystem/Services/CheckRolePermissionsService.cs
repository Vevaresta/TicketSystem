using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Ticketsystem.Data;
using Ticketsystem.Enums;
using Ticketsystem.Models;

namespace Ticketsystem.Services
{
    public class CheckRolePermissionsService
    {
        private readonly GetRolesService _getRolesService;
        private readonly TicketsystemContext _identityContext;

        public CheckRolePermissionsService(GetRolesService getRolesService, TicketsystemContext identityContext)
        {
            _getRolesService = getRolesService;
            _identityContext = identityContext;
        }

        public async Task<bool> HasPermissionAsync(User loggedInUser, RolePermissions permission)
        {
            var userRole = await _getRolesService.GetUserRoleAsync(loggedInUser);

            var permissionInDb = (from p in _identityContext.Permissions
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
    }
}
