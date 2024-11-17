using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Models.Enums;

namespace RagnarockTourGuide.Pages.Admin
{
    public class DeleteExhibitionModel : PageModel
    {
        private BackendController<Exhibition> _backendController;

        public DeleteExhibitionModel(BackendController<Exhibition> backendController)
        {
            _backendController = backendController;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret p� rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirig�r til forsiden, hvis brugeren ikke har den n�dvendige rolle
                return RedirectToPage("/Index");
            }
            //Validate ID
            if (_backendController.ReadRepository.GetByIdAsync(id) != null)
            {
                //Delete exhibition
                DeleteParameter parameter = new DeleteParameter()
                {
                    Id = id,
                };
                await _backendController.DeleteRepository.DeleteAsync(parameter);

                return RedirectToPage("AdminEpisodePage");
            }
            return RedirectToPage("AdminEpisodePage");
        }
    }
}
