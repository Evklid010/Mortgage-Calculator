using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace WebApplication1.Services
{
    public class PdfService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public PdfService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["PdfCo:ApiKey"]
                ?? throw new Exception("PdfCo API key is missing. Set PdfCo:ApiKey in appsettings.json");
        }

        public async Task<byte[]> GenerateMortgagePdfAsync(
            string username,
            decimal propertyPrice,
            decimal downPayment,
            double interestRate,
            int years,
            string paymentType,
            decimal monthlyPayment,
            decimal totalPayment,
            decimal overpayment)
        {
            string htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Ипотечный расчёт</title>
    <style>
        body {{ font-family: Arial, sans-serif; margin: 40px; }}
        h1 {{ text-align: center; }}
        table {{ width: 100%; border-collapse: collapse; margin-top: 20px; }}
        th, td {{ border: 1px solid #ddd; padding: 10px; }}
        th {{ background-color: #f2f2f2; }}
        .total {{ font-weight: bold; color: #e74c3c; }}
    </style>
</head>
<body>
    <h1>Ипотечный расчёт</h1>
    <p><b>Пользователь:</b> {username}</p>
    <p><b>Дата:</b> {DateTime.Now:dd.MM.yyyy HH:mm}</p>

    <h2>Входные данные</h2>
    <table>
        <tr><th>Стоимость</th><td>{propertyPrice:N0} ₽</td></tr>
        <tr><th>Первоначальный взнос</th><td>{downPayment:N0} ₽</td></tr>
        <tr><th>Ставка</th><td>{interestRate:F2}%</td></tr>
        <tr><th>Срок</th><td>{years} лет</td></tr>
        <tr><th>Тип платежа</th><td>{(paymentType == "annuity" ? "Аннуитетный" : "Дифференцированный")}</td></tr>
    </table>

    <h2>Результаты</h2>
    <table>
        <tr class='total'><th>Ежемесячный платёж</th><td>{monthlyPayment:N0} ₽</td></tr>
        <tr><th>Всего выплачено</th><td>{totalPayment:N0} ₽</td></tr>
        <tr><th>Переплата</th><td>{overpayment:N0} ₽</td></tr>
    </table>

    <p><small>Сгенерировано ипотечным калькулятором</small></p>
</body>
</html>";

            var requestBody = new
            {
                html = htmlContent,
                name = $"mortgage_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                paperSize = "A4",
                orientation = "Portrait",
                margins = "10px"
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                "https://api.pdf.co/v1/pdf/convert/from/html"
            );

            request.Headers.Add("x-api-key", _apiKey);
            request.Content = new StringContent(
                JsonConvert.SerializeObject(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"PDF.co error: {responseContent}");
            }

            dynamic pdfResponse = JsonConvert.DeserializeObject(responseContent);

            if (pdfResponse.error == true)
            {
                throw new Exception(pdfResponse.message.ToString());
            }

            string pdfUrl = pdfResponse.url;

            // Скачиваем готовый PDF
            return await _httpClient.GetByteArrayAsync(pdfUrl);
        }
    }
}