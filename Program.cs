using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. ПОДКЛЮЧЕНИЕ БАЗЫ ДАННЫХ SQLite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=auth.db"));

// Добавляем Razor Pages и контроллеры
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Cookie-auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";      // путь к странице входа
        options.LogoutPath = "/Account/Logout";    // путь к странице выхода
        options.AccessDeniedPath = "/Account/Login"; // при запрещенном доступе
    });


//  РЕГИСТРАЦИЯ ПОЛЬЗОВАТЕЛЬСКИХ СЕРВИСОВ ДЛЯ ВНЕШНЕГО И ВНУТРЕННЕГО API
builder.Services.AddAuthorization();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MortgageService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddHttpClient();

// --- Сессия ---
builder.Services.AddDistributedMemoryCache(); // память для сессий
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // время жизни сессии
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Middleware сессии
app.UseSession();

// Middleware аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();

app.Run();
