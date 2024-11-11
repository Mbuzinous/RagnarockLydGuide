using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces;

namespace Kojg_Ragnarock_Guide.Pages.Exhibitions
{
    public class FloorNumber : PageModel
    {
        private IExhibitionCRUDRepoistory<Exhibition> _repo;

        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        public int FloorNr { get; set; }

        private IUserValidator _userValidator;
        public FloorNumber(IExhibitionCRUDRepoistory<Exhibition> repository, IUserValidator userValidator)
        {
            _repo = repository;
            _userValidator = userValidator;
        }

        public IActionResult OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Member || userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            FloorNr = id;
            if (id == 2 || id == 3)
            {
                Exhibitions = _repo.FilterListByNumber(_repo.GetAll(), id);
            }
            else
            {
                Exhibitions = _repo.GetAll();
            }

            return Page();
        }
    }
}
