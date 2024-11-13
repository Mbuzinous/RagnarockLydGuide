using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products.User_CRUD_Repositories
{
    public class UpdateUserRepository : IUpdateRepository<User>
    {
        IFileHandler<IFormFile> _fileRepository;

        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public UpdateUserRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _fileRepository = fileRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task UpdateAsync(User user, User notUsed)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Users SET Name = @Name, Email = @Email, Password = @Password, 
                        RoleId = @RoleId, ImageFileName = @ImageFileName 
                    WHERE Id = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", user.Id);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@RoleId", (int)user.Role);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(user.ImageFile, _audioFileTarget));

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

    }
}
