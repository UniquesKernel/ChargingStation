namespace ChargingStationLibrary
{

    public interface IDisplay
    {
        public void DisplayChargerStatus(string inputText);
        public void DisplayMessage(string inputText);
        public void UpdateDisplay();
    }
}
