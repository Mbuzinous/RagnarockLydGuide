using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services
{
    public class UserExhibitionAnswerRepository : ICRUDRepository<UserExhibitionAnswer>
    {
        private readonly string _connectionString;

        public UserExhibitionAnswerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task CreateAsync(UserExhibitionAnswer userAnswer)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO UserExhibitionAnswers (UserId, ExhibitionQuestionId, UserAnswer, AnsweredAt)
                            VALUES (@UserId, @ExhibitionQuestionId, @UserAnswer, @AnsweredAt);";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userAnswer.UserId);
                    command.Parameters.AddWithValue("@ExhibitionQuestionId", userAnswer.ExhibitionQuestionId);
                    command.Parameters.AddWithValue("@UserAnswer", userAnswer.UserAnswer);
                    command.Parameters.AddWithValue("@AnsweredAt", userAnswer.AnsweredAt);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
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
        public async Task<UserExhibitionAnswer> GetByIdAsync(int id)
        {
            UserExhibitionAnswer userExhibitionAnswer = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT uea.Id, uea.UserId, uea.ExhibitionQuestionId, uea.UserAnswer, uea.AnsweredAt, 
                           eq.ExhibitionId, eq.QuestionId, q.QuestionText
                    FROM UserExhibitionAnswers uea
                    INNER JOIN ExhibitionQuestions eq ON uea.ExhibitionQuestionId = eq.Id
                    INNER JOIN Questions q ON eq.QuestionId = q.Id
                    WHERE uea.Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userExhibitionAnswer = new UserExhibitionAnswer
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            ExhibitionQuestionId = reader.GetInt32(2),
                            UserAnswer = reader.GetBoolean(3),
                            AnsweredAt = reader.GetDateTime(4),
                            ExhibitionQuestion = new ExhibitionQuestion
                            {
                                Id = reader.GetInt32(2),
                                ExhibitionId = reader.GetInt32(5),
                                QuestionId = reader.GetInt32(6),
                                Question = new Question
                                {
                                    Id = reader.GetInt32(6),
                                    QuestionText = reader.GetString(7)
                                }
                            }
                        };
                    }
                }
            }

            return userExhibitionAnswer;
        }

        public async Task<List<UserExhibitionAnswer>> GetAllAsync()
        {
            var userExhibitionAnswers = new List<UserExhibitionAnswer>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT uea.Id, uea.UserId, uea.ExhibitionQuestionId, uea.UserAnswer, uea.AnsweredAt, 
                           eq.ExhibitionId, eq.QuestionId, q.QuestionText
                    FROM UserExhibitionAnswers uea
                    INNER JOIN ExhibitionQuestions eq ON uea.ExhibitionQuestionId = eq.Id
                    INNER JOIN Questions q ON eq.QuestionId = q.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        userExhibitionAnswers.Add(new UserExhibitionAnswer
                        {
                            Id = reader.GetInt32(0),
                            UserId = reader.GetInt32(1),
                            ExhibitionQuestionId = reader.GetInt32(2),
                            UserAnswer = reader.GetBoolean(3),
                            AnsweredAt = reader.GetDateTime(4),
                            ExhibitionQuestion = new ExhibitionQuestion
                            {
                                Id = reader.GetInt32(2),
                                ExhibitionId = reader.GetInt32(5),
                                QuestionId = reader.GetInt32(6),
                                Question = new Question
                                {
                                    Id = reader.GetInt32(6),
                                    QuestionText = reader.GetString(7)
                                }
                            }
                        });
                    }
                }
            }

            return userExhibitionAnswers;
        }

        public List<UserExhibitionAnswer> FilterListByNumber(List<UserExhibitionAnswer> listOfAnswers, int userId)
        {
            var filteredList = new List<UserExhibitionAnswer>();

            foreach (var answer in listOfAnswers)
            {
                if (answer.UserId == userId)
                {
                    filteredList.Add(answer);
                }
            }

            return filteredList;
        }

        public async Task<List<int>> GetUsedNumbersAsync()
        {
            var usedIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT UserId FROM UserExhibitionAnswers";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usedIds.Add(reader.GetInt32(0));
                    }
                }
            }

            return usedIds;
        }
        public async Task UpdateAsync(UserExhibitionAnswer toBeUpdatedAnswer)
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
