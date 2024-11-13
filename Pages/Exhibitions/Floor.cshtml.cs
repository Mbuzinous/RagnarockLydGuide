    using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Exhibitions
{
    public class FloorNumber : PageModel
    {
        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        public int FloorNr { get; set; }

        private BackendController<Exhibition> _backendController;

        public FloorNumber(BackendController<Exhibition> backendController)
        {
            _backendController = backendController;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Member || userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            FloorNr = id;
            if (id == 2 || id == 3)
            {
                Exhibitions = _backendController.ReadRepository.FilterListByNumber(await _backendController.ReadRepository.GetAllAsync(), id);
            }
            else
            {
                Exhibitions = await _backendController.ReadRepository.GetAllAsync();
            }

            return Page();
        }
    }
}
