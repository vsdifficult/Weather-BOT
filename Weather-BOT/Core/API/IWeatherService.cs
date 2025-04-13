namespace Weather.Core.API 
{ 
    public interface IWeatherService
    { 
        Task<string> GetWeather(string city); 
    }
}