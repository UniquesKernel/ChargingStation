using UsbSimulator;
using System.Timers;


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
                
            }

        }

    }

    public class usbMock : IUsbCharger
    {
        // Constants
        private const double FullyChargedCurrent = 2.5; // mA
        private const double OverloadCurrent = 750; // mA
        private const int CurrentTickInterval = 250; // ms

        public event EventHandler<CurrentEventArgs> CurrentValueEvent;

        public double CurrentValue { get; set; }

        public bool Connected { get; private set; }

        private bool _overload;
        private bool _charging;
        private System.Timers.Timer _timer;
        private int _ticksSinceStart;

        public usbMock()
        {
            CurrentValue = 0.0;
            Connected = true;
            _overload = false;

            _timer = new System.Timers.Timer();
            _timer.Enabled = false;
            _timer.Interval = CurrentTickInterval;
            _timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            // Only execute if charging
            if (_charging)
            {
                OnNewCurrent();
            }
        }

        // Start charging
        public void StartCharge()
        {
            _charging = true;
            CurrentValue = 400;
            _timer.Start();
        }

        // Stop charging
        public void StopCharge()
        {
            _charging = false;
            _timer.Stop();
        }

        private void OnNewCurrent()
        {
            CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = this.CurrentValue });
        }

        public void setOvercharge()
        {
            CurrentValue = OverloadCurrent;
        }

        public void setChargeDone()
        {
            CurrentValue =  FullyChargedCurrent;
        }

    }

    public class displayMock : IDisplay
    {
        public string status { get; private set; }
        public void DisplayContent(string inputText)
        {
            status = inputText;
        }
    }

}


