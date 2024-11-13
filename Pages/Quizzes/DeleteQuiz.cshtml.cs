using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class DeleteQuizModel : PageModel
    {
        private BackendController<Quiz> _backendController;

        public DeleteQuizModel(BackendController<Quiz> backendController)
        {
            _backendController = backendController;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        public IActionResult OnGet(int id)
        {
            Quiz = _backendController.ReadRepository.GetById(id);

            if (Quiz == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            _backendController.DeleteRepository.Delete(id);
            return RedirectToPage("DisplayQuizzes");
        }
    }
}
