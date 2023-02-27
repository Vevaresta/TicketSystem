using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Ticketsystem.Areas.Identity.Data;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage;

public class MangeUsersModel : PageModel
{
    private readonly UserManager<TicketsystemUser> _userManager;

    public class UserModel
    {
        public UserModel(TicketsystemUser user, IEnumerable<string> roles)
        {
            User = user;
            Roles = roles;
        }

        public TicketsystemUser User { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }

    public List<UserModel> Users { get; set; }

    public MangeUsersModel(UserManager<TicketsystemUser> userManager)
    {
        _userManager = userManager;

        Users = new List<UserModel>();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var vm = new UserModel(user, roles);
            Users.Add(vm);
        }

        return Page();
    }

    public async Task<IActionResult> OnPostEditAsync(string userId)
    {
        if (ModelState.IsValid && userId != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return RedirectToPage("EditUserRoles", new { UserId = userId });
            }
        }

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(string userId)
    {
        if (ModelState.IsValid && userId != null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                return RedirectToPage("ConfirmUserDeletion", new { UserId = userId });
            }
        }

        return RedirectToPage();
    }
}