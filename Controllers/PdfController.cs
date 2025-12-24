using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PdfController : ControllerBase
    {
        private readonly PdfService _pdfService;
        private readonly ReportService _reportService;

        public PdfController(PdfService pdfService, ReportService reportService)
        {
            _pdfService = pdfService;
            _reportService = reportService;
        }

        // GET: api/pdf/generate/{id}
        [HttpGet("generate/{id}")]
        public async Task<IActionResult> GeneratePdf(Guid id)
        {
            // Получаем отчёт из БД
            var reports = await _reportService.GetUserReportsAsync(User);
            var report = reports.FirstOrDefault(r => r.Id == id);

            if (report == null)
            {
                return NotFound("Отчёт не найден или у вас нет доступа");
            }

            try
            {
                // Генерируем PDF через внешний API
                var pdfBytes = await _pdfService.GenerateMortgagePdfAsync(
                    username: User.Identity?.Name ?? "Пользователь",
                    propertyPrice: report.PropertyPrice,
                    downPayment: report.DownPayment,
                    interestRate: report.InterestRate,
                    years: report.Years,
                    paymentType: report.PaymentType,
                    monthlyPayment: report.MonthlyPayment,
                    totalPayment: report.TotalPayment,
                    overpayment: report.Overpayment
                );

                // Возвращаем PDF как файл для скачивания
                return File(pdfBytes, "application/pdf",
                    $"ипотечный_расчет_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка генерации PDF: {ex.Message}");
            }
        }

        // GET: api/pdf/test
        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<IActionResult> TestPdf()
        {
            try
            {
                // Тестовый PDF без данных пользователя
                var pdfBytes = await _pdfService.GenerateMortgagePdfAsync(
                    username: "Тестовый пользователь",
                    propertyPrice: 5_000_000,
                    downPayment: 1_000_000,
                    interestRate: 7.5,
                    years: 20,
                    paymentType: "annuity",
                    monthlyPayment: 35_000,
                    totalPayment: 8_400_000,
                    overpayment: 3_400_000
                );

                return File(pdfBytes, "application/pdf", "test.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Тестовая ошибка: {ex.Message}");
            }
        }
    }
}