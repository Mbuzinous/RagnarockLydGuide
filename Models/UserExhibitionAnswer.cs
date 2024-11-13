namespace RagnarockTourGuide.Models
{
    public class UserExhibitionAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExhibitionQuestionId { get; set; }
        public bool UserAnswer { get; set; } // User's response (True/False)
        public DateTime AnsweredAt { get; set; } = DateTime.Now;

        // Navigation properties
        public User User { get; set; }
        public ExhibitionQuestion ExhibitionQuestion { get; set; }
    }
}
