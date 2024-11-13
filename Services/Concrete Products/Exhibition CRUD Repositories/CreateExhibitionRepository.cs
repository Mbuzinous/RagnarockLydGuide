using RagnarockTourGuide.Models;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.CRUDServices
{
    public class CreateExhibitionRepository : ICreateRepository<Exhibition>
    {
        private readonly string _connectionString;
        private IFileHandler<IFormFile> _fileRepository;

        private readonly string _imageFileTarget = "exhibitionImages";
        private readonly string _audioFileTarget = "exhibitionAudios";

        public CreateExhibitionRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
        }
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
    }
}
