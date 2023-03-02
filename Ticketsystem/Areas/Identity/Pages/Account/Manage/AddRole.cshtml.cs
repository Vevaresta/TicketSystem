using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Ticketsystem.Areas.Identity.Data;
using Ticketsystem.Areas.Identity.Models;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class AddRoleModel : PageModel
    {
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;

        public AddRoleModel(RoleManager<EnhancedIdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        [Required]
        [Display(Name = "Rolle")]
        public string RoleName { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            var roles = from role in _roleManager.Roles
                        select role.Name;

            if (!roles.Contains(RoleName) )
            {
                await _roleManager.CreateAsync(new EnhancedIdentityRole { Name =  RoleName, Permissions = new List<Permission>()});
            }

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
