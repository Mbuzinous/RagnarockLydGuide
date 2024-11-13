using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class EditQuizModel : PageModel
    {
        private BackendController<Quiz> _backendController;

        public EditQuizModel(BackendController<Quiz> backendController)
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

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _backendController.UpdateRepository.UpdateAsync(Quiz, Quiz);
            return RedirectToPage("DisplayQuizzes");
        }
    }
}
