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
        private readonly RolesService _rolesService;
        private readonly RolePermissionsService _rolePermissionsService;

        public string RoleToEdit { get; set; }
        public List<string> Permissions;

        public EditRolePermissionsModel(RolesService rolesService, RolePermissionsService rolePermissionsService)
        {
            _rolesService = rolesService;
            _rolePermissionsService = rolePermissionsService;
            Permissions = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(string role)
        {
            RoleToEdit = role;

            var roleInDb = await _rolesService.GetRoleByName(role);

            foreach (var permission in roleInDb.Permissions)
            {
                Permissions.Add(permission.Name);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync(string role, string[] permissions)
        {
            List<PermissionsEnum> permissionsEnum = new List<PermissionsEnum>();

            foreach (var permission in permissions)
            {
                PermissionsEnum permissionEnum = Enum.Parse<PermissionsEnum>(permission);
                permissionsEnum.Add(permissionEnum);
            }

            var roleInDb = await _rolesService.GetRoleByName(role);

            await _rolePermissionsService.AddPermissionListToRole(roleInDb, permissionsEnum);

            //await rolePermissionsService.AddPermissionToRole(roleInDb, PermissionsEnum.DeleteTickets);

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
