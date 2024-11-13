namespace RagnarockTourGuide.Services.Concrete_Products.File_CRUD_Repositories
{
    public class ReadFileRepository
    {
        private readonly IWebHostEnvironment _environment;


        public ReadFileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
    }
}
