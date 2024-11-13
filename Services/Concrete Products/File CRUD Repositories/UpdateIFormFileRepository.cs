using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.Concrete_Products.File_CRUD_Repositories
{
    public class UpdateIFormFileRepository
    {
        private readonly IWebHostEnvironment _environment;


        public UpdateIFormFileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<string> UpdateFileAsync(IFormFile newFile, string oldFileName, string folderTarget)
        {
            if (newFile == null || newFile.Length == 0)
                throw new ArgumentException("New file is invalid.", nameof(newFile));

            string newFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(newFile.FileName)}";

            string newFileFullPath = Path.Combine(_environment.WebRootPath, folderTarget, newFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(newFileFullPath));

            using (FileStream stream = new FileStream(newFileFullPath, FileMode.Create))
            {
                await newFile.CopyToAsync(stream);
            }

            // Delete old audio file if it exists
            if (!string.IsNullOrEmpty(oldFileName))
            {
                string oldFileFullPath = Path.Combine(_environment.WebRootPath, folderTarget, oldFileName);
                if (File.Exists(oldFileFullPath))
                {
                    File.Delete(oldFileFullPath);
                }
            }

            return newFileName;
        }

    }
}
