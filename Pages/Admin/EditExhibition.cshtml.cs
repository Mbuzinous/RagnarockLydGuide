using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{
    public class EditExhibitionModel : PageModel
    {

        private ICRUDRepository<Exhibition> _repo;

        public Exhibition oldExhibition { get; set; } = new Exhibition();

        [BindProperty]
        public Exhibition toBeUpdatedExhibition { get; set; } = new Exhibition();
        public List<int> SuggestedExhibitionNumbers { get; private set; } = new List<int>();


        public EditExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;

        }
        //Making some empty string messages i use in html razer pages, lower i define them
        public async Task<IActionResult> OnGet(int id)
        {
            oldExhibition = _repo.GetById(id);
            toBeUpdatedExhibition = oldExhibition;

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
            await _repo.Update(toBeUpdatedExhibition, oldExhibition);

            ModelState.Clear();

            return RedirectToPage("AdminEpisodePage");
        }
    }
}
