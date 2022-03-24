﻿using System;
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

        [Test]
        public void Station_Disconnect_From_Charger()
        {
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = true });

            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = true });

            _chargeController.ConnectionStatusEvent += Raise.EventWith(new ChargerConnectEvent() { ChargerIsConnected = false });

            Assert.That(_uut._connectionStatus, Is.EqualTo(StationControl.ChargerConnectionState.Disconnected));
            _display.Received(1).DisplayMessage("Charger Disconnected!");
        }

    }
}