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

                Console.WriteLine("***************************************\n");
                Console.WriteLine("Charging Status: {0}\n", chargingStatus);
                Console.WriteLine("---------------------------------------\n");
                Console.WriteLine("{0}\n", messageToUser);
                Console.WriteLine("***************************************\n");
            }
            catch
            {

            }
        }
    }
}
