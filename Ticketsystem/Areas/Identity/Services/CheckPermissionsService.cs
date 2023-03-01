using Microsoft.AspNetCore.Identity;
using Ticketsystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Ticketsystem.Areas.Identity.Services
{
    public class CheckPermissionsService
    {
        private readonly RolesService _rolesService;
        private readonly IdentityContext _identityContext;

        public CheckPermissionsService(RolesService rolesService, IdentityContext identityContext)
        {
            _rolesService = rolesService;
            _identityContext = identityContext;
        }

        public async Task<bool> HasPermissionAsync(TicketsystemUser loggedInUser, PermissionsEnum permission)
        {
            var userRole = await _rolesService.GetUserRoleAsync(loggedInUser);

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
