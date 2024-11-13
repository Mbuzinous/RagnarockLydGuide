using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Pages.Admin
{
    public class EditExhibitionModel : PageModel
    {

        private IExhibitionCRUDRepoistory<Exhibition> _repo;

        public Exhibition oldExhibition { get; set; } = new Exhibition();

        [BindProperty]
        public Exhibition toBeUpdatedExhibition { get; set; } = new Exhibition();
        public List<int> SuggestedExhibitionNumbers { get; private set; } = new List<int>();


        private IUserValidator _userValidator;
        public EditExhibitionModel(IExhibitionCRUDRepoistory<Exhibition> repository, IUserValidator userValidator)
        {
            _repo = repository;
            _userValidator = userValidator;
        }
        //Making some empty string messages i use in html razer pages, lower i define them
        public async Task<IActionResult> OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret p� rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirig�r til forsiden, hvis brugeren ikke har den n�dvendige rolle
                return RedirectToPage("/Index");
            }

            oldExhibition = _repo.GetById(id);
            toBeUpdatedExhibition = oldExhibition;

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
            await _repo.UpdateAsync(toBeUpdatedExhibition, oldExhibition);

            ModelState.Clear();

            return RedirectToPage("AdminEpisodePage");
        }
    }
}
