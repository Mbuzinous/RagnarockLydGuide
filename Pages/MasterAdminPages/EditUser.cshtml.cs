using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Models.Enums;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public User UserToEdit { get; set; }

        private BackendController<User> _backendController;

        public EditUserModel(BackendController<User> backendController)
        {
            _backendController = backendController;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            UserToEdit = await _backendController.ReadRepository.GetByIdAsync(id);
            if (UserToEdit == null)
            {
                return RedirectToPage("DisplayUsers");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            User oldUser = await _backendController.ReadRepository.GetByIdAsync(UserToEdit.Id);
            await _backendController.UpdateRepository.UpdateAsync(UserToEdit, oldUser);
            return RedirectToPage("DisplayUsers");
        }
    }
}
