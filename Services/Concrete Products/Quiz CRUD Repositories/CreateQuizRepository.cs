using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.Quiz_CRUD_Repositories
{
    public class CreateQuizRepository : ICreateRepository<Quiz>
    {
        private readonly string _connectionString;


        public CreateQuizRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CreateAsync(Quiz quiz)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Quizzes (ExhibitionId, Question, CorrectAnswer) VALUES (@ExhibitionId, @Question, @CorrectAnswer)", conn))
                {
                    cmd.Parameters.AddWithValue("@ExhibitionId", quiz.ExhibitionId);
                    cmd.Parameters.AddWithValue("@Question", quiz.Question);
                    cmd.Parameters.AddWithValue("@CorrectAnswer", quiz.CorrectAnswer);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
