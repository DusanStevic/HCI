using System;
using System.Collections.Generic;
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
        WeatherLoader loadedData;
        public MainWindow()
        {
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
            InitializeComponent();
        }
    }
}
