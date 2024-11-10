using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{
    public class DeleteExhibitionModel : PageModel
    {
        private ICRUDRepository<Exhibition> _repo;

        public DeleteExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;
        }
        public IActionResult OnGet(int id)
        {
            //Validate ID
            if (_repo.GetById(id) != null)
            {
                //Delete Exhibition
                _repo.Delete(id);

                return RedirectToPage("AdminEpisodePage");
            }
            return RedirectToPage("AdminEpisodePage");
        }
    }
}
