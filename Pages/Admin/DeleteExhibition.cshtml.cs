using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Admin
{
    public class DeleteExhibitionModel : PageModel
    {
        private BackendController<Exhibition> _backendController;

        public DeleteExhibitionModel(BackendController<Exhibition> backendController)
        {
            _backendController = backendController;
        }

        public IActionResult OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            //Validate ID
            if (_backendController.ReadRepository.GetById(id) != null)
            {
                //Delete exhibition
                DeleteParameter parameter = new DeleteParameter()
                {
                    Id = id,
                };
                _backendController.DeleteRepository.Delete(parameter);

                return RedirectToPage("AdminEpisodePage");
            }
            return RedirectToPage("AdminEpisodePage");
        }
    }
}
