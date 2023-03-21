using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ConfirmUserDeletionModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public User UserToDelete;

        public ConfirmUserDeletionModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }

            if (UserToDelete == null)
            {
                UserToDelete = await _userManager.FindByIdAsync(userId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync(string userId)
        {
            User userToDelete = await _userManager.FindByIdAsync(userId);
            
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
