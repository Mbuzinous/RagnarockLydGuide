using Kojg_Ragnarock_Guide.Models;

namespace RagnarockTourGuide.Models
{
    public class Quiz
    {
        public int Id { get; set; }               // Unik identifikation for quizzen
        public int ExhibitionId { get; set; }     // Reference til den tilknyttede udstilling
        public string Question { get; set; }      // Spørgsmålets tekst
        public string CorrectAnswer { get; set; } // Det korrekte svar

        // Hvis quizzen har flere svarmuligheder, kan vi tilføje dem her.
        public List<string> Options { get; set; } // Valg, hvis det er en multiple-choice quiz
    }
}
