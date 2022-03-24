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

         /* Indtastnings muligheder:
          * E = Exit,
          * O = Open Door,
          * T = Connect Phone,
          * F = Disconnect Phone,
          * C = Close Door,
          * R = Input RFID,
          */

          bool finish = false;
            do
            {
                string input;      
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;

                switch (input[0])
                {
                    case 'E':
                        finish = true;
                        break;

                    case 'O':
                        if(!door.IsOpen && !door.IsLocked)
                        {
                            door.OpenDoor();
                        }
                        
                        break;

                    case 'T':
                      if(!chargeController.IsConnected && door.IsOpen)
                      {
                            chargeController.Connect();
                      }
                      
                      break;

                    case 'F':
                        if (chargeController.IsConnected && door.IsOpen)
                        {
                            chargeController.Disconnect();
                        }
                        
                      break;

                    case 'C':
                        if(door.IsOpen)
                        {
                            door.CloseDoor();
                        }
                        break;

                    case 'R':
                        display.DisplayMessage("Indtast RFID id: ");
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

