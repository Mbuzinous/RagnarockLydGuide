using Kojg_Ragnarock_Guide.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class EditUserModel : PageModel
    {
        private readonly IUserCRUDRepository<User> _repository;

        [BindProperty]
        public User UserToEdit { get; set; }

        private IUserValidator _userValidator;
        public EditUserModel(IUserCRUDRepository<User> repository, IUserValidator userValidator)
        {
            _repository = repository;
            _userValidator = userValidator;
        }

        public IActionResult OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            UserToEdit = _repository.GetById(id);
            if (UserToEdit == null)
            {
                return RedirectToPage("DisplayUsers");
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var oldUser = _repository.GetById(UserToEdit.Id);
            await _repository.UpdateAsync(UserToEdit, oldUser);
            return RedirectToPage("DisplayUsers");
        }
    }
}
