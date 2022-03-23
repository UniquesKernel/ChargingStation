using UsbSimulator;
using System.Timers;


namespace ChargingStationLibrary
{

    public class ChargeController : IChargeController
    {
        private IUsbCharger _usbCharger;
        //private IDisplay _display;

        private const int TimeTickerInterval = 250;

        public bool IsConnected { get; private set; } = false;

        public ChargeController(IUsbCharger usbCharger)//, IDisplay display)
        {
            _usbCharger = usbCharger;
            _usbCharger.CurrentValueEvent += OnCurrentChanged;
            //_display = display;
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

        public void OnCurrentChanged(object sender, CurrentEventArgs e)
        {
            if(e.Current > 500)
            {
                StopCharge();
                //display.overcharge();
            }else if(e.Current <= 500 && e.Current > 5)
            {
                //display.charging();
            }else if(e.Current <= 5 && e.Current > 0)
            {
                StopCharge();
                //display.chargingComplete
            }
            else
            {
                //display.nothing();
            }

        }

    }

}


