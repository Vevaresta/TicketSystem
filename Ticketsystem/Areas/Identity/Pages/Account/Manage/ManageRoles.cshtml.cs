using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ManageRolesModel : PageModel
    {
        private RoleManager<EnhancedIdentityRole> _roleManager;

        public ManageRolesModel(RoleManager<EnhancedIdentityRole> roleManager)
        {
            _roleManager = roleManager;
            Roles = new List<string>();
        }

        public List<string> Roles { get; set; }

        public IActionResult OnGet()
        {
            Roles = (from role in _roleManager.Roles
                            select role.Name).ToList();

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
