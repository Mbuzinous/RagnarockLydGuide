using RagnarockTourGuide.Enums;

namespace RagnarockTourGuide.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public Role Role { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? ImageFileName { get; set; }
        // Navigation property for UserExhibitionAnswers relationship
        public ICollection<UserExhibitionAnswer> UserExhibitionAnswers { get; set; } = new List<UserExhibitionAnswer>();

    }
}
