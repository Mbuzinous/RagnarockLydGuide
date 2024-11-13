namespace RagnarockTourGuide.Services.Concrete_Products.Question_CRUD_Repository
{
    public class UpdateQuestionRepository
    {
        private readonly string _connectionString;

        public UpdateQuestionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
