using Kojg_Ragnarock_Guide.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class UserDetailsModel : PageModel
    {
        private readonly IUserCRUDRepository<User> _repository;

        public User UserDetails { get; set; }

        private IUserValidator _userValidator;
        public UserDetailsModel(IUserCRUDRepository<User> repository, IUserValidator userValidator)
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

            UserDetails = _repository.GetById(id);
            if (UserDetails == null)
            {
                return RedirectToPage("DisplayUsers");
            }
            return Page();
        }
    }
}
