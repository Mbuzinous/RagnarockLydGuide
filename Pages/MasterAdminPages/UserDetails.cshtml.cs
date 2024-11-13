using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class UserDetailsModel : PageModel
    {
        public User UserDetails { get; set; }

        private BackendController<User> _backendController;

        public UserDetailsModel(BackendController<User> backendController)
        {
            _backendController = backendController;
        }

        public IActionResult OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            UserDetails = _backendController.ReadRepository.GetById(id);
            if (UserDetails == null)
            {
                return RedirectToPage("DisplayUsers");
            }
            return Page();
        }
    }
}
