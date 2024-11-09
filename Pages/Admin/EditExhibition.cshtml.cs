using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{
    public class EditExhibitionModel : PageModel
    {

        private ICRUDRepository<Exhibition> repo;

        public Exhibition oldExhibition { get; set; } = new Exhibition();

        [BindProperty]
        public Exhibition toBeUpdatedExhibition { get; set; } = new Exhibition();

        public string ErrorMessage { get; private set; } = "";
        public string SuccessMessage { get; private set; } = "";

        public EditExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            repo = repository;
        }
        //Making some empty string messages i use in html razer pages, lower i define them
        public void OnGet(int id)
        {
            //Validate ID
            if (id == null)
            {
                Response.Redirect("/Admin/AdminEpisodePage");
                return;
            }


            //Display Exhibition
            oldExhibition = repo.GetById(id);
            toBeUpdatedExhibition = repo.GetById(id);
        }

        public void OnPost(int id)
        {
            //Validate ID
            if (id == null)
            {
                Response.Redirect("/Admin/AdminEpisodePage");
                return;
            }

            //Validate Model State
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Udfyld alle felter";
                return;
            }

            //Update Exhibition in database
            repo.Update(toBeUpdatedExhibition, oldExhibition);

            SuccessMessage = "Udstilling blev opdateret";

            Response.Redirect("/Admin/AdminEpisodePage");

        }
    }
}
