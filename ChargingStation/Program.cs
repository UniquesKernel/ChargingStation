using System.Net.Http.Headers;
using UsbSimulator;

namespace ChargingStationLibrary;
    class Program
    {
        static void Main(string[] args)
        {
          DoorSimulator door = new DoorSimulator();
          UsbChargerSimulator charger = new UsbChargerSimulator();
          DisplaySimulator display = new DisplaySimulator();
          ChargeController chargeController = new ChargeController(charger, display);
          LogSimulator log = new LogSimulator();
          RfidReaderSimulator reader = new RfidReaderSimulator();

          StationControl stationControl = new StationControl(chargeController, door, reader, log, display);

            bool finish = false;
            do
            {
                string input;
                System.Console.WriteLine("Indtast E, O, T, F, C, R: ");
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        door.OpenDoor();
                        break;

                    case 'T':
                      chargeController.Connect();
                      break;

                    case 'F':
                      chargeController.Disconnect();
                      break;

                    case 'C':
                        door.CloseDoor();
                        break;

                    case 'R':
                        System.Console.WriteLine("Indtast RFID id: ");
                        string idString = System.Console.ReadLine();

                        int id = Convert.ToInt32(idString);
                        reader.SimulateNewRfidDetected(id);
                        break;

                    default:
                        break;
                }

            } while (!finish);
        }
    }

