using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json; 

namespace YourApp.Services
{
    public class WeatherService
    {
        private static readonly HttpClient client = new HttpClient();
        private const string apiKey = "96e868353549c8ccc62eee780c732449"; 

        // Fetch weather for a city in Fahrenheit
        public static async Task<WeatherInfo> GetWeatherAsync(string city)
        {
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=imperial&appid={apiKey}";

            try
            {
                var response = await client.GetStringAsync(url);
                var data = JsonConvert.DeserializeObject<WeatherApiResponse>(response);

                return new WeatherInfo
                {
                    City = data.name,
                    Temperature = data.main.temp,
                    Condition = data.weather[0].description
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Error fetching weather: " + ex.Message);
                return null;
            }
        }
    }

    // JSON response models
    public class WeatherApiResponse
    {
        public MainData main { get; set; }
        public WeatherData[] weather { get; set; }
        public string name { get; set; }
    }

    public class MainData
    {
        public float temp { get; set; }
    }

    public class WeatherData
    {
        public string description { get; set; }
    }

    // Simplified class for app use
    public class WeatherInfo
    {
        public string City { get; set; }
        public float Temperature { get; set; }
        public string Condition { get; set; }
    }
}
