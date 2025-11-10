using System.Net.Http;
using System.Text.Json;
using Microsoft.Maui.Controls;

namespace SeamlessBackEndProject
{
    public partial class MainPage : ContentPage
    {
        private const string ApiKey = "96e868353549c8ccc62eee780c732449"; 
        private readonly HttpClient _httpClient = new();

        public MainPage()
        {
            InitializeComponent();
            // Do not re-instantiate CityEntry or WeatherLabel here; they are set by XAML
        }

        private async void OnGetWeatherClicked(object sender, EventArgs e)
        {
            string city = CityEntry.Text?.Trim();

            if (string.IsNullOrEmpty(city))
            {
                await DisplayAlert("Error", "Please enter a city name.", "OK");
                return;
            }

            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={ApiKey}&units=metric";

            try
            {
                WeatherLabel.Text = "Fetching weather...";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);

                var main = doc.RootElement.GetProperty("main");
                double temp = main.GetProperty("temp").GetDouble();
                double tempF = (temp * 9 / 5) + 32;
                string condition = doc.RootElement.GetProperty("weather")[0].GetProperty("description").GetString();

                WeatherLabel.Text = $"🌡 Temperature: {tempF:F1}°F\n☁️ Condition: {condition}";
            }
            catch (Exception ex)
            {
                WeatherLabel.Text = $"Error: {ex.Message}";
            }
        }
    }
}
