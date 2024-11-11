using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class DisplayQuizzesModel : PageModel
    {
        private readonly IQuizCRUDRepository<Quiz>_quizRepository;

        public DisplayQuizzesModel(IQuizCRUDRepository<Quiz> quizRepository)
        {
            _quizRepository = quizRepository;
        }

        public List<Quiz> Quizzes { get; set; }

        public void OnGet()
        {
            Quizzes = _quizRepository.GetAll();
        }
    }
}
