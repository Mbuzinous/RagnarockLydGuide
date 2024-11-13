using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class QuizDetailsModel : PageModel
    {
        private BackendController<Quiz> _backendController;

        public QuizDetailsModel(BackendController<Quiz> controller)
        {
            _backendController = controller;
        }

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
    }
}
