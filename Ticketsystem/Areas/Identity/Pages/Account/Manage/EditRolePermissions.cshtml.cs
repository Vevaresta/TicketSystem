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
        private readonly GetRolesService _getRolesService;
        private readonly ChangeRolePermissionsService _changeRolePermissionsService;

        public string RoleToEdit { get; set; }
        public List<string> Permissions;

        public EditRolePermissionsModel(GetRolesService getRolesService, ChangeRolePermissionsService changeRolePermissionsService)
        {
            _getRolesService = getRolesService;
            _changeRolePermissionsService = changeRolePermissionsService;
            Permissions = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(string role)
        {
            RoleToEdit = role;

            EnhancedIdentityRole roleInDb = await _getRolesService.GetRoleByNameAsync(role);

            foreach (Permission permission in roleInDb.Permissions)
            {
                Permissions.Add(permission.Name);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync(string role, string[] permissions)
        {
            List<PermissionsEnum> permissionsEmumList = new List<PermissionsEnum>();

            foreach (string permission in permissions)
            {
                PermissionsEnum permissionEnum = Enum.Parse<PermissionsEnum>(permission);
                permissionsEmumList.Add(permissionEnum);
            }

            EnhancedIdentityRole roleInDb = await _getRolesService.GetRoleByNameAsync(role);

            await _changeRolePermissionsService.AddPermissionListToRole(roleInDb, permissionsEmumList);

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
