using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Services.Utilities
{
    public class FileHandler : IFileHandler<IFormFile>
    {
        private readonly IWebHostEnvironment _environment;


        public FileHandler(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folderTarget)
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
