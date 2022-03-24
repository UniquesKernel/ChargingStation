using System.Security.Cryptography.X509Certificates;
using System.Threading;
using ChargingStationLibrary;
using NUnit.Framework;

namespace ChargingStationTest;

[TestFixture]
public class TestDoorSimulator
{
  private DoorSimulator _uut;

  [SetUp]
  public void SetUp()
  {
    _uut = new DoorSimulator();
  }

  [Test]
  public void DoorIsClosedByDefault()
  {
    Assert.That(_uut.IsOpen, Is.False);
  }

  [Test]
  public void DoorIsUnlockedByDefault()
  {
    Assert.That(_uut.IsLocked, Is.False);
  }

  [Test]
  public void DoorCanBeLocked()
  {
    _uut.Lock();
    Assert.That(_uut.IsLocked, Is.True);
  }

  [Test]
  public void DoorCanBeUnlocked()
  {
    _uut.Lock();
    _uut.Unlock();
    Assert.That(_uut.IsLocked, Is.False);
  }

  [Test]
  public void DoorCanBeOpened()
  {
    _uut.SimulateOpenDoor();
    Assert.That(_uut.IsOpen, Is.True);
  }

  [Test]
  public void DoorCanBeClosed()
  {
    _uut.SimulateOpenDoor();
    _uut.SimulateClosedDoor();
    Assert.That(_uut.IsOpen, Is.False);
  }

  [Test]
  public void DoorCanBeOpenedByEvent()
  {
    ManualResetEvent pause = new ManualResetEvent(false);
    bool LastDoorStatus = false;

    _uut.DoorChanged += (o, args) =>
    {
      LastDoorStatus = args.DoorIsOpen;
      pause.Set();
    };

    _uut.OpenDoor();

    pause.Reset();

    Assert.That(LastDoorStatus, Is.True);
  }

  [Test]
  public void DoorCanBeClosedByEvent()
  {
    ManualResetEvent pause = new ManualResetEvent(false);
    bool LastDoorStatus = true;

    _uut.DoorChanged += (o, args) =>
    {
      LastDoorStatus = args.DoorIsOpen;
      pause.Set();
    };

    _uut.CloseDoor();

    pause.Reset();

    Assert.That(LastDoorStatus, Is.False);
  }

}