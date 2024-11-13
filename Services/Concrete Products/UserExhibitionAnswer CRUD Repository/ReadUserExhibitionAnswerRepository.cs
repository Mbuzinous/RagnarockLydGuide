using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.UserExhibitionAnswer_CRUD_Repository
{
    public class ReadUserExhibitionAnswerRepository : IReadRepository<UserExhibitionAnswer>
    {
        private readonly string _connectionString;

        public ReadUserExhibitionAnswerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
    }
}
