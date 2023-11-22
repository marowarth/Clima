using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Clima
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ApiKey = "4fa94c2c2fcc01492e0f7d8fded4b8a0";
        private const string ApiBaseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GetWeatherButton_Click(object sender, RoutedEventArgs e)
        {
            string city =  CityTextBox.Text;
            string apiUrl = string.Format(ApiBaseUrl, city, ApiKey);

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(jsonResponse);
                    DisplayWeatherInfo(weatherData);
                }
                else
                {
                    MessageBox.Show("Error al obtener datos del clima. Asegúrate de ingresar un nombre de ciudad válido.");
                }
            }
        }

        private void DisplayWeatherInfo(WeatherData weatherData)
        {
            TemperatureLabel.Content = $"Temperatura: {weatherData.Main.Temp}°C";
            HumidityLabel.Content = $"Humedad: {weatherData.Main.Humidity}%";
            DescriptionLabel.Content = $"Descripción: {weatherData.Weather[0].Description}";

            string weatherIcon = GetWeatherIcon(weatherData.Weather[0].id);
            WeatherIconImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri($"Images\\{weatherIcon}.png", UriKind.Relative));
        }

        private string GetWeatherIcon(int weatherId)
        {
            if (weatherId ==800)
            {
                return "sun";
            }
            else if (weatherId > 800 && weatherId < 805)
            {
                return "cloud";
            }
            else if (weatherId > 299 && weatherId < 600)
            {
                return "rain";
            }
            else if (weatherId>199 && weatherId<300)
            {
                return "storm";
            }
            else
            {
                return "unknown"; // Puedes tener un icono predeterminado para otros tipos de clima
            }
        }

        private class WeatherData
        {
            public MainData Main { get; set; }
            public WeatherInfo[] Weather { get; set; }
        }

        private class MainData
        {
            public double Temp { get; set; }
            public int Humidity { get; set; }
        }

        private class WeatherInfo
        {
            public string Description { get; set; }
            public int id { get; set; }
        }
    }
}
