using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class DeleteQuizModel : PageModel
    {
        private readonly IQuizCRUDRepository<Quiz> _quizRepository;

        public DeleteQuizModel(IQuizCRUDRepository<Quiz> quizRepository)
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

        public async Task<IActionResult> OnPostAsync(int id)
        {
            await _quizRepository.DeleteAsync(id);
            return RedirectToPage("DisplayQuizzes");
        }
    }
}
