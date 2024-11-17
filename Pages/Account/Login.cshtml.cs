using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models.Enums;

namespace RagnarockTourGuide.Pages.Account
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        private readonly IUserValidator _userValidator;

        public LoginModel(IUserValidator userValidator)
        {
            _userValidator = userValidator;
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email))
            {
                ErrorMessage = "E-mail skal udfyldes.";
                return Page();
            }
            if (string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Kodeord skal udfyldes.";
                return Page();
            }

            // Valider brugeroplysninger mod databasen
            Role? userRole = _userValidator.ValidateUser(Email, Password);
            if (userRole != null)
            {
                // Gem brugerens rolle i sessionen
                HttpContext.Session.SetString("UserRole", userRole.ToString());
                return RedirectToPage("/Index"); // Omdiriger til startsiden
            }
            else
            {
                ErrorMessage = "Ugyldig e-mail eller kodeord.";
                return Page();
            }
        }
    }
}
