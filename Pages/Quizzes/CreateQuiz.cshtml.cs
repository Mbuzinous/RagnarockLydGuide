using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Concrete_Products.Quiz_CRUD_Repositories;
using RagnarockTourGuide.Services.ConcreteFactories;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class CreateQuizModel : PageModel
    {
        private BackendController<Quiz> _backendController;

        public CreateQuizModel(BackendController<Quiz> backendController)
        {
            _backendController = backendController;
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
           await _backendController.CreateRepository.CreateAsync(Quiz);
            return RedirectToPage("DisplayQuizzes");
        }
    }
}
