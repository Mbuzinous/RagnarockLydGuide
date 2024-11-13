using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Services.Utilities;

namespace RagnarockTourGuide.Pages.Admin
{
    public class CreateExhibitionModel : PageModel
    {
        [BindProperty]
        public Exhibition Exhibition { get; set; }

        [BindProperty]
        public List<int> SelectedQuestions { get; set; }

        public List<Question> Questions { get; private set; }

        private readonly BackendController<Exhibition> _exhibitionController;
        private readonly BackendController<Question> _questionController;
        private readonly BackendController<ExhibitionQuestion> _exhibitionQuestionController;

        public CreateExhibitionModel(BackendController<Exhibition> exhibitionController,
                                     BackendController<Question> questionController,
                                     BackendController<ExhibitionQuestion> exhibitionQuestionController)
        {
            _exhibitionController = exhibitionController;
            _questionController = questionController;
            _exhibitionQuestionController = exhibitionQuestionController; // Added injection for ExhibitionQuestion
        }

        public async Task OnGetAsync()
        {
            Questions = await _questionController.ReadRepository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Questions = await _questionController.ReadRepository.GetAllAsync();
                return Page();
            }

            // Save Exhibition
            await _exhibitionController.CreateRepository.CreateAsync(Exhibition);

            // Link Selected Questions
            foreach (var questionId in SelectedQuestions)
            {
                var exhibitionQuestion = new ExhibitionQuestion
                {
                    ExhibitionId = Exhibition.Id,
                    QuestionId = questionId,
                    IsTheAnswerTrue = false // default, can be updated later
                };

                // Use the correct repository for saving ExhibitionQuestion
                await _exhibitionQuestionController.CreateRepository.CreateAsync(exhibitionQuestion);
            }

            return RedirectToPage("AdminEpisodePage");
        }
    }
}
