using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WeatherApplication
{
    class LoadTimer
    {
        private static Timer timer;
        private MainWindow window;

        public LoadTimer(MainWindow mainWindow)
        {
            timer = new System.Timers.Timer();
            timer.Interval = 600000;
            timer.Elapsed += OnTimerEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            this.window = mainWindow;
        }

        private void OnTimerEvent(Object source, System.Timers.ElapsedEventArgs e)
        {

            this.window.automaticUpdate();

        }
    }
}
