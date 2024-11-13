namespace RagnarockTourGuide.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public bool RealAnswer { get; set; } // Global correct answer for the question

        // Navigation property for ExhibitionQuestions relationship
        public ICollection<ExhibitionQuestion> ExhibitionQuestions { get; set; } = new List<ExhibitionQuestion>();
    }

}
