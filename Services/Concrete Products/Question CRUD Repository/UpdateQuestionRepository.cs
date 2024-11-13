using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.Question_CRUD_Repository
{
    public class UpdateQuestionRepository : IUpdateRepository<Question>
    {
        private readonly string _connectionString;

        public UpdateQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task UpdateAsync(Question toBeUpdatedQuestion, Question oldQuestion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Questions 
                    SET QuestionText = @QuestionText, 
                        RealAnswer = @RealAnswer
                    WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", toBeUpdatedQuestion.Id);
                cmd.Parameters.AddWithValue("@QuestionText", toBeUpdatedQuestion.QuestionText ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@RealAnswer", toBeUpdatedQuestion.RealAnswer);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
