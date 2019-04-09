﻿using System;
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

                }
            }

        }

       

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
            
            

        }

      

      

      
    }
}
