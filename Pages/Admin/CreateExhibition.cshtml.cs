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

        public List<int> SuggestedExhibitionNumbers { get; private set; } = new List<int>();    

        public CreateExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;
        }

        public async Task<IActionResult> OnGet()
        {
            var usedNumbers = await _repo.GetUsedExhibitionNumbersAsync();
            SuggestedExhibitionNumbers = Enumerable.Range(1, 100).Except(usedNumbers).ToList();

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
