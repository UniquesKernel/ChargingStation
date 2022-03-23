using System;
using NUnit.Framework;
using ChargingStationLibrary;

namespace ChargingStationTest
{
    [TestFixture]
    public class TestDisplaySimulator
    {
        private string? _consoleText;
        private DisplaySimulator _uut;

        [SetUp]
        public void Setup()
        {
            _uut = new DisplaySimulator();
            _consoleText = null;
        }

        [TestCase("TestText")]
        public void Validate_Displayed_Text(string testText)
        {
            _uut.DisplayContent(testText);

            Assert.That(_consoleText, Is.EqualTo(_uut.FrameBuffer));
        }
         
    }
}
