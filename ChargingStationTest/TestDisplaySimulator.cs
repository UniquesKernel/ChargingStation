using System.Diagnostics;
using NUnit.Framework;
using ChargingStationLibrary;

namespace ChargingStationTest
{
    [TestFixture]
    public class TestDisplaySimulator
    {
        private string? _consoleText;
        private DisplaySimulator? _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new DisplaySimulator();
        }

        [TestCase("Message")]
        public void Validate_Displayed_Messsage(string testText)
        {
            _uut.DisplayMessage(testText);

            Assert.That(testText, Is.EqualTo(_uut.messageToUser));
        }

        [TestCase("ChargingStatus")]
        public void Validate_Displayed_ChargingStatus(string testText)
        {
            _uut.DisplayChargerStatus(testText);

            Assert.That(testText , Is.EqualTo(_uut.chargingStatus));
        }
    }
}
