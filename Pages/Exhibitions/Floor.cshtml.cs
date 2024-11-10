using Kojg_Ragnarock_Guide.Interfaces;
using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kojg_Ragnarock_Guide.Pages.Exhibitions
{
    public class FloorNumber : PageModel
    {
        private ICRUDRepository<Exhibition> _repo;

        public List<Exhibition> Exhibitions { get; private set; } = new List<Exhibition>();

        public int FloorNr { get; set; }

        public FloorNumber(ICRUDRepository<Exhibition> repository)
        {
            _repo = repository;
        }

        public IActionResult OnGet(int id)
        {
            FloorNr = id;
            if (id == 2 || id == 3)
            {
                Exhibitions = _repo.FilterListByNumber(_repo.GetAll(), id);
            }
            else
            {
                Exhibitions = _repo.GetAll();
            }

            return Page();
        }
    }
}
