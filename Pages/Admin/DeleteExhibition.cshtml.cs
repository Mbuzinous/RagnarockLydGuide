using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Pages.Admin
{
    public class DeleteExhibitionModel : PageModel
    {
        private IExhibitionCRUDRepoistory<Exhibition> _repo;

        private IUserValidator _userValidator;
        public DeleteExhibitionModel(IExhibitionCRUDRepoistory<Exhibition> repository, IUserValidator userValidator)
        {
            _repo = repository;
            _userValidator = userValidator;
        }
        public IActionResult OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            //Validate ID
            if (_repo.GetById(id) != null)
            {
                //Delete Exhibition
                _repo.Delete(id);

                return RedirectToPage("AdminEpisodePage");
            }
            return RedirectToPage("AdminEpisodePage");
        }
    }
}
