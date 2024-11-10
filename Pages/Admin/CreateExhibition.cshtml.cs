using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{

    public class CreateExhibitionModel : PageModel
    {
        private ICRUDRepository<Exhibition> _repo;

        [BindProperty]
        public Exhibition Exhibition { get; set; }

        public CreateExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //Save Exhibition
            await _repo.Create(Exhibition);

            ModelState.Clear();

            return RedirectToPage("AdminEpisodePage");
        }
    }
}
