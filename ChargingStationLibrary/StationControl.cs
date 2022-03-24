#nullable disable
namespace ChargingStationLibrary
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum ChargingStatitionState
        {
            Available,
            Locked,
            DoorOpen
        };
        public enum ChargerConnectionState
        {
            Connected,
            Disconnected
        };
        public enum DoorState
        {
            open,
            closed
        }

        // Her mangler flere member variable
        public ChargingStatitionState _stationState;
        public ChargerConnectionState _connectionStatus;
        public DoorState _doorState;
        private IChargeController _charger;
        private int _oldId;
        private IDoor _door;
        private IRfidReader _rfidReader;
        private ILog _log;
        private IDisplay _display;

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        public StationControl(
          IChargeController charger,
          IDoor door,
          IRfidReader rfidReader,
          ILog log,
          IDisplay display
          )
        {
          _charger = charger;
          _door = door;
          _rfidReader = rfidReader;
          _log = log;
          _display = display;
          _stationState = ChargingStatitionState.Available;
          _doorState = DoorState.closed;
          _connectionStatus = ChargerConnectionState.Disconnected;


            _rfidReader.RfidDetected += RfidDetected;
          _door.DoorChanged += OnDoorStatusChange;
          _charger.ConnectionStatusEvent += OnConnectionChange;

        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(object sender, RfidEventArgs e)
        {
          int id = e.Rfid;

            switch (_stationState)
            {
                case ChargingStatitionState.Available:
                    // Check for ladeforbindelse
                    if (_charger.IsConnected)
                    {
                        if (_doorState == DoorState.closed)
                        {
                            _door.Lock();
                            _charger.StartCharge();
                            _oldId = id;
                            _log.Log($": Skab låst med RFID: {id}");

                            _display.DisplayMessage("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                            _stationState = ChargingStatitionState.Locked;
                        }
                        else
                        {
                            _display.DisplayMessage("Luk døren og prøv igen");
                        }
                    }
                    else
                    {
                        _display.DisplayMessage("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case ChargingStatitionState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.Unlock();
                        
                        _log.Log($": Skab låst op med RFID: {id}");
                       
                        _display.DisplayMessage("Tag din telefon ud af skabet og luk døren");
                        _stationState = ChargingStatitionState.Available;
                    }
                    else
                    {
                        _display.DisplayMessage("Forkert RFID tag");
                    }

                    break;
            }
        }

        private void OnDoorStatusChange(object sender, DoorEventArgs e)
        {
          if (_doorState == DoorState.closed && e.DoorIsOpen == true)
          {
            _display.DisplayMessage("Tilslut Telefon");
            _doorState = DoorState.open;
            
          }
          else if (_doorState == DoorState.open && e.DoorIsOpen == false)
          {
            _display.DisplayMessage("Indlæs Rfid");
            _doorState = DoorState.closed;
          }
        }

        private void OnConnectionChange(object sender, ChargerConnectEvent e )
        {
            if(e.ChargerIsConnected == true && _connectionStatus == ChargerConnectionState.Disconnected)
            {
                _display.DisplayMessage("Charger Connected!");
                _connectionStatus = ChargerConnectionState.Connected;
            }
            if(e.ChargerIsConnected == false && _connectionStatus == ChargerConnectionState.Connected)
            {
                _display.DisplayMessage("Charger Disconnected!");
                _connectionStatus = ChargerConnectionState.Disconnected;
            }
        }
    }
}
