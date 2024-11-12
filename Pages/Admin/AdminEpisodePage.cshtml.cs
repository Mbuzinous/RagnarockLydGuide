using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{
    public class AdminEpisodePage : PageModel
    {
        private IExhibitionCRUDRepoistory<Exhibition> _repo;

        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        [BindProperty(SupportsGet = true)]
        public int FilterCriteria { get; set; }

        private IUserValidator _userValidator;
        public AdminEpisodePage(IExhibitionCRUDRepoistory<Exhibition> repository, IUserValidator userValidator)
        {
            _repo = repository;
            _userValidator = userValidator;
        }
        public IActionResult OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            if (FilterCriteria == 2 || FilterCriteria == 3)
            {
                Exhibitions = _repo.FilterListByNumber(_repo.GetAll(), FilterCriteria);
            }
            else
            {
                Exhibitions = _repo.GetAll();
            }
            return Page();
        }

    }
}
