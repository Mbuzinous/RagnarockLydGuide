using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.Quiz_CRUD_Repositories
{
    public class DeleteQuizRepository : IDeleteRepository<Quiz>
    {
        private readonly string _connectionString;


        public DeleteQuizRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                 conn.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Quizzes WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
