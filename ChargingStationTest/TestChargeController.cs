using NUnit.Framework;
using ChargingStationLibrary;
using UsbSimulator;

namespace ChargingStationTest;

[TestFixture]
public class TestChargeController
{
    private ChargeController _uut;
    [SetUp]
    public void SetUp()
    {
        _uut = new ChargeController(new UsbChargerSimulator());
    }

    [Test]
    public void IsDisconnectedByDefault()
    {
        Assert.That(_uut.IsConnected, Is.False);
    }

    [Test]
    public void testConnect()
    {
        _uut.Connect();
        Assert.That(_uut.IsConnected,Is.True);
    }

    [Test]
    public void testDisconnect()
    {
        _uut.Connect();
        _uut.Disconnect();
        Assert.That(_uut.IsConnected, Is.False);
    }
}