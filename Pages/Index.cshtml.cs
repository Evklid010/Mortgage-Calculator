using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly MortgageService _mortgageService;
        private readonly ReportService _reportService;

        [BindProperty]
        public MortgageRequest Input { get; set; } = new();

        public MortgageResult? Result { get; set; }
        public string Message { get; set; }

        public IndexModel(MortgageService mortgageService, ReportService reportService)
        {
            _mortgageService = mortgageService;
            _reportService = reportService;
        }

        public async Task OnPostAsync()
        {
            if (!ModelState.IsValid) return;

            var calc = _mortgageService.Calculate(Input);
            Result = calc.Result;

            // Сохраняем отчёт в БД
            var report = await _reportService.SaveReportAsync(User, Input, Result);
            if (report != null)
            {
                Message = "Отчёт сохранён в истории!";
            }
        }
    }
}