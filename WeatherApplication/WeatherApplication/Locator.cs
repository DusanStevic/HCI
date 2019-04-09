using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace WeatherApplication
{
    class Locator
    {
        public double _lat { get; set; }
        public double _lon { get; set; }

        public Locator()
        {
            Console.WriteLine("Locating...");
            _lat = -11111;
            _lon = -11111;
            locate();
        }

        public void locate()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();
            GeoCoordinate coord = new GeoCoordinate();
            long iter = 0;
            do
            {
                watcher.TryStart(false, TimeSpan.FromMilliseconds(2000));
                coord = watcher.Position.Location;
                iter++;
            } while (coord.IsUnknown && iter < 9999999);

            if (iter < 9999999)
            {
                _lat = coord.Latitude;
                _lon = coord.Longitude;
            }
        }
    }
}
