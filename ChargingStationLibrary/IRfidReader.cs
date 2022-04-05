namespace ChargingStationLibrary
{
    public class RfidEventArgs : EventArgs
    {
        public int Rfid { get; set; }
    }
    
    public interface IRfidReader
    {
        event EventHandler<RfidEventArgs> RfidDetected;
    }
}
