using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Sockets;
using Ticketsystem.Data;
using Ticketsystem.Models.Database;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class ConfirmUserDeletionModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly TicketsystemContext _ticketsystemContext;

        public User UserToDelete;

        public ConfirmUserDeletionModel(UserManager<User> userManager, TicketsystemContext ticketsystemContext)
        {
            _userManager = userManager;
            _ticketsystemContext = ticketsystemContext;
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

                var ticketChanges = await _ticketsystemContext.TicketChanges.Where(d => d.UserId == userId).ToListAsync();
                foreach (var change in ticketChanges)
                {
                    change.UserId = "Fallback";
                }
                await _ticketsystemContext.SaveChangesAsync();
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
