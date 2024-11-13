namespace RagnarockTourGuide.Models
{
    public class ExhibitionQuestion
    {
        public int Id { get; set; }
        public int ExhibitionId { get; set; }
        public int QuestionId { get; set; }
        public bool IsTheAnswerTrue { get; set; } // Specific answer in this context

        public Exhibition Exhibition { get; set; }
        public Question Question { get; set; }
    }
}
