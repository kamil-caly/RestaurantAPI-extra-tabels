using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IWeatherForecastService service;
        
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Get([FromQuery] int result, 
            [FromBody]TemperatureInfo info)
        {
            if (result < 1 || info.MaxValue <= info.MinValue)
            {
                return BadRequest();
            }

            return Ok(service.Get(result, info.MinValue, info.MaxValue));
        }

        [HttpPost]
        public ActionResult<string> Hello([FromBody] string name)
        {
            HttpContext.Response.StatusCode = 401;
            return StatusCode(401, $"hello {name}");

            //return NotFound(name);
        }

    }
}