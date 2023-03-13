using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Enums;
using Ticketsystem.Models;
using Ticketsystem.Services;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class EditRolePermissionsModel : PageModel
    {
        private readonly IServiceFactory _serviceFactory;

        public string RoleToEdit { get; set; }
        public List<string> Permissions;

        public EditRolePermissionsModel(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;

            Permissions = new List<string>();
        }

        public async Task<IActionResult> OnGetAsync(string role)
        {
            RoleToEdit = role;

            Role roleInDb = await _serviceFactory.GetRolesService().GetRoleByNameAsync(role);

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

            Role roleInDb = await _serviceFactory.GetRolesService().GetRoleByNameAsync(role);

            await _serviceFactory.GetRolePermissionsService().AddPermissionListToRole(roleInDb, permissionsEmumList);

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
