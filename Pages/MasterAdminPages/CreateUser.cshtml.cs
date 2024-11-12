using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class CreateUserModel : PageModel
    {
        private readonly IUserCRUDRepository<User> _repository;

        [BindProperty]
        public User NewUser { get; set; } = new User();

        private IUserValidator _userValidator;
        public CreateUserModel(IUserCRUDRepository<User> repository, IUserValidator userValidator)
        {
            _repository = repository;
            _userValidator = userValidator;
        }

        public IActionResult OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            return Page();
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repository.CreateAsync(NewUser);
            return RedirectToPage("DisplayUsers");
        }
    }
}
