using Kojg_Ragnarock_Guide.Interfaces;

namespace Kojg_Ragnarock_Guide.Services
{
    public class ImageFileRepository : IImageFileRepository
    {
        private readonly IWebHostEnvironment _environment;

        public ImageFileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveImageAsFileAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("Image file is invalid.", nameof(imageFile));

            var newImageFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(imageFile.FileName)}";
            var imageFullPath = Path.Combine(_environment.WebRootPath, "exhibitionImages", newImageFileName);

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(imageFullPath));

            using (var stream = new FileStream(imageFullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return newImageFileName;
        }

        public async Task<string> UpdateImageAsync(IFormFile newImageFile, string oldImageFileName)
        {
            if (newImageFile == null || newImageFile.Length == 0)
                throw new ArgumentException("New image file is invalid.", nameof(newImageFile));

            var newImageFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(newImageFile.FileName)}";
            var newImageFullPath = Path.Combine(_environment.WebRootPath, "exhibitionImages", newImageFileName);

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(newImageFullPath));

            using (var stream = new FileStream(newImageFullPath, FileMode.Create))
            {
                await newImageFile.CopyToAsync(stream);
            }

            // Delete old image file if it exists
            if (!string.IsNullOrEmpty(oldImageFileName))
            {
                var oldImageFullPath = Path.Combine(_environment.WebRootPath, "exhibitionImages", oldImageFileName);
                if (File.Exists(oldImageFullPath))
                {
                    File.Delete(oldImageFullPath);
                }
            }

            return newImageFileName;
        }

        public void DeleteImage(string imageFileName)
        {
            if (string.IsNullOrEmpty(imageFileName))
                throw new ArgumentException("Image file name is invalid.", nameof(imageFileName));

            var imageFullPath = Path.Combine(_environment.WebRootPath, "exhibitionImages", imageFileName);
            if (File.Exists(imageFullPath))
            {
                File.Delete(imageFullPath);
            }
        }
    }
}
