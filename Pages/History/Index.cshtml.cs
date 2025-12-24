using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Pages.History
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ReportService _reportService;

        public List<MortgageReport> Reports { get; set; } = new();
        public string Message { get; set; }

        public IndexModel(ReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task OnGetAsync()
        {
            Reports = await _reportService.GetUserReportsAsync(User);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var result = await _reportService.DeleteReportAsync(id, User);

            if (result)
            {
                Message = "Отчёт успешно удалён";
            }
            else
            {
                Message = "Ошибка при удалении отчёта";
            }

            Reports = await _reportService.GetUserReportsAsync(User);
            return Page();
        }
    }
}