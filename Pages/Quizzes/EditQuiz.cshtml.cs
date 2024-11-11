using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class EditQuizModel : PageModel
    {
        private readonly IQuizCRUDRepository<Quiz> _quizRepository;

        public EditQuizModel(IQuizCRUDRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        public IActionResult OnGet(int id)
        {
            Quiz =  _quizRepository.GetById(id);

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

            await _quizRepository.UpdateAsync(Quiz, Quiz);
            return RedirectToPage("DisplayQuizzes");
        }
    }
}
