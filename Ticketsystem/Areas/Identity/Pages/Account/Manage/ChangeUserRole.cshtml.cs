using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ChangeUserRoleModel : PageModel
    {
        private readonly UserManager<TicketsystemUser> _userManager;

        public TicketsystemUser UserToEdit { get; set; }

        [BindProperty]
        public string Role { get; set; }

        public ChangeUserRoleModel(UserManager<TicketsystemUser> userManager)
        {
            _userManager = userManager;
            UserToEdit = new TicketsystemUser();
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            Role = userRoles.FirstOrDefault();
            UserToEdit = user;

            return Page();
        }

        public async Task<IActionResult> OnPostChangeUserRoleAsync(string userToEdit)
        {
            var user = await _userManager.FindByIdAsync(userToEdit);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            await _userManager.AddToRoleAsync(user, Role);

            return RedirectToPage("ManageUsers");
        }
    }
}
