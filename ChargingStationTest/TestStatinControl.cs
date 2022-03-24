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

        private RfidEventArgs _RfidDetectedEvent;
        private DoorEventArgs _DoorDetectedEvent;

        [SetUp]
        public void Setup()
        {
            _display = Substitute.For<IDisplay>();
            _door = Substitute.For<IDoor>();
            _rfidReader = Substitute.For<IRfidReader>();
            _chargeController = Substitute.For<IChargeController>();
            _log = Substitute.For<ILog>();

            _uut = new StationControl(_chargeController, _door, _rfidReader, _log, _display);

            _RfidDetectedEvent = null;
            _DoorDetectedEvent = null;

            _rfidReader.RfidDetected +=
                (o, args) =>
                {
                    _RfidDetectedEvent = args;
                };
            _door.DoorChanged +=
                (o, args) =>
                {
                    _DoorDetectedEvent = args;
                };
        }

        
        [TestCase(true)]
        public void Station_Door_From_Closed_to_Open_Event(bool doorOpen)
        {
            //Tell the substitute to raise the event with EventArgs:
            _door.DoorChanged += Raise.EventWith(new DoorEventArgs() { DoorIsOpen = doorOpen});

            Assert.That(_uut._doorState, Is.EqualTo(StationControl.DoorState.open));
        }
    }
}
