using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Exhibitions
{
    public class FloorNumber : PageModel
    {
        private ICRUDRepository<Exhibition> repo;

        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        [BindProperty(SupportsGet = true)]
        public string FilterCriteria { get; set; }

        public FloorNumber(ICRUDRepository<Exhibition> repository)
        {
            repo = repository;
        }

        public void OnGet()
        {
            Exhibitions = repo.GetAll();
        }
    }
}
