using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly AuthService _auth;

        public RegisterModel(AuthService auth)
        {
            _auth = auth;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "¬ведите им€ пользовател€ и пароль";
                return Page();
            }

            var user = await _auth.RegisterAsync(Username, Password);

            if (user != null)
            {
                Message = $"ѕользователь {user.Username} успешно зарегистрирован!";
                return RedirectToPage("/Account/Login");
            }
            else
            {
                Message = "ѕользователь с таким именем уже существует";
                return Page();
            }
        }
    }
}