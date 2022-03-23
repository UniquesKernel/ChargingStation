﻿using NUnit.Framework;
using ChargingStationLibrary;
using UsbSimulator;
using System;
using System.Threading;
using System.Threading.Tasks;
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

    [Test]
    public void testStopCharge()
    {
        _uut.Connect();
        _uut.StartCharge();
        System.Threading.Thread.Sleep(500);

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

    private bool _overload;
    public bool _charging { get; private set; }
    private System.Timers.Timer _timer;
    private int _ticksSinceStart;

    public usbMock()
    {
        CurrentValue = 0.0;
        Connected = true;
        _overload = false;

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
    public void DisplayContent(string inputText)
    {
        status = inputText;
    }
}