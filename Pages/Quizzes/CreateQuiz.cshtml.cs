using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class CreateQuizModel : PageModel
    {
        private readonly IQuizCRUDRepository<Quiz> _quizRepository;

        public CreateQuizModel(IQuizCRUDRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        [BindProperty]
        public Quiz Quiz { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _quizRepository.CreateAsync(Quiz);
            return RedirectToPage("DisplayQuizzes");
        }
    }
}
