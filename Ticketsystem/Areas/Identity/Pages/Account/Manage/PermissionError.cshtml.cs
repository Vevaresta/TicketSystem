using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Ticketsystem.Areas.Identity.Pages.Account.Manage
{
    public class PermissionErrorModel : PageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToAction("PermissionError", "Home");
        }
    }
}
