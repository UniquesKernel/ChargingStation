using System;
using NUnit.Framework;
using ChargingStationLibrary;

namespace ChargingStationTest
{
    [TestFixture]
    public class TestRfidReaderSimulator
    {
        private RfidReaderSimulator _uut;

        public void Setup()
        {
            _uut = new RfidReaderSimulator();
        }
    }
}
