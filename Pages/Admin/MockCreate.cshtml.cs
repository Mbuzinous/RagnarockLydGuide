using RagnarockTourGuide.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RagnarockTourGuide.Interfaces.PreviousRepos;

namespace RagnarockTourGuide.Pages.Admin
{
    public class MockCreateModel : PageModel
    {
        public Exhibition exhibition { get; set; } 
        public User user { get; set; }
        public MockCreateModel()
        {
        }
        public void OnGet()
        {

        }
    }
}
