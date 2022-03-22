using UsbSimulator;
using System.Timers;


namespace ChargingStationLibrary
{

    public class ChargeController : IChargeController
    {
        private UsbChargerSimulator _usbCharger;
        //private IDisplay _display;

        private const int TimeTickerInterval = 250;

        public bool IsConnected { get; private set; } = false;

        public ChargeController(UsbChargerSimulator usbCharger)//, IDisplay display)
        {
            _usbCharger = usbCharger;
            //_display = display;


            //set timer
            _timer = new System.Timers.Timer();
            _timer.Enabled = false;
            _timer.Interval = TimeTickerInterval;
            _timer.Elapsed += TimerOnElapsed;

        }

        
        private System.Timers.Timer _timer;


        public event EventHandler<ChargerConnectEvent>? ConnectionStatusEvent;

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            OnConnectionChanged();
        }

        public void SimulateConnect()
        {
            if (!IsConnected)
            {
                IsConnected = true;
            }
        }

        public void SimulateDisconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
            }
        }

        public void Connect()
        {
            SimulateConnect();
            OnConnectionChanged();
            _timer.Start();
        }

        public void Disconnect()
        {
            SimulateDisconnect();
            OnConnectionChanged();
            _timer.Start();
        }

        public void StartCharge()
        {
            _usbCharger.StartCharge();
        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
        }

        public void ChargingMessages()
        {
            throw new NotImplementedException();
        }

        private void OnConnectionChanged()
        {
            ConnectionStatusEvent?.Invoke(this, new ChargerConnectEvent() { ChargerIsConnected = IsConnected });
        }

    }


}


