namespace RagnarockTourGuide.Services.Concrete_Products.File_CRUD_Repositories
{
    public class DeleteFileRepository
    {
        private readonly IWebHostEnvironment _environment;


        public DeleteFileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public void DeleteFile(string fileName, string folderTarget)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is invalid.", nameof(fileName));
            string fileFullPath = Path.Combine(_environment.WebRootPath, folderTarget, fileName);

            if (File.Exists(fileFullPath))
            {
                File.Delete(fileFullPath);
            }
        }

    }
}
