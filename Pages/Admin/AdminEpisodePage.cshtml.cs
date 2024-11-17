using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Models.Enums;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.BackendController;

namespace RagnarockTourGuide.Pages.Admin
{
    public class AdminEpisodePage : PageModel
    {
        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        [BindProperty(SupportsGet = true)]
        public int FilterCriteria { get; set; }

        private ICRUDRepository<Exhibition> _crudRepo;
        private IUserValidator _userRepository;


        public AdminEpisodePage(ICRUDRepository<Exhibition> repository, IUserValidator userValidator)
        {
            _crudRepo = repository;
            _userRepository = userValidator;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userRepository.GetUserRole(HttpContext.Session);

            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.Admin || userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }
            if (FilterCriteria == 2 || FilterCriteria == 3)
            {
                Exhibitions = _crudRepo.FilterListByNumber(await _crudRepo.GetAllAsync(), FilterCriteria);
            }
            else
            {
                Exhibitions = await _crudRepo.GetAllAsync();
            }
            return Page();
        }

    }
}
