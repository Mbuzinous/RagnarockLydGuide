using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.UserExhibitionAnswer_CRUD_Repository
{
    public class CreateUserExhibitionAnswerRepository : ICreateRepository<UserExhibitionAnswer>
    {
        private readonly string _connectionString;

        public CreateUserExhibitionAnswerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CreateAsync(UserExhibitionAnswer userAnswer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO UserExhibitionAnswers (UserId, ExhibitionQuestionId, UserAnswer, AnsweredAt)
                            VALUES (@UserId, @ExhibitionQuestionId, @UserAnswer, @AnsweredAt);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userAnswer.UserId);
                    command.Parameters.AddWithValue("@ExhibitionQuestionId", userAnswer.ExhibitionQuestionId);
                    command.Parameters.AddWithValue("@UserAnswer", userAnswer.UserAnswer);
                    command.Parameters.AddWithValue("@AnsweredAt", userAnswer.AnsweredAt);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
