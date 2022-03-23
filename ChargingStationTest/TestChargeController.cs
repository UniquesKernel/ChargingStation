using NUnit.Framework;
using ChargingStationLibrary;
using UsbSimulator;
using System.Threading;
using System.Threading.Tasks;

namespace ChargingStationTest;

[TestFixture]
public class TestChargeController
{
    private ChargeController _uut;
    private displayMock mockDisplay = new displayMock();
    private usbMock mockUSB = new usbMock();

    [SetUp]
    public void SetUp()
    {   
        _uut = new ChargeController(mockUSB, mockDisplay); 
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

    [Test]
    public void testStartCharge()
    {
        _uut.Connect();
        _uut.StartCharge();
        System.Threading.Thread.Sleep(500);

        Assert.That(mockDisplay.status, Is.EqualTo("charging"));
    }

    [Test]
    public void testOverloadEvent()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.setOvercharge();
        System.Threading.Thread.Sleep(500);

        Assert.That(mockDisplay.status, Is.EqualTo("overcharge"));
    }

    [Test]
    public void testFullCharge()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.setChargeDone();
        System.Threading.Thread.Sleep(500);

        Assert.That(mockDisplay.status, Is.EqualTo("charging complete"));
    }


    [Test]
    public void testLowChargingCurrent()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.CurrentValue = 5.1;
        System.Threading.Thread.Sleep(500);

        Assert.That(mockDisplay.status, Is.EqualTo("charging"));
    }

    [Test]
    public void testNoConection()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.CurrentValue = 0;
        System.Threading.Thread.Sleep(500);

        Assert.That(mockDisplay.status, Is.EqualTo(""));
    }
}