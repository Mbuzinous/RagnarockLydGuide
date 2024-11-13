using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class DisplayUsersModel : PageModel
    {
        public List<User> Users { get; set; } = new List<User>();

        private IUserValidator _userValidator;
        private IReadRepository<User> _readRepository;

        public DisplayUsersModel(ICRUDFactory<User> factory, IUserValidator userValidator)
        {
            _readRepository = factory.ReadRepository();
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
            Users = _readRepository.GetAll();
            return Page();
        }
    }
}
