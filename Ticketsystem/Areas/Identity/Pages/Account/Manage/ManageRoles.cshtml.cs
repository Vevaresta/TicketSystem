using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Models;
using Ticketsystem.Services;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ManageRolesModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly GetRolesToDisplayService _getRolesToDisplayService;

        public ManageRolesModel(RoleManager<Role> roleManager, GetRolesToDisplayService getRolesToDisplayService)
        {
            _roleManager = roleManager;
            _getRolesToDisplayService = getRolesToDisplayService;
            Roles = new List<string>();
        }

        public List<string> Roles { get; set; }
        public List<string> RolesToDisplay { get; set; }

        public IActionResult OnGet()
        {
            Roles = (from role in _roleManager.Roles
                            select role.Name).ToList();

            RolesToDisplay = _getRolesToDisplayService.GetList();

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
