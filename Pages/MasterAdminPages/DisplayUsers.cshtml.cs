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

        public IActionResult OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret p� rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirig�r til forsiden, hvis brugeren ikke har den n�dvendige rolle
                return RedirectToPage("/Index");
            }
            Users = _backendController.ReadRepository.GetAll();
            return Page();
        }
    }
}
