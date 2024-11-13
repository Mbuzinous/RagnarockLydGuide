using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Admin
{
    public class EditExhibitionModel : PageModel
    {
        public Exhibition oldExhibition { get; set; } = new Exhibition();

        [BindProperty]
        public Exhibition toBeUpdatedExhibition { get; set; } = new Exhibition();
        public List<int> SuggestedExhibitionNumbers { get; private set; } = new List<int>();


        private BackendController<Exhibition> _backendController;

        public EditExhibitionModel(BackendController<Exhibition> backendController)
        {
            _backendController = backendController;
        }
        //Making some empty string messages i use in html razer pages, lower i define them
        public async Task<IActionResult> OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            oldExhibition = _backendController.ReadRepository.GetById(id);
            toBeUpdatedExhibition = oldExhibition;

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
            await _backendController.UpdateRepository.UpdateAsync(toBeUpdatedExhibition, oldExhibition);

            ModelState.Clear();

            return RedirectToPage("AdminEpisodePage");
        }
    }
}
