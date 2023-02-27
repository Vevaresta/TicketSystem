using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class EditUserRolesModel : PageModel
    {
        private UserManager<TicketsystemUser> _userManager;

        public TicketsystemUser UserToEdit { get; set; }
        public IEnumerable<string> UserRoles { get; set; }

        public EditUserRolesModel(UserManager<TicketsystemUser> userManager)
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

            UserRoles = await _userManager.GetRolesAsync(user);
            UserToEdit = user;

            return Page();
        }

        public async Task<IActionResult> OnPostEditUserRolesAsync(string userToEdit, string[] selectedRoles)
        {
            var user = await _userManager.FindByIdAsync(userToEdit);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(roles));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update user roles.");
                return Page();
            }

            result = await _userManager.RemoveFromRolesAsync(user, roles.Except(selectedRoles));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to update user roles.");
                return Page();
            }

            return RedirectToPage("ManageUsers");
        }
    }
}
