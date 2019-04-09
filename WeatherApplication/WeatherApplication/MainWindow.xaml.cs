using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
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

namespace WeatherApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> Favourites { get; set; }
        public ObservableCollection<string> PomocniFavourites { get; set; }
        int brojacTrazenja = 0;
        public string City
        {
            get; set;
        }
        WeatherLoader loadedData;
        LoadTimer timer;

        public void ReadFavourites()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Favourites.txt");

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {

                    sw.Close();

                }

            }
            try
            {
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Favourites.Add(s);
                        PomocniFavourites.Add(s.ToUpper());
                    }
                }
            }
            catch (FileNotFoundException fnf)
            {
                Console.WriteLine(fnf.ToString());
            }
        }
        public void WriteFavorites()
        {

            string currentDirectory = Directory.GetCurrentDirectory();
            string path = System.IO.Path.Combine(currentDirectory, "Favourites.txt");

            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    foreach (var item in Favourites)
                    {
                        sw.WriteLine(item);
                    }

                    sw.Close();
                }
            }
            else
            {
                try
                {
                    using (var sw = new StreamWriter(path, false))
                    {
                        foreach (var item in Favourites)
                        {
                            sw.WriteLine(item);
                        }

                        sw.Close();

                    }
                }
                catch (FileNotFoundException fnf)
                {
                    Console.WriteLine(fnf.ToString());
                }
            }

        }
        public MainWindow()
        {
            Favourites = new ObservableCollection<string>();
            PomocniFavourites = new ObservableCollection<string>();
            ReadFavourites();
            Locator locator = new Locator();
            string errorMessage = "";
            if (locator._lat != -11111 && locator._lon != -11111)
            {
                loadedData = new WeatherLoader(locator._lat, locator._lon);

            }
            else
            {
                Console.WriteLine("Could not find location. Default location is London.");
                errorMessage = "Could not find your location. Please search the city for which you want to see the weather forecast.";
                loadedData = new WeatherLoader("London");
            }
            timer = new LoadTimer(this);
            this.DataContext = this;
            InitializeComponent();
            if (errorMessage != "")
            {
                this.errorMessage.Content = errorMessage;
            }
            FoundCityName.IsReadOnly = true;
            CurrentTemp.IsReadOnly = true;
            Description.IsReadOnly = true;
            Forecast.IsReadOnly = true;
            day2.IsReadOnly = true;
            day22.IsReadOnly = true;
            day3.IsReadOnly = true;
            day33.IsReadOnly = true;
            day4.IsReadOnly = true;
            day44.IsReadOnly = true;
            day5.IsReadOnly = true;
            day55.IsReadOnly = true;
            time1.IsReadOnly = true;
            time2.IsReadOnly = true;
            time3.IsReadOnly = true;
            time4.IsReadOnly = true;
            time5.IsReadOnly = true;
            time6.IsReadOnly = true;
            time7.IsReadOnly = true;
            time8.IsReadOnly = true;
            clock1.IsReadOnly = true;
            clock2.IsReadOnly = true;
            clock3.IsReadOnly = true;
            clock4.IsReadOnly = true;
            clock5.IsReadOnly = true;
            clock6.IsReadOnly = true;
            clock7.IsReadOnly = true;
            clock8.IsReadOnly = true;
            update.IsReadOnly = true;
            dataForDefault();
        }

        private void dataForDefault()
        {
            changeWindowInfo();
        }

        private void fillInLists(List<list> l1, List<list> l2, List<list> l3, List<list> l4, List<list> l5,
            List<list> mainList)
        {
            string datumm = mainList[0].dt_txt.Split(' ')[0];
            int brojac = 1;

            foreach (list l in mainList)
            {

                string datum = l.dt_txt.Split(' ')[0];
                if (datum.Equals(datumm) == true)
                {
                    if (brojac == 1)
                    {
                        l1.Add(l);
                    }
                    else if (brojac == 2)
                    {
                        l2.Add(l);

                    }
                    else if (brojac == 3)
                    {
                        l3.Add(l);
                    }
                    else if (brojac == 4)
                    {
                        l4.Add(l);
                    }
                    else
                    {
                        l5.Add(l);
                    }
                }
                else
                {
                    brojac++;
                    datumm = datum;
                }
            }

            if (l1.Count() != 8)
            {
                int manjak = 8 - l1.Count();
                for (int i = 0; i < manjak; i++)
                {
                    l1.Add(l2.ElementAt(i));
                }
            }
        }

        private void fillInTodayInfo()
        {
            var fullFilePath = $"http://openweathermap.org/img/w/{loadedData.weatherInfo.weather[0].icon}.png";
            Console.WriteLine(loadedData.weatherInfo.weather[0].icon);
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();


            this.WeatherIcon.Source = bitmap;
            this.FoundCityName.Text = loadedData.weatherInfo.name + ", " + loadedData.weatherInfo.sys.country;
            this.CurrentTemp.Text = Math.Round(loadedData.weatherInfo.main.temp, 0).ToString() + "°C";
            this.Description.Text = "Wind: " + loadedData.weatherInfo.wind.speed.ToString() + " m/s\n" +
                                    "Cloudness: " + loadedData.weatherInfo.clouds.all.ToString() + " %\n" +
                                    "Pressure: " + loadedData.weatherInfo.main.pressure.ToString() + " hpa\n" +
                                    "Humidity: " + loadedData.weatherInfo.main.humidity.ToString() + " %\n" +
                                    "Min temperature: " + Math.Round(loadedData.weatherInfo.main.temp_min, 0).ToString() + " °C\n" +
                                    "Max temperature: " + Math.Round(loadedData.weatherInfo.main.temp_max, 0).ToString() + " °C\n";
        }

        private void dataForOtherDays(List<list> listName, TextBox field1, TextBox field2, string nazivSlike, Image poljeSlika)
        {
            double min = 1000;
            double max = -1000;
            foreach (list l in listName)
            {
                if (l.main.temp < min)
                {
                    min = l.main.temp;
                }

                if (l.main.temp > max)
                {
                    max = l.main.temp;
                }
            }

            String datumString = listName[0].dt_txt;
            DateTime dateTime = DateTime.Parse(datumString,
                          System.Globalization.CultureInfo.InvariantCulture);
            int broj = 0;
            try
            {
                broj = Int32.Parse(listName[0].dt_txt.Split(' ')[0].Split('-')[2]);
            }
            catch (FormatException e)
            {
                Console.WriteLine(e.Message);
            }
            field1.Text = /*listName[0].dt_txt.Split(' ')[0];//datum*/dateTime.DayOfWeek.ToString() + " " + broj.ToString();
            field2.Text = Math.Round((min - 273), 0) + "°/" + Math.Round((max - 273), 0) + "°" + "\n" + listName[0].weather[0].description + "\n" + listName[0].main.pressure + " hpa";


            var slika = $"http://openweathermap.org/img/w/{nazivSlike}.png";
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(slika, UriKind.Absolute);
            bitmap.EndInit();
            poljeSlika.Source = bitmap;
        }

        private void addingHourData(List<list> listData)
        {
            int brojacTime = 1;

            var fullFilePath = $"http://openweathermap.org/img/w/{loadedData.weatherInfo.weather[0].icon}.png";
            var fullFilePath0 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[0].weather[0].icon}.png";
            var fullFilePath1 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[1].weather[0].icon}.png";
            var fullFilePath2 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[2].weather[0].icon}.png";
            var fullFilePath3 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[3].weather[0].icon}.png";
            var fullFilePath4 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[4].weather[0].icon}.png";
            var fullFilePath5 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[5].weather[0].icon}.png";
            var fullFilePath6 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[6].weather[0].icon}.png";
            var fullFilePath7 = $"http://openweathermap.org/img/w/{loadedData.weatherForecast.list[7].weather[0].icon}.png";


            foreach (list l in listData)
            {
                if (brojacTime == 1)
                {

                    clock1.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time1.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap0 = new BitmapImage();
                    bitmap0.BeginInit();
                    bitmap0.UriSource = new Uri(fullFilePath0, UriKind.Absolute);
                    bitmap0.EndInit();
                    WeatherIcon_Copy0.Source = bitmap0;
                }
                else if (brojacTime == 2)
                {
                    clock2.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time2.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap1 = new BitmapImage();
                    bitmap1.BeginInit();
                    bitmap1.UriSource = new Uri(fullFilePath1, UriKind.Absolute);
                    bitmap1.EndInit();
                    WeatherIcon_Copy1.Source = bitmap1;
                }
                else if (brojacTime == 3)
                {
                    clock3.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time3.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap2 = new BitmapImage();
                    bitmap2.BeginInit();
                    bitmap2.UriSource = new Uri(fullFilePath2, UriKind.Absolute);
                    bitmap2.EndInit();
                    WeatherIcon_Copy2.Source = bitmap2;
                }
                else if (brojacTime == 4)
                {
                    clock4.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time4.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap3 = new BitmapImage();
                    bitmap3.BeginInit();
                    bitmap3.UriSource = new Uri(fullFilePath3, UriKind.Absolute);
                    bitmap3.EndInit();
                    WeatherIcon_Copy3.Source = bitmap3;
                }
                else if (brojacTime == 5)
                {
                    clock5.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time5.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap4 = new BitmapImage();
                    bitmap4.BeginInit();
                    bitmap4.UriSource = new Uri(fullFilePath4, UriKind.Absolute);
                    bitmap4.EndInit();
                    WeatherIcon_Copy4.Source = bitmap4;
                }
                else if (brojacTime == 6)
                {
                    clock6.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time6.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap5 = new BitmapImage();
                    bitmap5.BeginInit();
                    bitmap5.UriSource = new Uri(fullFilePath5, UriKind.Absolute);
                    bitmap5.EndInit();
                    WeatherIcon_Copy5.Source = bitmap5;
                }
                else if (brojacTime == 7)
                {
                    clock7.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time7.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap6 = new BitmapImage();
                    bitmap6.BeginInit();
                    bitmap6.UriSource = new Uri(fullFilePath6, UriKind.Absolute);
                    bitmap6.EndInit();
                    WeatherIcon_Copy6.Source = bitmap6;
                }
                else if (brojacTime == 8)
                {
                    clock8.Text = l.dt_txt.Split(' ')[1].Split(':')[0] + ":" + l.dt_txt.Split(' ')[1].Split(':')[1];
                    time8.Text = Math.Round((l.main.temp - 273), 0) + "°C\n" + l.main.speed.ToString() + " m/s\n" + l.weather[0].description;
                    BitmapImage bitmap7 = new BitmapImage();
                    bitmap7.BeginInit();
                    bitmap7.UriSource = new Uri(fullFilePath7, UriKind.Absolute);
                    bitmap7.EndInit();
                    WeatherIcon_Copy7.Source = bitmap7;
                }
                brojacTime++;
            }

        }

        private void changeWindowInfo()
        {
            //LEONA
        }

        private void CityButton_Click(object sender, RoutedEventArgs e)
        {
            if (brojacTrazenja != 0)
            {
                checkBox.IsChecked = false;
            }
            if (!loadedData.failed)
            {
                City = this.CityText.Text;
                bool success = loadedData.updateCity(City);
                if (success)
                {
                    this.errorMessage.Content = "";
                    update.Text = "Last updated: " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                    changeWindowInfo();
                }
                else
                {
                    this.errorMessage.Content = "City is not found. Please check the spelling and internet connection.";
                }
            }
            brojacTrazenja++;
        }
        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            City = this.CityText.Text;
            if (checkBox.IsChecked == true)
            {
                if (loadedData.cityExists(City) == true)
                {
                    if (PomocniFavourites.Contains(City.ToUpper()) == false)
                    {
                        Favourites.Add(City);
                        PomocniFavourites.Add(City.ToUpper());
                        if (Favourites.Count() == 6)
                        {
                            Favourites.RemoveAt(0);
                            PomocniFavourites.RemoveAt(0);
                        }
                        this.errorMessage.Content = "";
                    }
                    else
                    {
                        this.errorMessage.Content = "City is already added to favourites.";
                    }
                }
                else
                {
                    this.errorMessage.Content = "City couldn't be added to favourites. Please check the spelling and internet connection.";
                }

            }
            else
            {
                this.errorMessage.Content = "";
            }
            WriteFavorites();
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            String cbi = (String)(sender as ComboBox).SelectedItem;
            City = cbi;
            bool success = loadedData.updateCity(City);
            if (success)
            {
                update.Text = "Last updated: " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                this.errorMessage.Content = "";
                this.CityText.Text = City;
                changeWindowInfo();
            }
        }

        public void automaticUpdate()
        {
            bool updated = loadedData.update();
            if (updated)
            {
                this.Dispatcher.Invoke(() =>
                {
                    update.Text = "Last updated: " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                    this.errorMessage.Content = "";
                    changeWindowInfo();
                });
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            loadedData.updateCity(City);
            bool updated = loadedData.update();
            if (updated)
            {
                update.Text = "Last updated: " + string.Format("{0:HH:mm:ss}", DateTime.Now);
                this.errorMessage.Content = "";
                Console.WriteLine("Application was just updated.");
                changeWindowInfo();
            }
        }

        private void clock1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
