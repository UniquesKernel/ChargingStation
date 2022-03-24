#nullable disable
using System;
using System.Timers;

namespace ChargingStationLibrary
{
    public class DisplaySimulator : IDisplay
    {
        public string welcomeMessage = "Welcome to Charger Station!";

        public string messageToUser;

        public string chargingStatus;

        public DisplaySimulator()
        {
            messageToUser = welcomeMessage;
            chargingStatus = "";
            UpdateDisplay();    

        }

        public void DisplayChargerStatus(string inputText)
        {
            chargingStatus = inputText;
            UpdateDisplay();
        }

        public void DisplayMessage(string inputText)
        {
            messageToUser = inputText;
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            try
            {
                Console.Clear();

                Console.WriteLine("***************************************");
                Console.WriteLine("Charging Status: {1}", chargingStatus);
                Console.WriteLine("---------------------------------------");
                Console.WriteLine("{1}", messageToUser);
                Console.WriteLine("***************************************");
            }
            catch
            {

            }
        }
    }
}
