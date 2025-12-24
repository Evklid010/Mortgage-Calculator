using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Security.Claims;

namespace WebApplication1.Services
{
    public class ReportService
    {
        private readonly AppDbContext _db;

        public ReportService(AppDbContext db)
        {
            _db = db;
        }

        // Сохранить новый отчёт
        public async Task<MortgageReport> SaveReportAsync(
            ClaimsPrincipal user,
            MortgageRequest request,
            MortgageResult result)
        {
            var username = user.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return null;

            var dbUser = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (dbUser == null)
                return null;

            var report = new MortgageReport
            {
                UserId = dbUser.Id,
                PropertyPrice = request.PropertyPrice,
                DownPayment = request.DownPayment,
                InterestRate = request.InterestRate,
                Years = request.Years,
                PaymentType = request.PaymentType,
                MonthlyPayment = result.MonthlyPayment,
                TotalPayment = result.TotalPayment,
                Overpayment = result.Overpayment
            };

            _db.MortgageReports.Add(report);
            await _db.SaveChangesAsync();
            return report;
        }

        // Получить все отчёты пользователя
        public async Task<List<MortgageReport>> GetUserReportsAsync(ClaimsPrincipal user)
        {
            var username = user.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return new List<MortgageReport>();

            return await _db.MortgageReports
                .Include(r => r.User)
                .Where(r => r.User.Username == username)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        // Удалить отчёт
        public async Task<bool> DeleteReportAsync(Guid reportId, ClaimsPrincipal user)
        {
            var username = user.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return false;

            var report = await _db.MortgageReports
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == reportId && r.User.Username == username);

            if (report == null)
                return false;

            _db.MortgageReports.Remove(report);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}