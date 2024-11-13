using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.ExhibitionQuestion_CRUD_Repository
{
    public class ReadExhibitionQuestionRepository : IReadRepository<ExhibitionQuestion>
    {
        private readonly string _connectionString;

        public ReadExhibitionQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
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
    }
}
