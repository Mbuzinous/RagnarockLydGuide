using Kojg_Ragnarock_Guide.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class DisplayUsersModel : PageModel
    {
        private readonly IUserCRUDRepository<User> _repository;

        public List<User> Users { get; set; } = new List<User>();

        private IUserValidator _userValidator;
        public DisplayUsersModel(IUserCRUDRepository<User> repository, IUserValidator userValidator)
        {
            _repository = repository;
            _userValidator = userValidator;
        }

        public IActionResult OnGet()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret p� rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirig�r til forsiden, hvis brugeren ikke har den n�dvendige rolle
                return RedirectToPage("/Index");
            }
            Users = _repository.GetAll();
            return Page();
        }
    }
}
