using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.UserExhibitionAnswer_CRUD_Repository
{
    public class UpdateUserExhibitionAnswerRepository : IUpdateRepository<UserExhibitionAnswer>
    {
        private readonly string _connectionString;

        public UpdateUserExhibitionAnswerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task UpdateAsync(UserExhibitionAnswer toBeUpdatedAnswer, UserExhibitionAnswer oldAnswer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE UserExhibitionAnswers 
                    SET UserAnswer = @UserAnswer, 
                        AnsweredAt = @AnsweredAt
                    WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", toBeUpdatedAnswer.Id);
                cmd.Parameters.AddWithValue("@UserAnswer", toBeUpdatedAnswer.UserAnswer);
                cmd.Parameters.AddWithValue("@AnsweredAt", toBeUpdatedAnswer.AnsweredAt);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
