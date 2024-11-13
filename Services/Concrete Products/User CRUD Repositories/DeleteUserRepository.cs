using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.User_CRUD_Repositories
{
    public class DeleteUserRepository : IDeleteRepository<User>
    {
        IFileHandler<IFormFile> _fileRepository;

        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public DeleteUserRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _fileRepository = fileRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // Delete method accepting only user ID, for compatibility with IDeleteRepository<T>
        public async Task DeleteAsync(DeleteParameter parameter)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "EXEC DeleteUser @CurrentUserRole, @TargetUserRole, @TargetUserId";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@CurrentUserRole", parameter.CurrentUserRole);
                cmd.Parameters.AddWithValue("@TargetUserRole", parameter.TargetUserRole);
                cmd.Parameters.AddWithValue("@TargetUserId", parameter.Id);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }

        }
    }
}
