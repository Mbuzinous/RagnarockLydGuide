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
    public class DisplayUsersModel : PageModel
    {
        public List<User> Users { get; set; } = new List<User>();

        private BackendController<User> _backendController;

        public DisplayUsersModel(BackendController<User> backendController)
        {
            _backendController = backendController;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            Users = await _backendController.ReadRepository.GetAllAsync();
            return Page();
        }
    }
}
