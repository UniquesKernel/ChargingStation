using System;
using NUnit.Framework;
using ChargingStationLibrary;

namespace ChargingStationTest
{
    [TestFixture]
    public class TestRfidReaderSimulator
    {
        private RfidReaderSimulator _uut;

        private RfidEventArgs _RfidDetectedevent;

        [SetUp]
        public void Setup()
        {
            _uut = new RfidReaderSimulator();

            _RfidDetectedevent = null;

            _uut.RfidDetected +=
                (o, args) =>
                {
                    _RfidDetectedevent = args;
                };
        }

        [Test]
        public void Detect_Rfid_EventFired()
        {
            _uut.SimulateNewRfidDetected(1234);
            Assert.That(_RfidDetectedevent, Is.Not.Null);
        }
    }
}
