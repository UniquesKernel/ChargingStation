#nullable disable
using NUnit.Framework;
using ChargingStationLibrary;
using UsbSimulator;
using System;
using System.Timers;

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
        bool connected = false;
        _uut.ConnectionStatusEvent += (o, args) => connected = args.ChargerIsConnected;
        _uut.Connect();
        System.Threading.Thread.Sleep(300);
        Assert.That(connected, Is.True);
    }

    [Test]
    public void testDisconnect()
    {
        bool connected = false;
        _uut.ConnectionStatusEvent += (o, args) => connected = args.ChargerIsConnected;

        _uut.Connect();
        System.Threading.Thread.Sleep(300);
        Assert.That(connected, Is.True); // check that it connected properly ( just in case)

        _uut.Disconnect();
        System.Threading.Thread.Sleep(300);
        Assert.That(connected, Is.False);
    }

    [Test]
    public void testStartCharge()
    {
        _uut.Connect();
        _uut.StartCharge();
        System.Threading.Thread.Sleep(300);

        Assert.That(mockDisplay.status, Is.EqualTo("charging"));
    }

    [Test]
    public void testOverloadEvent()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.setOvercharge();
        System.Threading.Thread.Sleep(300);

        Assert.That(mockDisplay.status, Is.EqualTo("overcharge"));
    }

    [Test]
    public void testFullCharge()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.setChargeDone();
        System.Threading.Thread.Sleep(300);

        Assert.That(mockDisplay.status, Is.EqualTo("charging complete"));
    }


    [Test]
    public void testLowChargingCurrent()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.CurrentValue = 5.1;
        System.Threading.Thread.Sleep(300);

        Assert.That(mockDisplay.status, Is.EqualTo("charging"));
    }

    [Test]
    public void testChargingEdgecase()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.CurrentValue = 5;
        System.Threading.Thread.Sleep(300);

        Assert.That(mockDisplay.status, Is.EqualTo("charging complete"));
    }

    [Test]
    public void testNoConection()
    {
        _uut.Connect();
        _uut.StartCharge();
        mockUSB.CurrentValue = 0;
        System.Threading.Thread.Sleep(300);

        Assert.That(mockDisplay.status, Is.EqualTo(""));
    }

    [Test]
    public void testStopCharge()
    {
        _uut.Connect();
        _uut.StartCharge();
        System.Threading.Thread.Sleep(300);

        _uut.StopCharge();
        Assert.That(mockUSB._charging, Is.EqualTo(false));
    }


}  
public class usbMock : IUsbCharger
{
    // Constants
    private const double FullyChargedCurrent = 2.5; // mA
    private const double OverloadCurrent = 750; // mA
    private const int CurrentTickInterval = 250; // ms

    public event EventHandler<CurrentEventArgs> CurrentValueEvent;

    public double CurrentValue { get; set; }

    public bool Connected { get; private set; }

    public bool _charging { get; private set; }
    private System.Timers.Timer _timer;

    public usbMock()
    {
        CurrentValue = 0.0;
        Connected = true;

        _timer = new System.Timers.Timer();
        _timer.Enabled = false;
        _timer.Interval = CurrentTickInterval;
        _timer.Elapsed += TimerOnElapsed;
    }

    private void TimerOnElapsed(object sender, ElapsedEventArgs e)
    {
        // Only execute if charging
        if (_charging)
        {
            OnNewCurrent();
        }
    }

    // Start charging
    public void StartCharge()
    {
        _charging = true;
        CurrentValue = 400;
        _timer.Start();
    }

    // Stop charging
    public void StopCharge()
    {
        _charging = false;
        _timer.Stop();
    }

    private void OnNewCurrent()
    {
        CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = this.CurrentValue });
    }

    public void setOvercharge()
    {
        CurrentValue = OverloadCurrent;
    }

    public void setChargeDone()
    {
        CurrentValue = FullyChargedCurrent;
    }

}

public class displayMock : IDisplay
{
    public string status { get; private set; }
    public void DisplayChargerStatus(string inputText)
    {
        status = inputText;
    }
    public void DisplayMessage(string inputText) { }
    public void UpdateDisplay() { }
}
