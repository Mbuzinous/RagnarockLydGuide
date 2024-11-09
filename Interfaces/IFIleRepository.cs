namespace RagnarockTourGuide.Interfaces
{
    public interface IFIleRepository<T>
    {
        Task<string> SaveFileAsync(T file, string folderTarget);
        Task<string> UpdateFileAsync(IFormFile newFile, string oldFileName, string folderTarget);
        void DeleteFile(string audioFileName, string folderTarget);
    }
}
