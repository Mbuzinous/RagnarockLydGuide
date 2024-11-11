using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RagnarockTourGuide.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            HttpContext.Session.Clear(); // Ryd sessionen
            return RedirectToPage("/Index"); // Omdiriger til startsiden
        }
    }
}
