namespace RagnarockTourGuide.Interfaces.PreviousRepos
{
    public interface IFileHandler<T>
    {
        Task<string> CreateAsync(IFormFile file, string folderTarget);
        Task<string> UpdateFileAsync(IFormFile newFile, string oldFileName, string folderTarget);
        void DeleteFile(string fileName, string folderTarget);
    }
}
