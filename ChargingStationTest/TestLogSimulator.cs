using System;
using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using ChargingStationLibrary;

namespace ChargingStationTest
{

  [TestFixture]
  public class LogSimulatorTest
  {
    private LogSimulator _uut;

    [SetUp]
    public void SetUp()
    {
      _uut = new LogSimulator();
    }

    [Test]
    public void ConstructorCreateFile()
    {
      Assert.That(File.Exists(_uut.FilePath), Is.True);  
    }

    [Test]
    public void LogFileIsEmptyByDefault()
    {
      Assert.That(File.ReadAllText(_uut.FilePath),Is.EqualTo(""));
    }

    [Test]
    public void LogSimulatorCanLogASingleMesssage()
    {
      string loggedMessage = _uut.RecordMessage("This is a test Message");
      string[] messageFromLogFile = File.ReadAllLines(_uut.FilePath);
      Assert.That(loggedMessage, Is.EqualTo(messageFromLogFile[0]));
    }

    [Test]
    public void LogSimulatorCanLogMultipleMessages()
    {
      string loggedMessage1 = _uut.RecordMessage("this is the first test Message");
      string loggedMessage2 = _uut.RecordMessage("This is the second test Message");
      string[] messagesFromLogFile = File.ReadAllLines(_uut.FilePath);

      Assert.That(loggedMessage1, Is.EquivalentTo(messagesFromLogFile[0]));
      Assert.That(loggedMessage2, Is.EquivalentTo(messagesFromLogFile[1]));
    }

    [Test]
    public void LogSimulatorConstructorDoesNotOverwriteLogFileOnConstruction()
    {
      string loggedMessage1 = _uut.RecordMessage("this is the first test Message");

      _uut = new LogSimulator();

      string[] messagesFromLogFile = File.ReadAllLines(_uut.FilePath);


      Assert.That(loggedMessage1, Is.EquivalentTo(messagesFromLogFile[0]));
    }

    [TearDown]
    public void TearDown()
    {
      GC.Collect();
      File.Delete(_uut.FilePath);

    }
  }
}
