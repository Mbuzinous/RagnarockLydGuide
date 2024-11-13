using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.Quiz_CRUD_Repositories
{
    public class UpdateQuizRepository : IUpdateRepository<Quiz>
    {
        private readonly string _connectionString;


        public UpdateQuizRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task UpdateAsync(Quiz quiz, Quiz notUsed)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("UPDATE Quizzes SET ExhibitionId = @ExhibitionId, Question = @Question, CorrectAnswer = @CorrectAnswer WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@ExhibitionId", quiz.ExhibitionId);
                    cmd.Parameters.AddWithValue("@Question", quiz.Question);
                    cmd.Parameters.AddWithValue("@CorrectAnswer", quiz.CorrectAnswer);
                    cmd.Parameters.AddWithValue("@Id", quiz.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task ResetDailyQuizzesAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("UPDATE UserQuizzes SET IsCompleted = 0, CompletionTime = NULL WHERE IsCompleted = 1", conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }



    }
}
