using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.Quiz_CRUD_Repositories
{
    public class ReadQuizRepository : IReadRepository<Quiz>
    {
        private readonly string _connectionString;


        public ReadQuizRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public Quiz GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Quizzes WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Quiz
                            {
                                Id = (int)reader["Id"],
                                ExhibitionId = (int)reader["ExhibitionId"],
                                Question = reader["Question"].ToString(),
                                CorrectAnswer = reader["CorrectAnswer"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }

        public List<Quiz> GetAll()
        {
            var quizzes = new List<Quiz>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Quizzes", conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            quizzes.Add(new Quiz
                            {
                                Id = (int)reader["Id"],
                                ExhibitionId = (int)reader["ExhibitionId"],
                                Question = reader["Question"].ToString(),
                                CorrectAnswer = reader["CorrectAnswer"].ToString()
                            });
                        }
                    }
                }
            }
            return quizzes;
        }



        // Implementering af specifikke metoder fra IQuizRepository

        public async Task<List<Quiz>> GetQuizzesByExhibitionIdAsync(int exhibitionId)
        {
            var quizzes = new List<Quiz>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Quizzes WHERE ExhibitionId = @ExhibitionId", conn))
                {
                    cmd.Parameters.AddWithValue("@ExhibitionId", exhibitionId);
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            quizzes.Add(new Quiz
                            {
                                Id = (int)reader["Id"],
                                ExhibitionId = (int)reader["ExhibitionId"],
                                Question = reader["Question"].ToString(),
                                CorrectAnswer = reader["CorrectAnswer"].ToString()
                            });
                        }
                    }
                }
            }
            return quizzes;
        }

        public async Task<List<int>> GetUsedQuizIdsAsync()
        {
            var quizIds = new List<int>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("SELECT DISTINCT QuizId FROM UserQuizzes", conn))
                {
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            quizIds.Add((int)reader["QuizId"]);
                        }
                    }
                }
            }
            return quizIds;
        }

        public List<Quiz> FilterListByNumber(List<Quiz> listOfT, int filterNr)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetUsedNumbersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
