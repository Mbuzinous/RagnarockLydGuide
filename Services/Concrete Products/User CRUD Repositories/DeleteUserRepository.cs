using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.Concrete_Products.User_CRUD_Repositories
{
    public class DeleteUserRepository 
    {
        IFileHandler<IFormFile> _fileRepository;

        private readonly string _connectionString;
        private readonly string _audioFileTarget = "userImages";


        public DeleteUserRepository(IConfiguration configuration, IFileHandler<IFormFile> fileRepository)
        {
            _fileRepository = fileRepository;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // Delete a user by ID
        public void Delete(int currentUserRole, int targetUserRole, int targetUserId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {


                string query = "EXEC DeleteUser @CurrentUserRole, @TargetUserRole, @TargetUserId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CurrentUserRole", currentUserRole);
                cmd.Parameters.AddWithValue("@TargetUserRole", targetUserRole);
                cmd.Parameters.AddWithValue("@TargetUserId", targetUserId);

                conn.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                }

                catch (SqlException sqlEx)
                {
                    if (sqlEx.Number == 50003)
                    {
                        Console.WriteLine("Unauthorized: Cannot delete a user with the same or higher role.");
                    }
                    else
                    {
                        Console.WriteLine($"SQL Error: {sqlEx.Message}");
                    }
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                    throw;
                }

            }
        }

    }
}
