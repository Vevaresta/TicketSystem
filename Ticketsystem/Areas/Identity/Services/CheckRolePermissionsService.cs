using Microsoft.AspNetCore.Identity;
using Ticketsystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ticketsystem.Areas.Identity.Services
{
    public class CheckRolePermissionsService
    {
        private readonly GetRolesService _getRolesService;
        private readonly IdentityContext _identityContext;

        public CheckRolePermissionsService(GetRolesService getRolesService, IdentityContext identityContext)
        {
            _getRolesService = getRolesService;
            _identityContext = identityContext;
        }

        public async Task<bool> HasPermissionAsync(TicketsystemUser loggedInUser, PermissionsEnum permission)
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
