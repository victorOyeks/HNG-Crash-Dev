using API.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using API.Dto;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Services
{
    public class GreetingsService : IGreetingsRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        public GreetingsService(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<GreetingDto> GreetAsync(string name)
        {
            var ip = await GetIp();
            var userLocation =  await GetUserLocation(ip);
            var userWeather = await GetUserWeather(userLocation);

            return new GreetingDto
            {
                Client_ip = ip,
                Location = userLocation,
                Greeting = $"Hello, {name}!, the temperature is {userWeather} degrees Celcius in {userLocation}"
            };
        }

        private Task<string> GetIp()
        {
            var context = _httpContextAccessor.HttpContext;
            string ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(ip))
            {
                ip = ip.Split(',').First().Trim();
            }
            else
            {
                ip = context.Connection.RemoteIpAddress.ToString();
                if (ip == "::1")
                {
                    ip = "127.0.0.1";
                }
            }

            // Convert IPv6-mapped IPv4 address to regular IPv4
            if (ip.StartsWith("::ffff:"))
            {
                ip = ip.Substring(7);
            }

            return Task.FromResult(ip);
        }

        private async Task<string> GetUserLocation(string ip)
        {
            try
            {
                var response = await _httpClient.GetStringAsync($"http://ip-api.com/json/{ip}");
                var locationData = JsonConvert.DeserializeObject<LocationData>(response);

                return locationData.city;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error fetching user location: {ex.Message}");
                return "Unknown";
            }
        }

        private async Task<string> GetUserWeather(string location)
        {
            var response = await _httpClient.GetStringAsync($"https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/{location}?key=PJT7TQMBACYBW9EECVMAJYDWL");

            var jsonResponse = JObject.Parse(response);

            var temperatureFahrenheit = (double)jsonResponse["days"][0]["temp"]; 

            var temperatureCelsius = Math.Round((temperatureFahrenheit - 32) * 5 / 9, 2);

            var formattedTemperature = temperatureCelsius.ToString("0.00");

            return formattedTemperature;
        }
    }
}
