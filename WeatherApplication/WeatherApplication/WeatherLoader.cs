using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WeatherApplication
{
    class WeatherLoader
    {
        private string APPID = "7d846cb23fa7771b84080d0e80d9a1f7";
        private string weather_url, forecast_url;
        public WeatherForcast weatherForecast { get; set; }
        public WeatherInfo.Root weatherInfo { get; set; }
        public bool failed { get; set; }

        public WeatherLoader(string city)
        {
            failed = false;
            try
            {
                weather_url =
                    string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&cnt=6",
                        city, APPID);
                forecast_url = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&APPID={1}", city,
                    APPID);
                weatherForecast = new WeatherForcast();
                weatherInfo = new WeatherInfo.Root();
                loadCurrenttWeather();
                loadForecast();

            }
            catch (Exception e)
            {
                failed = true;
            }
        }

        public WeatherLoader(double lat, double lon)
        {
            failed = false;
            weather_url = string.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric&cnt=6", lat, lon, APPID);
            forecast_url = string.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&APPID={2}", lat, lon, APPID);
            weatherForecast = new WeatherForcast();
            weatherInfo = new WeatherInfo.Root();
            try
            {
                loadCurrenttWeather();
                loadForecast();
            }
            catch (Exception e)
            {
                failed = true;
            }

        }

        public void loadCurrenttWeather()
        {
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(weather_url);
                var jsonObj = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);
                weatherInfo = jsonObj;
            }
        }

        public void loadForecast()
        {
            using (WebClient web = new WebClient())
            {
                var json = web.DownloadString(forecast_url);
                var jsonObj = JsonConvert.DeserializeObject<WeatherForcast>(json);
                weatherForecast = jsonObj;
            }
        }

        public bool updateCity(string city)
        {
            string oldWeatherUrl = weather_url;
            string oldForecastUrl = forecast_url;
            weather_url = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&cnt=6", city, APPID);
            forecast_url = string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&APPID={1}", city, APPID);

            bool success = update();
            if (!success)
            {
                weather_url = oldWeatherUrl;
                forecast_url = oldForecastUrl;
            }
            return success;
        }

        public bool updateCoordinates(double lat, double lon)
        {
            string oldWeatherUrl = weather_url;
            string oldForecastUrl = forecast_url;
            weather_url = string.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric&cnt=6", lat, lon, APPID);
            forecast_url = string.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&APPID={2}", lat, lon, APPID);

            bool success = update();
            if (!success)
            {
                weather_url = oldWeatherUrl;
                forecast_url = oldForecastUrl;
            }
            return success;
        }

        public bool update()
        {
            try
            {
                loadCurrenttWeather();
                loadForecast();
                failed = false;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool cityExists(string city)
        {
            string tryCity = string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&cnt=6", city, APPID);
            bool success = true;
            try
            {
                using (WebClient web = new WebClient())
                {
                    var json = web.DownloadString(tryCity);
                    var jsonObj = JsonConvert.DeserializeObject<WeatherInfo.Root>(json);
                }
            }
            catch (Exception e)
            {
                success = false;
            }
            return success;
        }
    }
}
