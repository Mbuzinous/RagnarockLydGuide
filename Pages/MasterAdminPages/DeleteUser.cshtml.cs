using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using RagnarockTourGuide.Models;
using RagnarockTourGuide.Models.Enums;

namespace RagnarockTourGuide.Pages.MasterAdminPages
{
    public class DeleteUserModel : PageModel
    {
        [BindProperty]
        public User UserToDelete { get; set; }
        public string ErrorMessage { get; private set; } // Property to store error message

        private BackendController<User> _backendController;

        public DeleteUserModel(BackendController<User> backendController)
        {
            _backendController = backendController;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Hent brugerens rolle fra sessionen
            Role userRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);
            
            // Tjek om brugeren har adgang baseret på rollen
            if (userRole != Role.MasterAdmin)
            {
                // Omdirigér til forsiden, hvis brugeren ikke har den nødvendige rolle
                return RedirectToPage("/Index");
            }

            UserToDelete = await _backendController.ReadRepository.GetByIdAsync(id);
            if (UserToDelete == null)
            {
                return RedirectToPage("DisplayUsers");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                // Retrieve roles for current and target users
                Role currentUserRole = _backendController.UserValidator.GetUserRole(HttpContext.Session);
                Role targetUserRole = UserToDelete.Role;
                DeleteParameter parameter = new DeleteParameter()
                {
                    Id = UserToDelete.Id,
                    CurrentUserRole = (int)currentUserRole,
                    TargetUserRole = (int)targetUserRole
                };

                // Call the Delete method and pass roles and target ID
                await _backendController.DeleteRepository.DeleteAsync(parameter);

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
