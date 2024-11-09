namespace Kojg_Ragnarock_Guide.Interfaces
{
    public interface IAudioFileRepository
    {
        Task<string> SaveAudioAsFileAsync(IFormFile audioFile);
        Task<string> UpdateAudioAsync(IFormFile newAudioFile, string oldAudioFileName);
        void DeleteAudio(string audioFileName);
    }
}
