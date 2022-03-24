#nullable disable
using System;

namespace ChargingStationLibrary
{
    public class RfidReaderSimulator : IRfidReader
    {

        public RfidReaderSimulator() { }

        public int _newRfid { get; private set; }

        public event EventHandler<RfidEventArgs> RfidDetected;

        public void SimulateNewRfidDetected(int rfid)
        {
          _newRfid = rfid;
          OnRfidDetected();
        }

        private void OnRfidDetected()
        {
            RfidDetected.Invoke(this, new RfidEventArgs() { Rfid = this._newRfid });
        }
    }
}
