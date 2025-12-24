using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MortgageController : ControllerBase
    {
        private static MortgageService _service = new();

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpPost]
        public IActionResult Post([FromBody] MortgageRequest request)
        {
            if (request.DownPayment >= request.PropertyPrice)
                return BadRequest("Первоначальный взнос больше стоимости недвижимости");

            var result = _service.Calculate(request);
            return Ok(result);
        }
    }
}