using Kojg_Ragnarock_Guide.Interfaces;

namespace Kojg_Ragnarock_Guide.Services
{
    public class AudioFileRepository : IAudioFileRepository
    {
        private readonly IWebHostEnvironment _environment;

        public AudioFileRepository(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveAudioAsFileAsync(IFormFile audioFile)
        {
            if (audioFile == null || audioFile.Length == 0)
                throw new ArgumentException("Audio file is invalid.", nameof(audioFile));

            var newAudioFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(audioFile.FileName)}";
            var audioFullPath = Path.Combine(_environment.WebRootPath, "exhibitionAudios", newAudioFileName);

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(audioFullPath));

            using (var stream = new FileStream(audioFullPath, FileMode.Create))
            {
                await audioFile.CopyToAsync(stream);
            }

            return newAudioFileName;
        }

        public async Task<string> UpdateAudioAsync(IFormFile newAudioFile, string oldAudioFileName)
        {
            if (newAudioFile == null || newAudioFile.Length == 0)
                throw new ArgumentException("New audio file is invalid.", nameof(newAudioFile));

            var newAudioFileName = $"{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(newAudioFile.FileName)}";
            var newAudioFullPath = Path.Combine(_environment.WebRootPath, "exhibitionAudios", newAudioFileName);

            Directory.CreateDirectory(Path.GetDirectoryName(newAudioFullPath));

            using (var stream = new FileStream(newAudioFullPath, FileMode.Create))
            {
                await newAudioFile.CopyToAsync(stream);
            }

            // Delete old audio file if it exists
            if (!string.IsNullOrEmpty(oldAudioFileName))
            {
                var oldAudioFullPath = Path.Combine(_environment.WebRootPath, "exhibitionAudios", oldAudioFileName);
                if (File.Exists(oldAudioFullPath))
                {
                    File.Delete(oldAudioFullPath);
                }
            }

            return newAudioFileName;
        }

        public void DeleteAudio(string audioFileName)
        {
            if (string.IsNullOrEmpty(audioFileName))
                throw new ArgumentException("Audio file name is invalid.", nameof(audioFileName));

            var audioFullPath = Path.Combine(_environment.WebRootPath, "exhibitionAudios", audioFileName);
            if (File.Exists(audioFullPath))
            {
                File.Delete(audioFullPath);
            }
        }
    }
}
