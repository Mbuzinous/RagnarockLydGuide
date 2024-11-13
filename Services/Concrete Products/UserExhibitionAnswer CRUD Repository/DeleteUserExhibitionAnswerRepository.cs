using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.UserExhibitionAnswer_CRUD_Repository
{
    public class DeleteUserExhibitionAnswerRepository : IDeleteRepository<UserExhibitionAnswer>
    {
        private readonly string _connectionString;

        public DeleteUserExhibitionAnswerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM UserExhibitionAnswers WHERE Id = @UserExhibitionAnswerId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserExhibitionAnswerId", parameter.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
