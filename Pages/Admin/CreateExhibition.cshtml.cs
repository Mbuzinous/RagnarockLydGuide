using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Admin
{

    public class CreateExhibitionModel : PageModel
    {
        [BindProperty]
        public Exhibition Exhibition { get; set; }

        public List<int> SuggestedExhibitionNumbers { get; private set; } = new List<int>();

        private BackendController<Exhibition> _backendController;

        public CreateExhibitionModel(BackendController<Exhibition> backendController)
        {
            _backendController = backendController;
        }
        public async Task<IActionResult> OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            var usedNumbers = await _backendController.ReadRepository.GetUsedNumbersAsync();
            SuggestedExhibitionNumbers = Enumerable.Range(1, 100).Except(usedNumbers).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Save Exhibition
            await _backendController.CreateRepository.CreateAsync(Exhibition);

            ModelState.Clear();

            return RedirectToPage("AdminEpisodePage");
        }

    }
}
