using System;
using NUnit.Framework;
using ChargingStationLibrary;

namespace ChargingStationTest
{
    [TestFixture]
    public class TestRfidReaderSimulator
    {
        private RfidReaderSimulator _uut;

        private RfidEventArgs _RfidDetectedEvent;

        [SetUp]
        public void Setup()
        {
            _uut = new RfidReaderSimulator();

            _RfidDetectedEvent = null;

            _uut.RfidDetected +=
                (o, args) =>
                {
                    _RfidDetectedEvent = args;
                };
        }

        [Test]
        public void Detect_Rfid_EventFired()
        {
            _uut.SimulateNewRfidDetected(1234);
            Assert.That(_RfidDetectedEvent, Is.Not.Null);
        }

        [TestCase(1234)]
        public void Correct_Rfid_Value_Passed(int rfid)
        {
            _uut.SimulateNewRfidDetected(rfid);

            Assert.That(_RfidDetectedEvent.Rfid, Is.EqualTo(rfid));
        }

        
    }
}
