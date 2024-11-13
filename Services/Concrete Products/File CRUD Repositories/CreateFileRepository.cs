namespace RagnarockTourGuide.Services.Concrete_Products.File_CRUD_Repositories
{
    public class CreateFileRepository
    {
        private readonly IWebHostEnvironment _environment;


        public CreateFileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> CreateAsync(IFormFile file, string folderTarget)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is invalid.", nameof(file));

            var newFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";
            string fullFilePath = Path.Combine(_environment.WebRootPath, folderTarget, newFileName);

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(fullFilePath));

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return newFileName;
        }

    }
}
