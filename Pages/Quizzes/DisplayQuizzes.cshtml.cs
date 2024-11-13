using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Quizzes
{
    public class DisplayQuizzesModel : PageModel
    {
        private BackendController<Quiz> _backendController;

        public DisplayQuizzesModel(BackendController<Quiz> backendController)
        {
            _backendController = backendController;
        }

        public List<Quiz> Quizzes { get; set; }

        public void OnGet()
        {
            Quizzes = _backendController.ReadRepository.GetAll();
        }
    }
}
