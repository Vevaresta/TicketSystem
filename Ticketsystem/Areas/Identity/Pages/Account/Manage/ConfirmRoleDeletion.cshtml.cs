using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ConfirmRoleDeletionModel : PageModel
    {
        private readonly UserManager<TicketsystemUser> _userManager;
        private readonly RoleManager<EnhancedIdentityRole> _roleManager;

        public ConfirmRoleDeletionModel(UserManager<TicketsystemUser> userManager, RoleManager<EnhancedIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public EnhancedIdentityRole RoleToDelete { get; set; }

        public IActionResult OnGetAsync(string role)
        {
            RoleToDelete = new EnhancedIdentityRole(role);

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                    await _userManager.AddToRoleAsync(user, DefaultRoles.Fallback.ToString());
                    await _userManager.UpdateAsync(user);
                }
            }

            await _roleManager.DeleteAsync(role);
            await _roleManager.UpdateAsync(role);

            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage(nameof(ManageNavPages.ManageRoles));
        }
    }
}
