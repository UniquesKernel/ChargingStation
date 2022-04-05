using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ChargingStationLibrary;
using NSubstitute;

namespace ChargingStationTest
{
    [TestFixture]
    public class TestStatinControl
    {
        private StationControl _uut;
        private IDisplay _display;
        private IDoor _door;
        private IRfidReader _rfidReader;
        private IChargeController _chargeController;
        private ILog _log;

        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();
            _rfidReader = Substitute.For<IRfidReader>();
            _chargeController = Substitute.For<IChargeController>();
            _log = Substitute.For<ILog>();

            _uut = new StationControl(_chargeController, _door, _rfidReader, _log, _display);
        }


        [Test]
        public void Station_Door_From_Closed_to_Open_Event()
        {
            //Tell the substitute to raise the event with EventArgs:
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });

            Assert.That(_uut._doorState, Is.EqualTo(StationControl.DoorState.open));
            _display.Received(1).DisplayMessage("Tilslut Telefon");
        }

        [Test]
        public void Station_Door_From_Open_to_Closed_Event()
        {
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = false });

            Assert.That(_uut._doorState, Is.EqualTo(StationControl.DoorState.closed));
            _display.Received(1).DisplayMessage("Indlæs Rfid");
        }

        [Test]
        public void Station_Connect_To_Charger()
        {
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });

            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });

            Assert.That(_uut._connectionStatus, Is.EqualTo(StationControl.ChargerConnectionState.Connected));
            _display.Received(1).DisplayMessage("Charger Connected!");
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Station_Disconnect_From_Charger(bool withDoors)
        {
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });

            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });

            if (withDoors)
            { 
                _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = false });

                _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });
            }

            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = false });

            Assert.That(_uut._connectionStatus, Is.EqualTo(StationControl.ChargerConnectionState.Disconnected));
            _display.Received(1).DisplayMessage("Charger Disconnected!");
        }

        [Test]
        public void Station_RFID_Door_Closed()
        {
            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = false });
            _uut._stationState = StationControl.ChargingStatitionState.Available;

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9875 });

            Assert.That(_uut.OldId, Is.EqualTo(9875));
            _display.Received(1).DisplayMessage("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
            _door.Received(1).Lock();
            _chargeController.Received(1).StartCharge();
        }

        [Test]
        public void Station_RFID_Door_Open()
        {
            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9875 });

           
            _display.Received(1).DisplayMessage("Luk døren og prøv igen");

        }

        [Test]
        public void Station_RFID_Not_Connected()
        {
            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = false });

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9875 });


            _display.Received(1).DisplayMessage("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");

        }

        [Test]
        public void Station_Locked_RFID_Correct()
        {
            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = false });

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9875 });

            Assert.That(_uut._stationState, Is.EqualTo(StationControl.ChargingStatitionState.Locked));

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9875 });

            _chargeController.Received(1).StopCharge();
            _door.Received(1).Unlock();
            _log.Received(1).RecordMessage($": Skab låst op med RFID: 9875");
            _display.Received(1).DisplayMessage("Tag din telefon ud af skabet og luk døren");
            Assert.That(_uut._stationState, Is.EqualTo(StationControl.ChargingStatitionState.Available));

        }
        [Test]
        public void Station_Locked_RFID_Incorrect()
        {
            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = false });

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9875 });

            Assert.That(_uut._stationState, Is.EqualTo(StationControl.ChargingStatitionState.Locked));

            _rfidReader.RfidDetected += Raise.EventWith(new RfidEventArgs() { Rfid = 9854 });

            _display.Received(1).DisplayMessage("Forkert RFID tag");

        }
    }
}
