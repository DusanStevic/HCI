using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApplication
{
    public class WeatherForcast
    {
        public city city { get; set; }
        public List<list> list { get; set; }
    }

    public class city
    {
        public string name { get; set; }
    }

    public class list
    {
        public main main { get; set; }
        public List<weather> weather { get; set; }
        public string dt_txt { get; set; }
    }

    public class main
    {
        public double temp { get; set; }
        public double humidity { get; set; }
        public double pressure { get; set; }
        public double speed { get; set; }
    }

    public class weather
    {
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }
}
