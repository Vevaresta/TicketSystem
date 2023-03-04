using Microsoft.AspNetCore.Identity;
using Ticketsystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Ticketsystem.Areas.Identity.Models;
using Ticketsystem.Areas.Identity.Enums;

namespace Ticketsystem.Areas.Identity.Services
{
    public class CheckRolePermissionsService
    {
        private readonly GetRolesService _getRolesService;
        private readonly UsersContext _identityContext;

        public CheckRolePermissionsService(GetRolesService getRolesService, UsersContext identityContext)
        {
            _getRolesService = getRolesService;
            _identityContext = identityContext;
        }

        public async Task<bool> HasPermissionAsync(TicketsystemUser loggedInUser, RolePermissions permission)
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
