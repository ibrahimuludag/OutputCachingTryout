using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace OutputCachingTryout.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] Cities = new[]
        {
            "London", "Paris", "Amsterdam", "Utrect"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast Default")]
        [OutputCache]
        public IEnumerable<WeatherForecast> GetDefault()
        {
            _logger.LogInformation($"{nameof(GetDefault)} is called");
            
            return GetForecast();
        }

        [HttpGet("get-bypolicy", Name = "GetWeatherForecast Policy")]
        [OutputCache(PolicyName = "Expire20")]
        public IEnumerable<WeatherForecast> GetPolicy()
        {
            _logger.LogInformation($"{nameof(GetPolicy)} is called");
            
            return GetForecast();
        }

        [HttpGet("{city}", Name = "GetWeatherForecast Query")]
        [OutputCache(VaryByQueryKeys = new[] { "city" })]
        public IEnumerable<WeatherForecast> GetByQuery(string city)
        {
            _logger.LogInformation($"{nameof(GetByQuery)} is called");

            return GetForecast().Where(c => c.City.Equals(city , StringComparison.OrdinalIgnoreCase));
        }

        [HttpGet("nocache", Name = "GetWeatherForecast NoCache")]
        [OutputCache(PolicyName = "NoCache")]
        public IEnumerable<WeatherForecast> NoCache()
        {
            _logger.LogInformation($"{nameof(NoCache)} is called");

            return GetForecast();
        }
        [HttpGet("nolock", Name = "GetWeatherForecast NoCache")]
        [OutputCache(PolicyName = "NoCache")]
        public IEnumerable<WeatherForecast> NoLock()
        {
            _logger.LogInformation($"{nameof(NoLock)} is called");

            return GetForecast();
        }

        private static IEnumerable<WeatherForecast> GetForecast (){
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)],
                City = Cities[Random.Shared.Next(Cities.Length)]
            })
            .ToArray();
        }
    }
}