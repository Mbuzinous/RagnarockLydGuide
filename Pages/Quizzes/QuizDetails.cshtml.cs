using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class QuizDetailsModel : PageModel
    {
        private readonly IQuizCRUDRepository<Quiz> _quizRepository;

        public QuizDetailsModel(IQuizCRUDRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

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
    }
}
