#nullable disable
using UsbSimulator;
using System.Timers;


namespace ChargingStationLibrary
{

    public class ChargeController : IChargeController
    {
        private IUsbCharger _usbCharger;
        private IDisplay _display;

        private const int TimeTickerInterval = 250;
        private System.Timers.Timer _timer;

        public bool IsConnected { get; private set; } = false;
        public bool OldConnectStatus { get; private set; }

        public event EventHandler<ChargerConnectEvent> ConnectionStatusEvent;

        public ChargeController(IUsbCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            _usbCharger.CurrentValueEvent += OnCurrentChanged;

            _timer = new System.Timers.Timer();
            _timer.Enabled = false;
            _timer.Interval = TimeTickerInterval;
            _timer.Elapsed += TimerOnElapsed;

            _timer.Start();

        }

        
        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            OnConnectionChange();
        }

        public void Connect()
        {
            if (!IsConnected)
            {
                IsConnected = true;
            }
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
            }
        }

        public void StartCharge()
        {
            if (IsConnected)
            {
                _usbCharger.StartCharge();
                _timer.Stop(); //no need to check if phone is connected if door is locked, and phone is charging
                _display.DisplayChargerStatus("Charging");
            }

        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
            _timer.Start(); //start checking if phone is removed
            _display.DisplayChargerStatus("Charging Stopped");
        }


        private void OnCurrentChanged(object sender, CurrentEventArgs e)
        {
          
            if (e.Current > 500)
            {
                StopCharge();
                _display.DisplayChargerStatus("overcharge");
            }
            else if (e.Current <= 500 && e.Current > 5)
            {
                
            }
            else if (e.Current <= 5 && e.Current > 0)
            {
                StopCharge();
                _display.DisplayChargerStatus("charging complete");
            }
            else
            {
                _display.DisplayChargerStatus("");
            }
          
        }

        private void OnConnectionChange()
        {
            ConnectionStatusEvent?.Invoke(this, new ChargerConnectEvent() { ChargerIsConnected = this.IsConnected });
        }

    }


}


