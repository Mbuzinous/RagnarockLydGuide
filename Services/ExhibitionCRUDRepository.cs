using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Kojg_Ragnarock_Guide.Services;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces;

namespace RagnarockTourGuide.Services
{
    public class ExhibitionCRUDRepository : ICRUDRepository<Exhibition>
    {
        private readonly string _connectionString;
        private IFIleRepository<IFormFile> _fileRepository;

        private readonly string _imageFileTarget = "exhibitionImages";
        private readonly string _audioFileTarget = "exhibitionAudios";
        public ExhibitionCRUDRepository(IConfiguration configuration, IFIleRepository<IFormFile> fileRepository)
        {
            // Hent forbindelsesstrengen fra konfigurationen
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
        }
        public async Task Create(Exhibition toBeCreatedExhibition)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Exhibitions (ExhibitionNumber, FloorNumber, Title, Description, ImageFileName, AudioFileName) " +
                               "VALUES (@ExhibitionNumber, @FloorNumber, @Title, @Description, @ImageFileName, @AudioFileName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ExhibitionNumber", toBeCreatedExhibition.ExhibitionNumber);
                cmd.Parameters.AddWithValue("@FloorNumber", toBeCreatedExhibition.FloorNumber);
                cmd.Parameters.AddWithValue("@Title", toBeCreatedExhibition.Title);
                cmd.Parameters.AddWithValue("@Description", toBeCreatedExhibition.Description);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(toBeCreatedExhibition.ImageFile, _imageFileTarget));
                cmd.Parameters.AddWithValue("@AudioFileName", await _fileRepository.SaveFileAsync(toBeCreatedExhibition.AudioFile, _audioFileTarget));

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Exhibition GetById(int id)
        {
            Exhibition exhibitionToGet = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Exhibitions WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        exhibitionToGet = new Exhibition
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ExhibitionNumber = Convert.ToInt32(reader["ExhibitionNumber"]),
                            FloorNumber = Convert.ToInt32(reader["FloorNumber"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageFileName = reader["ImageFileName"].ToString(),
                            AudioFileName = reader["AudioFileName"].ToString()
                        };
                    }
                }
            }

            return exhibitionToGet;
        }
        public List<Exhibition> GetAll()
        {
            List<Exhibition> listOfAllExhibitions = new List<Exhibition>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Exhibitions";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listOfAllExhibitions.Add(new Exhibition
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            ExhibitionNumber = Convert.ToInt32(reader["ExhibitionNumber"]),
                            FloorNumber = Convert.ToInt32(reader["FloorNumber"]),
                            Title = reader["Title"].ToString(),
                            Description = reader["Description"].ToString(),
                            ImageFileName = reader["ImageFileName"].ToString(),
                            AudioFileName = reader["AudioFileName"].ToString()
                        });
                    }
                }
            }

            return listOfAllExhibitions;
        }
        public async Task Update(Exhibition toBeUpdatedExhibition, Exhibition oldExhibition)
        {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE Exhibitions SET ExhibitionNumber = @ExhibitionNumber, FloorNumber = @FloorNumber, " +
                                   "Title = @Title, Description = @Description, ImageFileName = @ImageFileName, AudioFileName = @AudioFileName " +
                                   "WHERE Id = @Id";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", toBeUpdatedExhibition.Id);
                    cmd.Parameters.AddWithValue("@ExhibitionNumber", toBeUpdatedExhibition.ExhibitionNumber);
                    cmd.Parameters.AddWithValue("@FloorNumber", toBeUpdatedExhibition.FloorNumber);
                    cmd.Parameters.AddWithValue("@Title", toBeUpdatedExhibition.Title);
                    cmd.Parameters.AddWithValue("@Description", toBeUpdatedExhibition.Description);
                    cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedExhibition.ImageFile, oldExhibition.ImageFileName, _imageFileTarget));
                    cmd.Parameters.AddWithValue("@AudioFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedExhibition.AudioFile, oldExhibition.AudioFileName, _audioFileTarget));

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        public void Delete(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Exhibitions WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
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
        public async Task<List<int>> GetUsedExhibitionNumbersAsync()
        {
            List<int> usedNumbers = new List<int>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT ExhibitionNumber FROM Exhibitions";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
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
