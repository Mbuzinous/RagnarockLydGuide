using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Admin
{
    public class AdminEpisodePage : PageModel
    {
        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        [BindProperty(SupportsGet = true)]
        public int FilterCriteria { get; set; }

        private BackendController<Exhibition> _backendController;

        public AdminEpisodePage(BackendController<Exhibition> backendController)
        {
            _backendController = backendController;
        }
        public IActionResult OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            if (FilterCriteria == 2 || FilterCriteria == 3)
            {
                Exhibitions = _backendController.ReadRepository.FilterListByNumber(_backendController.ReadRepository.GetAll(), FilterCriteria);
            }
            else
            {
                Exhibitions = _backendController.ReadRepository.GetAll();
            }
            return Page();
        }

    }
}
