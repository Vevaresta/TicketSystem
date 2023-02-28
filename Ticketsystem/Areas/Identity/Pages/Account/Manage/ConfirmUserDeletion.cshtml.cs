using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ConfirmUserDeletionModel : PageModel
    {
        private readonly UserManager<TicketsystemUser> _userManager;

        public TicketsystemUser UserToDelete;

        public ConfirmUserDeletionModel(UserManager<TicketsystemUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (UserToDelete == null)
            {
                UserToDelete = await _userManager.FindByIdAsync(userId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync(string userId)
        {
            TicketsystemUser userToDelete = await _userManager.FindByIdAsync(userId);
            
            if (userToDelete != null)
            {
                await _userManager.DeleteAsync(userToDelete);
            }

            return RedirectToPage(nameof(ManageNavPages.ManageUsers));
        }

        public IActionResult OnPostCancel()
        {
            return RedirectToPage(nameof(ManageNavPages.ManageUsers));
        }
    }
}
