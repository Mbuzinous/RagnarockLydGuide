namespace RagnarockTourGuide.Models
{
    public class UserQuiz
    {
        public int Id { get; set; }                // Primærnøgle
        public int UserId { get; set; }            // Reference til brugeren
        public int QuizId { get; set; }            // Reference til quizzen
        public bool IsCompleted { get; set; }      // Om quizzen er gennemført
        public DateTime? CompletionTime { get; set; } // Tidspunkt for gennemførelse
        public int Score { get; set; }             // Score for quizzen
    }
}
