#nullable disable
using System;
using System.Timers;

namespace ChargingStationLibrary
{
    public class DisplaySimulator :IDisplay
    {
        private string _frameBuffer;
        public string FrameBuffer { get; set; }

        private const int TimeTickerInterval = 500;

        private string _welcomeMessage = "Welcome to Charger Station!";
        public string WelcomeMessage { get; }


        public DisplaySimulator()
        {
            _timer = new System.Timers.Timer();
            _timer.Enabled = true;
            _timer.Interval = TimeTickerInterval;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            _frameBuffer = _welcomeMessage;
            Console.WriteLine(_frameBuffer);
        }

        private System.Timers.Timer _timer;

        public void DisplayContent(string inputText)
        {
            _frameBuffer = inputText; 
        }

        private void TimerOnElapsed(object? sender, ElapsedEventArgs e)
        {
            Console.Clear();
            Console.WriteLine(_frameBuffer);
        }
    }
}
