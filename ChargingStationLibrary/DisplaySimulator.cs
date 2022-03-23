#nullable disable
using System;

namespace ChargingStationLibrary
{
    public class DisplaySimulator :IDisplay
    {
        private string _frameBuffer;
        public string FrameBuffer { get; set; }

        public  DisplaySimulator()
        {
            _frameBuffer = null;
        }

        public void DisplayContent(string inputText)
        {
            _frameBuffer = inputText;
            Console.WriteLine(inputText);
        }
    }
}
