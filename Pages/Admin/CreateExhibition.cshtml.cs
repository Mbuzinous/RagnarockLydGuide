using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{

    public class CreateExhibitionModel : PageModel
    {
        private ICRUDRepository<Exhibition> repo;

        [BindProperty]
        public Exhibition Exhibition { get; set; } = new Exhibition();

        //Making some empty string messages i use in html razer pages, lower i define them
        public string ErrorMessage { get; private set; } = "";
        public string SuccessMessage { get; private set; } = "";

        public CreateExhibitionModel(ICRUDRepository<Exhibition> repository)
        {
            repo = repository;
        }

        public void OnGet()
        {
        }


        public void OnPost()
        {
            //Validate Input
            if (Exhibition.ImageFile == null)
            {
                ModelState.AddModelError("Exhibition.ImageFile", "Du er nødt til at uploade et billed");
            }

            else if (Exhibition.AudioFile == null)
            {
                ModelState.AddModelError("Exhibition.AudioFile", "Du er nødt til at uploade mp3 lydfil");
            }

            else if (!ModelState.IsValid)
            {
                ErrorMessage = "Udfyld venligst alle ledige felter";
                return;
            }
            else
            {
                //Save Exhibition
                repo.Create(Exhibition);

                ModelState.Clear();

                SuccessMessage = "Udstilling er nu oprettet";

                Response.Redirect("/Admin/AdminEpisodePage");
            }
        }
    }
}
