using Kojg_Ragnarock_Guide.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Client;
using RagnarockTourGuide.Interfaces.CRUDFactoryInterfaces;
using RagnarockTourGuide.Interfaces.FactoryInterfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.Admin
{
    public class MockCreateModel : PageModel
    {
        private readonly ICreateRepository<Exhibition> _create;

        public Exhibition exhibition { get; set; } 
        public User user { get; set; }
        public MockCreateModel(ICRUDFactory<Exhibition> factory)
        {
             _create = factory.CreateRepository();
        }
        public void OnGet()
        {
            _create.Create(user);
            _create.Create(exhibition);
        }
    }
}
