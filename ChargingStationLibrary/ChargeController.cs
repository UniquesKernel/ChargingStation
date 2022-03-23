using UsbSimulator;


namespace ChargingStationLibrary
{

    public class ChargeController : IChargeController
    {
        private IUsbCharger _usbCharger;
        private IDisplay _display;

        private const int TimeTickerInterval = 250;

        public bool IsConnected { get; private set; } = false;

        public ChargeController(IUsbCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            _usbCharger.CurrentValueEvent += OnCurrentChanged;
            
        }

        public event EventHandler<ChargerConnectEvent> ConnectionStatusEvent;


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
            }

        }

        public void StopCharge()
        {
            _usbCharger.StopCharge();
        }


        public void OnCurrentChanged(object sender, CurrentEventArgs e)
        {
            if (e.Current > 500)
            {
                StopCharge();
                _display.DisplayContent("overcharge");
            } 
            else if (e.Current <= 500 && e.Current > 5)
            {
                _display.DisplayContent("charging");
            }
            else if (e.Current <= 5 && e.Current > 0)
            {
                StopCharge();
                _display.DisplayContent("charging complete");
            }
            else
            {
                _display.DisplayContent("");
            }

        }

    }


}


