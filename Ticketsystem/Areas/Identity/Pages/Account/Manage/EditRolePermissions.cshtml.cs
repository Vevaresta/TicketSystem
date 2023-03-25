using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Enums;
using Ticketsystem.Models.Database;
using Ticketsystem.DbAccess;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class EditRolePermissionsModel : PageModel
    {
        private readonly IDbAccessFactory _serviceFactory;
        private readonly RolePermissionsDbAccess _rolePermissionsService;

        public string RoleToEdit { get; set; }
        public List<string> Permissions;

        public EditRolePermissionsModel(IDbAccessFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
            _rolePermissionsService = serviceFactory.RolePermissionsDbAccess;

            Permissions = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(string role)
        {
            RoleToEdit = role;

            Role roleInDb = await _serviceFactory.RolesDbAccess.GetRoleByName(role);

            foreach (Permission permission in roleInDb.Permissions)
            {
                Permissions.Add(permission.Name);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync(string role, string[] permissions)
        {
            List<RolePermissions> permissionsEmumList = new List<RolePermissions>();

            foreach (string permission in permissions)
            {
                RolePermissions permissionEnum = Enum.Parse<RolePermissions>(permission);
                permissionsEmumList.Add(permissionEnum);
            }

            Role roleInDb = await _serviceFactory.RolesDbAccess.GetRoleByName(role);

            await _rolePermissionsService.AddPermissionListToRole(roleInDb, permissionsEmumList);

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
