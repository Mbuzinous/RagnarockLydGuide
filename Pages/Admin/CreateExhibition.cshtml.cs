using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.BackendController;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Admin
{
    public class CreateExhibitionModel : PageModel
    {
        [BindProperty]
        public Exhibition Exhibition { get; set; }

        public List<Question> Questions { get; private set; }
        public List<Question> SelectedQuestions { get; set; }


        private ICRUDRepository<Exhibition> _crudRepo;
        private IUserValidator _userRepository;

        public CreateExhibitionModel(ICRUDRepository<Exhibition> repository, IUserValidator userValidator)
        {
            _crudRepo = repository;
            _userRepository = userValidator;
        }

        public async Task OnGetAsync()
        {
            Questions = await _questionBackendController.CRUDRepository.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Questions = await _questionBackendController.CRUDRepository.GetAllAsync();
                return Page();
            }

            // Save Exhibition
            await _exhibitionBackendController.CRUDRepository.CreateAsync(Exhibition);

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
