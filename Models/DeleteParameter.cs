namespace RagnarockTourGuide.Models
{
    public class DeleteParameter
    {
        public int Id { get; set; }
        public int? CurrentUserRole { get; set; }
        public int? TargetUserRole { get; set; }
    }
}
