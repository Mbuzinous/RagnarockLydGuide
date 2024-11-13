using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class EditUserModel : PageModel
    {
        [BindProperty]
        public User UserToEdit { get; set; }

        private IUserValidator _userValidator;
        private IReadRepository<User> _readRepository;
        private IUpdateRepository<User> _updateRepository;

        public EditUserModel(ICRUDFactory<User> factory, IUserValidator userValidator)
        {
            _readRepository = factory.ReadRepository();
            _updateRepository = factory.UpdateRepository();
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

            UserToEdit = _readRepository.GetById(id);
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

            User oldUser = _readRepository.GetById(UserToEdit.Id);
            await _updateRepository.UpdateAsync(UserToEdit, oldUser);
            return RedirectToPage("DisplayUsers");
        }
    }
}
