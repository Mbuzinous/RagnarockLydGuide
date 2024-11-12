using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{

    public class CreateExhibitionModel : PageModel
    {
        private IExhibitionCRUDRepoistory<Exhibition> _repo;

        [BindProperty]
        public Exhibition Exhibition { get; set; }

        public List<int> SuggestedExhibitionNumbers { get; private set; } = new List<int>();

        private IUserValidator _userValidator;
        public CreateExhibitionModel(IExhibitionCRUDRepoistory<Exhibition> repository, IUserValidator userValidator)
        {
            _repo = repository;
            _userValidator = userValidator;
        }

        public async Task<IActionResult> OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            var usedNumbers = await _repo.GetUsedNumbersAsync();
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
            await _repo.CreateAsync(Exhibition);

            ModelState.Clear();

            return RedirectToPage("AdminEpisodePage");
        }

    }
}
