using Microsoft.AspNetCore.Mvc;

namespace ApiFaceUnah.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Allan", "Alcides", "Paty", "Cool", "Mild", "Warm", "Balmy", "Juan", "Sweltering", "Scorching"
        ];

        [HttpGet(Name = "GetWeateherForecast")]
        public IActionResult Get()
        {
            var lista = new List<WeatherForecast>();

            foreach (var index in Enumerable.Range(1, 10))
            {
                if (index > 10)
                {
                    return BadRequest(new
                    {
                        Message = "El Id generado no puede ser mayor que 10",
                        IdGenerado = index
                    });
                }

                lista.Add(new WeatherForecast
                {
                    Id = index,
                    Summary = Summaries[(index - 1) % Summaries.Length],

                });

            }

            return Ok(new
            {
                Message = "Datos generados correctamente",
                Total = lista.Count,
                Data = lista
            });
        }
    }
}