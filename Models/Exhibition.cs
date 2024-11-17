using System.ComponentModel.DataAnnotations;

namespace RagnarockTourGuide.Models
{
    public class Exhibition
    {
        public int Id { get; set; }
        public int ExhibitionNumber { get; set; }
        public int FloorNumber { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public IFormFile? ImageFile { get; set; }
        [Required]
        public IFormFile? AudioFile { get; set; }


        //Used 
        public string? ImageFileName { get; set; }
        public string? AudioFileName { get; set; }
        public string? OldImageFileName { get; set; }
        public string? OldAudioFileName { get; set; }

        // Navigation property
        public ICollection<ExhibitionQuestion> ExhibitionQuestions { get; set; } = new List<ExhibitionQuestion>();
    }
}
