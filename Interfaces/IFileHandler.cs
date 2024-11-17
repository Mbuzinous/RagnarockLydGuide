namespace RagnarockTourGuide.Interfaces
{
    public interface IFileHandler<T>
    {
        Task<string> SaveFileAsync(IFormFile file, string folderTarget);
        Task<string> UpdateFileAsync(IFormFile newFile, string oldFileName, string folderTarget);
        void DeleteFile(string fileName, string folderTarget);
    }
}
