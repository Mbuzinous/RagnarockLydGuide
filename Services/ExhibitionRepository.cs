using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services
{
    public class ExhibitionRepository : ICRUDRepository<Exhibition>
    {
        private readonly string _connectionString;
        private IFileHandler<IFormFile> _fileRepository;

        private readonly string _imageFileTarget = "exhibitionImages";
        private readonly string _audioFileTarget = "exhibitionAudios";

        public ExhibitionRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
        }

        //Create
        public async Task CreateAsync(Exhibition exhibition)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO Exhibitions (ExhibitionNumber, FloorNumber, Title, Description, ImageFileName, AudioFileName)
                    VALUES (@ExhibitionNumber, @FloorNumber, @Title, @Description, @ImageFileName, @AudioFileName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ExhibitionNumber", exhibition.ExhibitionNumber);
                cmd.Parameters.AddWithValue("@FloorNumber", exhibition.FloorNumber);
                cmd.Parameters.AddWithValue("@Title", exhibition.Title);
                cmd.Parameters.AddWithValue("@Description", exhibition.Description);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(exhibition.ImageFile, _imageFileTarget));
                cmd.Parameters.AddWithValue("@AudioFileName", await _fileRepository.SaveFileAsync(exhibition.AudioFile, _audioFileTarget));

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Read
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

        //Update
        public async Task UpdateAsync(Exhibition toBeUpdatedExhibition)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Exhibitions SET ExhibitionNumber = @ExhibitionNumber, FloorNumber = @FloorNumber, Title = @Title, 
                        Description = @Description, ImageFileName = @ImageFileName, AudioFileName = @AudioFileName 
                    WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", toBeUpdatedExhibition.Id);
                cmd.Parameters.AddWithValue("@ExhibitionNumber", toBeUpdatedExhibition.ExhibitionNumber);
                cmd.Parameters.AddWithValue("@FloorNumber", toBeUpdatedExhibition.FloorNumber);
                cmd.Parameters.AddWithValue("@Title", toBeUpdatedExhibition.Title);
                cmd.Parameters.AddWithValue("@Description", toBeUpdatedExhibition.Description);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedExhibition.ImageFile, toBeUpdatedExhibition.OldImageFileName, _imageFileTarget));
                cmd.Parameters.AddWithValue("@AudioFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedExhibition.AudioFile, toBeUpdatedExhibition.OldAudioFileName, _audioFileTarget));

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        //Delete
        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Exhibitions WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", parameter.Id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
}
