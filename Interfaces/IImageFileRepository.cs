namespace Kojg_Ragnarock_Guide.Interfaces
{
    public interface IImageFileRepository
    {
        Task<string> SaveImageAsFileAsync(IFormFile imageFile);
        Task<string> UpdateImageAsync(IFormFile newImageFile, string oldImageFileName);
        void DeleteImage(string audioFileName);
    }
}
