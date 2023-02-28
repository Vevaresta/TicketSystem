using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Services;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class EditRolePermissionsModel : PageModel
    {
        private IdentityContext _identityContext;
        private RoleManager<EnhancedIdentityRole> _roleManager;

        public string RoleToEdit { get; set; }
        public List<string> Permissions;

        public EditRolePermissionsModel(IdentityContext identityContext, RoleManager<EnhancedIdentityRole> roleManager)
        {
            _identityContext = identityContext;
            _roleManager = roleManager;
            Permissions = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(string role)
        {
            RoleToEdit = role;

            RolePermissionsService rolePermissionsService = new RolePermissionsService(_identityContext, _roleManager);
            RolesEnum rolesEnum = Enum.Parse<RolesEnum>(role);

            var roleInDb = await rolePermissionsService.GetRole(rolesEnum);

            foreach (var permission in roleInDb.Permissions)
            {
                Permissions.Add(permission.Name);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync(string role, string[] permissions)
        {
            RolePermissionsService rolePermissionsService = new RolePermissionsService(_identityContext, _roleManager);
            List<PermissionsEnum> permissionsEnum = new List<PermissionsEnum>();

            foreach (var permission in permissions)
            {
                PermissionsEnum permissionEnum = Enum.Parse<PermissionsEnum>(permission);
                permissionsEnum.Add(permissionEnum);
            }

            var roleInDb = await rolePermissionsService.GetRole(Enum.Parse<RolesEnum>(role));

            await rolePermissionsService.AddPermissionListToRole(roleInDb, permissionsEnum);

            //await rolePermissionsService.AddPermissionToRole(roleInDb, PermissionsEnum.DeleteTickets);

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
