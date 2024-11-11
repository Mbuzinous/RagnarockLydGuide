using Kojg_Ragnarock_Guide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Interfaces
{
    public interface IQuizCRUDRepository<T> : ICRUDRepository<T> where T : Quiz
    {
        Task<List<T>> GetQuizzesByExhibitionIdAsync(int exhibitionId); // Henter alle quizzer for en given udstilling
        Task<List<int>> GetUsedQuizIdsAsync(); // Henter quiz-id'er, som er blevet brugt i "UserQuizzes"
        Task ResetDailyQuizzesAsync(); // Nulstiller quiz-data dagligt
        Task DeleteAsync(int id);
    }
}
