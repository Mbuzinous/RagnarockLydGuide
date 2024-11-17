using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Exhibitions
{
    public class ExhibitionQuestionsModel : PageModel
    {
        public Exhibition Exhibition { get; set; }
        public List<ExhibitionQuestion> ExhibitionQuestions { get; set; } = new List<ExhibitionQuestion>();

        [BindProperty]
        public List<bool> UserAnswers { get; set; } // To bind user's answers to each question

        private readonly BackendController<Exhibition> _exhibitionController;
        private readonly BackendController<UserExhibitionAnswer> _userAnswerController;

        public ExhibitionQuestionsModel(BackendController<Exhibition> exhibitionController, BackendController<UserExhibitionAnswer> userAnswerController)
        {
            _exhibitionController = exhibitionController;
            _userAnswerController = userAnswerController;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Exhibition = await _exhibitionController.ReadRepository.GetByIdAsync(id);
            ExhibitionQuestions = Exhibition.ExhibitionQuestions?.ToList() ?? new List<ExhibitionQuestion>();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (ExhibitionQuestions.Count != UserAnswers.Count)
            {
                ModelState.AddModelError(string.Empty, "Mismatch between questions and answers.");
                return Page();
            }

            for (int i = 0; i < ExhibitionQuestions.Count; i++)
            {
                var userAnswer = new UserExhibitionAnswer
                {
                    UserId = userId,
                    ExhibitionQuestionId = ExhibitionQuestions[i].Id,
                    UserAnswer = UserAnswers[i],
                    AnsweredAt = DateTime.Now
                };

                await _userAnswerController.CreateRepository.CreateAsync(userAnswer);
            }

            return RedirectToPage("ExhibitionSummary", new { id });
        }
    }
}
