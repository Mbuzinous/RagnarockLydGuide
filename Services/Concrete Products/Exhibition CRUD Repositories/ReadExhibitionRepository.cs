using RagnarockTourGuide.Models;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.Concrete_Products.Exhibition_CRUD_Repositories
{
    public class ReadExhibitionRepository : IReadRepository<Exhibition>
    {
        private readonly string _connectionString;
        private IFileHandler<IFormFile> _fileRepository;

        private readonly string _imageFileTarget = "exhibitionImages";
        private readonly string _audioFileTarget = "exhibitionAudios";

        public ReadExhibitionRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
        }
        public async Task<Exhibition> GetByIdAsync(int id)
        {
            Exhibition exhibition = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Exhibitions WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                await conn.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        exhibition = new Exhibition
                        {
                            Id = (int)reader["Id"],
                            ExhibitionNumber = (int)reader["ExhibitionNumber"],
                            FloorNumber = (int)reader["FloorNumber"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageFileName = reader["ImageFileName"].ToString(),
                            AudioFileName = reader["AudioFileName"].ToString()
                        };
                    }
                }
            }

            return exhibition;
        }

        public async Task<List<Exhibition>> GetAllAsync()
        {
            var exhibitions = new List<Exhibition>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Exhibitions";

                SqlCommand cmd = new SqlCommand(query, conn);
                await conn.OpenAsync();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        exhibitions.Add(new Exhibition
                        {
                            Id = (int)reader["Id"],
                            ExhibitionNumber = (int)reader["ExhibitionNumber"],
                            FloorNumber = (int)reader["FloorNumber"],
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageFileName = reader["ImageFileName"].ToString(),
                            AudioFileName = reader["AudioFileName"].ToString()
                        });
                    }
                }
            }

            return exhibitions;
        }
        public List<Exhibition> FilterListByNumber(List<Exhibition> exhibitions, int floorNr)
        {
            List<Exhibition> filteredList = new List<Exhibition>();

            foreach (Exhibition existingExhibition in exhibitions)
            {
                if (existingExhibition.FloorNumber == floorNr)
                {
                    filteredList.Add(existingExhibition);
                }
            }
            return filteredList;
        }

        public async Task<List<int>> GetUsedNumbersAsync()
        {
            List<int> usedNumbers = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ExhibitionNumber FROM Exhibitions";
                SqlCommand cmd = new SqlCommand(query, conn);

                await conn.OpenAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        usedNumbers.Add(reader.GetInt32(0));
                    }
                }
            }

            return usedNumbers;
        }

    }
}
