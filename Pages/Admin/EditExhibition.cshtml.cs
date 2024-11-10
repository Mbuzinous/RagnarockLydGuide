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

        public EditExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;
        }
        //Making some empty string messages i use in html razer pages, lower i define them
        public IActionResult OnGet(int id)
        {
            //Validate ID
            if (_repo.GetById(id) == null)
            {
                return RedirectToPage("AdminEpisodePage");
            }

            oldExhibition = _repo.GetById(id);

            //Display Exhibition
            toBeUpdatedExhibition = _repo.GetById(id);
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            //Validate ID
            if (_repo.GetById(id) == null)
            {
                return RedirectToPage("AdminEpisodePage");
            }
            //Validate Model State
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //Update Exhibition
            await _repo.Update(toBeUpdatedExhibition, oldExhibition);

            return RedirectToPage("AdminEpisodePage");
        }
    }
}
