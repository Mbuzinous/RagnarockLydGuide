using RagnarockTourGuide.Models;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.Concrete_Products.Exhibition_CRUD_Repositories
{
    public class DeleteExhibitionRepository : IDeleteRepository<Exhibition>
    {
        private readonly string _connectionString;
        private IFileHandler<IFormFile> _fileRepository;

        private readonly string _imageFileTarget = "exhibitionImages";
        private readonly string _audioFileTarget = "exhibitionAudios";

        public DeleteExhibitionRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _fileRepository = fileRepository;
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

    }
}
