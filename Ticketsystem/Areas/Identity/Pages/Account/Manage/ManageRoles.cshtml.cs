using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Models.Database;
using Ticketsystem.DbAccess;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ManageRolesModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;
        private IDbAccessFactory _serviceFactory;

        public ManageRolesModel(RoleManager<Role> roleManager, IDbAccessFactory serviceFactory)
        {
            _roleManager = roleManager;
            _serviceFactory = serviceFactory;
            Roles = new List<string>();
        }

        public List<string> Roles { get; set; }
        public List<string> RolesToDisplay { get; set; }

        public IActionResult OnGet()
        {
            Roles = (from role in _roleManager.Roles
                            select role.Name).ToList();

            RolesToDisplay = _serviceFactory.GetRolesToDisplayDbAccess().GetList();

            return Page();
        }

        public IActionResult OnPostEdit(string role)
        {
            return RedirectToPage(nameof(ManageNavPages.EditRolePermissions), new { Role = role });
        }

        public IActionResult OnPostAdd()
        {
            return RedirectToPage(nameof(ManageNavPages.AddRole));
        }

        public IActionResult OnPostDelete(string role)
        {
            return RedirectToPage(nameof(ManageNavPages.ConfirmRoleDeletion), new { Role = role });
        }
    }
}
