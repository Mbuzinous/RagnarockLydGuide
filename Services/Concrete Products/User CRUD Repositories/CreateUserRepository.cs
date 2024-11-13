using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Interfaces.PreviousRepos;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Services.Concrete_Products
{
    public class CreateUserRepository : ICreateRepository<User>
    {
        IFileHandler<IFormFile> _fileRepository;

        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public CreateUserRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _fileRepository = fileRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task CreateAsync(User user)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            INSERT INTO Users (Name, Email, Password, RoleId, ImageFileName)
            VALUES (@Name, @Email, @Password, @RoleId, @ImageFileName)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Password", user.Password);  // Husk at hash, hvis nødvendigt
                cmd.Parameters.AddWithValue("@RoleId", (int)user.Role);
                cmd.Parameters.AddWithValue("@ImageFileName", await _fileRepository.SaveFileAsync(user.ImageFile, _audioFileTarget));

                try
                {
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (SqlException ex)
                {
                    // Log fejlmeddelelsen her
                    Console.WriteLine($"SQL Error: {ex.Message}");
                    throw;  // Eller håndter fejlen efter behov
                }
            }
        }

    }
}
