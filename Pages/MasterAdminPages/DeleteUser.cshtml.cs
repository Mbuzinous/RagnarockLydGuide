using Kojg_Ragnarock_Guide.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Enums;
using RagnarockTourGuide.Interfaces;
using RagnarockTourGuide.Models;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class DeleteUserModel : PageModel
    {
        private readonly IUserCRUDRepository<User> _repository;

        [BindProperty]
        public User UserToDelete { get; set; }
        public string ErrorMessage { get; private set; } // Property to store error message

        private IUserValidator _userValidator;

        public DeleteUserModel(IUserCRUDRepository<User> repository, IUserValidator userValidator)
        {
            _repository = repository;
            _userValidator = userValidator;
        }

        public IActionResult OnGet(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _userValidator.GetUserRole(HttpContext.Session);
            
            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            UserToDelete = _repository.GetById(id);
            if (UserToDelete == null)
            {
                return RedirectToPage("DisplayUsers");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                // Retrieve roles for current and target users
                Role currentUserRole = _userValidator.GetUserRole(HttpContext.Session);
                Role targetUserRole = UserToDelete.Role;

                // Call the Delete method and pass roles and target ID
                _repository.Delete((int)currentUserRole, (int)targetUserRole, UserToDelete.Id);

                // Redirect on successful deletion
                return RedirectToPage("DisplayUsers");
            }
            catch (SqlException sqlEx)
            {
                // Specific SQL error handling for role-based authorization errors
                if (sqlEx.Number == 50003) // Example error code for "Cannot delete user with the same or higher role"
                {
                    ErrorMessage = "Unauthorized: Cannot delete a user with the same or higher role.";
                }
                else
                {
                    ErrorMessage = $"SQL Error: {sqlEx.Message}";
                }
                return Page(); // Return to the page to display the error message
            }
            catch (Exception ex)
            {
                // General exception handling
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                return Page();
            }
        }
    }
}
