using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.ExhibitionQuestion_CRUD_Repository
{
    public class UpdateExhibitionQuestionRepository : IUpdateRepository<ExhibitionQuestion>
    {
        private readonly string _connectionString;

        public UpdateExhibitionQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task UpdateAsync(ExhibitionQuestion toBeUpdatedExhibitionQuestion, ExhibitionQuestion oldExhibitionQuestion)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE ExhibitionQuestions SET 
                        ExhibitionId = @ExhibitionId, 
                        QuestionId = @QuestionId, 
                        IsTheAnswerTrue = @IsTheAnswerTrue 
                    WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", toBeUpdatedExhibitionQuestion.Id);
                cmd.Parameters.AddWithValue("@ExhibitionId", toBeUpdatedExhibitionQuestion.ExhibitionId);
                cmd.Parameters.AddWithValue("@QuestionId", toBeUpdatedExhibitionQuestion.QuestionId);
                cmd.Parameters.AddWithValue("@IsTheAnswerTrue", toBeUpdatedExhibitionQuestion.IsTheAnswerTrue);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
