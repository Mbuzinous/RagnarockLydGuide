using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.PreviousServices
{
    public class QuizCRUDRepository : IQuizCRUDRepository<Quiz>
    {
        private readonly string _connectionString;


        public QuizCRUDRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Implementering af CRUD-metoder fra ICRUDRepository

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

        public async Task UpdateAsync(Quiz quiz, Quiz notUsed)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("UPDATE Quizzes SET ExhibitionId = @ExhibitionId, Question = @Question, CorrectAnswer = @CorrectAnswer WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@ExhibitionId", quiz.ExhibitionId);
                    cmd.Parameters.AddWithValue("@Question", quiz.Question);
                    cmd.Parameters.AddWithValue("@CorrectAnswer", quiz.CorrectAnswer);
                    cmd.Parameters.AddWithValue("@Id", quiz.Id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Quizzes WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
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

        public async Task ResetDailyQuizzesAsync()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("UPDATE UserQuizzes SET IsCompleted = 0, CompletionTime = NULL WHERE IsCompleted = 1", conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
