using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using WebApplication1.Services;

namespace WebApplication1.Pages.Account
{
    public class DeleteAccountModel : PageModel
    {
        private readonly AuthService _auth;

        public DeleteAccountModel(AuthService auth)
        {
            _auth = auth;
        }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Account/Login");
            }

            // Проверяем пароль
            var user = await _auth.LoginAsync(username, Password);
            if (user == null)
            {
                Message = "Неверный пароль";
                return Page();
            }

            // Удаляем пользователя
            var result = await _auth.DeleteUserAsync(username);
            if (result)
            {
                // Выходим из системы
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                HttpContext.Session.Clear();

                Message = "Аккаунт успешно удалён. Вы будете перенаправлены на страницу входа.";
                await Task.Delay(2000); // Ждём 2 секунды
                return RedirectToPage("/Account/Login");
            }

            Message = "Ошибка при удалении аккаунта";
            return Page();
        }
    }
}