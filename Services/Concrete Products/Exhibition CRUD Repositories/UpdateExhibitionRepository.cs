using RagnarockTourGuide.Models;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.Concrete_Products.Exhibition_CRUD_Repositories
{
    public class UpdateExhibitionRepository : IUpdateRepository<Exhibition>
    {
        private readonly string _connectionString;
        private IFileHandler<IFormFile> _fileRepository;

        private readonly string _imageFileTarget = "exhibitionImages";
        private readonly string _audioFileTarget = "exhibitionAudios";

        public UpdateExhibitionRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
        }
        public async Task UpdateAsync(Exhibition toBeUpdatedExhibition, Exhibition oldExhibition)
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
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedExhibition.ImageFile, oldExhibition.ImageFileName, _imageFileTarget));
                cmd.Parameters.AddWithValue("@AudioFileName", await _fileRepository.UpdateFileAsync(toBeUpdatedExhibition.AudioFile, oldExhibition.AudioFileName, _audioFileTarget));

                conn.Open();
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}
