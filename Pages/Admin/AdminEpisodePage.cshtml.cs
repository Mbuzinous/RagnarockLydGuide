using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Admin
{
    public class AdminEpisodePage : PageModel
    {
        private ICRUDRepository<Exhibition> _repo;

        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        [BindProperty(SupportsGet = true)]
        public int FilterCriteria { get; set; }

        public AdminEpisodePage(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;
        }
        public IActionResult OnGet()
        {
            if (FilterCriteria == 2 || FilterCriteria == 3)
            {
                Exhibitions = _repo.FilterListByNumber(_repo.GetAll(), FilterCriteria);
            }
            else
            {
                Exhibitions = _repo.GetAll();
            }
            return Page();
        }
    }
}
