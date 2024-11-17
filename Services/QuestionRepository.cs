using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services
{
    public class QuestionRepository : ICRUDRepository<Question>
    {
        private readonly string _connectionString;

        public QuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CreateAsync(Question question)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            INSERT INTO Questions (QuestionText, RealAnswer)
            VALUES (@QuestionText, @RealAnswer);
        ";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionText", question.QuestionText);
                    command.Parameters.AddWithValue("@RealAnswer", question.RealAnswer);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "DELETE FROM Questions WHERE Id = @QuestionId;";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@QuestionId", parameter.Id);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task<Question> GetByIdAsync(int id)
        {
            Question question = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Questions WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        question = new Question
                        {
                            Id = (int)reader["Id"],
                            QuestionText = reader["QuestionText"].ToString(),
                            RealAnswer = (bool)reader["RealAnswer"]
                        };
                    }
                }
            }

            return question;
        }

        public async Task<List<Question>> GetAllAsync()
        {
            var questions = new List<Question>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Questions";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        questions.Add(new Question
                        {
                            Id = (int)reader["Id"],
                            QuestionText = reader["QuestionText"].ToString(),
                            RealAnswer = (bool)reader["RealAnswer"]
                        });
                    }
                }
            }

            return questions;
        }

        public List<Question> FilterListByNumber(List<Question> listOfQuestions, int filterNr)
        {
            var filteredList = new List<Question>();

            foreach (var question in listOfQuestions)
            {
                if (question.Id == filterNr) // Assuming we filter by question ID
                {
                    filteredList.Add(question);
                }
            }

            return filteredList;
        }

        public async Task<List<int>> GetUsedNumbersAsync()
        {
            var usedIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT Id FROM Questions";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usedIds.Add((int)reader["Id"]);
                    }
                }
            }

            return usedIds;
        }
        public async Task UpdateAsync(Question toBeUpdatedQuestion)
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
