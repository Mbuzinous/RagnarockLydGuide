using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace Kojg_Ragnarock_Guide.Pages.Admin
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

            // Tjek om brugeren har adgang baseret p� rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirig�r til forsiden, hvis brugeren ikke har den n�dvendige rolle
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
