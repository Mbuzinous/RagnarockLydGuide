using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.ExhibitionQuestion_CRUD_Repository
{
    public class DeleteExhibitionQuestionRepository : IDeleteRepository<ExhibitionQuestion>
    {
        private readonly string _connectionString;

        public DeleteExhibitionQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM ExhibitionQuestions WHERE Id = @ExhibitionQuestionId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ExhibitionQuestionId", parameter.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
