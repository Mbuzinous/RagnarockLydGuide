using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;


namespace RagnarockTourGuide.Services.Concrete_Products.ExhibitionQuestionCRUDRepository
{
    public class CreateExhibitionQuestionRepository : ICreateRepository<ExhibitionQuestion>
    {
        private readonly string _connectionString;

        public CreateExhibitionQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task CreateAsync(ExhibitionQuestion exhibitionQuestion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            INSERT INTO ExhibitionQuestions (ExhibitionId, QuestionId, IsTheAnswerTrue)
            VALUES (@ExhibitionId, @QuestionId, @IsTheAnswerTrue);
        ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExhibitionId", exhibitionQuestion.Id);
                    command.Parameters.AddWithValue("@QuestionId", exhibitionQuestion.QuestionId);
                    command.Parameters.AddWithValue("@IsTheAnswerTrue", exhibitionQuestion.IsTheAnswerTrue);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
