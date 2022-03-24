#nullable disable
namespace ChargingStationLibrary
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen
        };

        // Her mangler flere member variable
        private LadeskabState _state;
        private IChargeController _charger;
        private int _oldId;
        private IDoor _door;
        private IRfidReader _rfidReader;
        private ILog _log;
        private IDisplay _display;

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil
        private bool oldDoorStatus;
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

          _rfidReader.RfidDetected += RfidDetected;
          _door.DoorChanged += OnDoorStatusChange;

        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(object sender, RfidEventArgs e)
        {
          int id = e.Rfid;

            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.IsConnected)
                    {
                        _door.Lock();
                        _charger.StartCharge();
                        _oldId = id;
                        _log.Log($": Skab låst med RFID: {id}");

                        _display.DisplayMessage("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;
                    }
                    else
                    {
                        _display.DisplayMessage("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.Unlock();
                        
                        _log.Log($": Skab låst op med RFID: {id}");
                       
                        _display.DisplayMessage("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
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
          if (e.DoorIsOpen != oldDoorStatus && e.DoorIsOpen == true)
          {
            _display.DisplayMessage("Tilslut Telefon");
            oldDoorStatus = true;
          }
          else if (e.DoorIsOpen != oldDoorStatus && e.DoorIsOpen == false)
          {
            _display.DisplayMessage("Indlæs Rfid");
          }
        }
    }
}
