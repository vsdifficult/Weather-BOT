using System.Net.Http;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Weather.Core.API
{ 

    public class WeatherService : IWeatherService
    {
        private readonly string _apiKey = "YOUR_OPENWEATHERMAP_API_KEY";
        public async Task<string> GetWeather(string city)
        {
            using var client = new HttpClient();
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=ru";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return "❌ Город не найден или ошибка на сервере.";
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var temp = root.GetProperty("main").GetProperty("temp").GetDecimal();
            var feels = root.GetProperty("main").GetProperty("feels_like").GetDecimal();
            var weather = root.GetProperty("weather")[0].GetProperty("description").GetString();
            return $"🌤 Погода в {city}:\nТемпература: {temp}°C, ощущается как {feels}°C\n{weather}";
        }
    }
    
}