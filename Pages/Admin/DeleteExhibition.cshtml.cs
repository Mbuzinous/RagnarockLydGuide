using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{
    public class DeleteExhibitionModel : PageModel
    {
        private ICRUDRepository<Exhibition> repo;

        public DeleteExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            repo = repository;
        }
        public void OnGet(int id)
        {
            //Validate ID
            if (id == null)
            {
                Response.Redirect("/Admin/AdminEpisodePage");
                return;
            }

            //Delete Exhibition
            repo.Delete(id);

            //redirect to Index page
            Response.Redirect("/Admin/AdminEpisodePage");
        }
    }
}
