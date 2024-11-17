using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services
{
    public class ExhibitionQuestionRepository : ICRUDRepository<ExhibitionQuestion>
    {
        private readonly string _connectionString;

        public ExhibitionQuestionRepository(IConfiguration configuration)
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
        public async Task<ExhibitionQuestion> GetByIdAsync(int id)
        {
            ExhibitionQuestion exhibitionQuestion = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT eq.Id, eq.ExhibitionId, eq.QuestionId, eq.IsTheAnswerTrue, 
                           q.QuestionText, q.RealAnswer 
                    FROM ExhibitionQuestions eq
                    INNER JOIN Questions q ON eq.QuestionId = q.Id
                    WHERE eq.Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        exhibitionQuestion = new ExhibitionQuestion
                        {
                            Id = reader.GetInt32(0),
                            ExhibitionId = reader.GetInt32(1),
                            QuestionId = reader.GetInt32(2),
                            IsTheAnswerTrue = reader.GetBoolean(3),
                            Question = new Question
                            {
                                Id = reader.GetInt32(2),
                                QuestionText = reader.GetString(4),
                                RealAnswer = reader.GetBoolean(5)
                            }
                        };
                    }
                }
            }

            return exhibitionQuestion;
        }

        public async Task<List<ExhibitionQuestion>> GetAllAsync()
        {
            var exhibitionQuestions = new List<ExhibitionQuestion>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT eq.Id, eq.ExhibitionId, eq.QuestionId, eq.IsTheAnswerTrue, 
                           q.QuestionText, q.RealAnswer 
                    FROM ExhibitionQuestions eq
                    INNER JOIN Questions q ON eq.QuestionId = q.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        exhibitionQuestions.Add(new ExhibitionQuestion
                        {
                            Id = reader.GetInt32(0),
                            ExhibitionId = reader.GetInt32(1),
                            QuestionId = reader.GetInt32(2),
                            IsTheAnswerTrue = reader.GetBoolean(3),
                            Question = new Question
                            {
                                Id = reader.GetInt32(2),
                                QuestionText = reader.GetString(4),
                                RealAnswer = reader.GetBoolean(5)
                            }
                        });
                    }
                }
            }

            return exhibitionQuestions;
        }

        public List<ExhibitionQuestion> FilterListByNumber(List<ExhibitionQuestion> listOfT, int exhibitionId)
        {
            var filteredList = new List<ExhibitionQuestion>();

            foreach (var question in listOfT)
            {
                if (question.ExhibitionId == exhibitionId)
                {
                    filteredList.Add(question);
                }
            }

            return filteredList;
        }

        public async Task<List<int>> GetUsedNumbersAsync()
        {
            var usedQuestionIds = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT DISTINCT QuestionId FROM ExhibitionQuestions";
                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usedQuestionIds.Add(reader.GetInt32(0));
                    }
                }
            }

            return usedQuestionIds;
        }
        public async Task UpdateAsync(ExhibitionQuestion toBeUpdatedExhibitionQuestion)
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
