using NUnit.Framework;

namespace ChargingStationTest;

[TestFixture]
public class TestChargeController
{
  private ChargeController _uut;

  [SetUp]
  public void SetUp()
  {
    UsbChargerSimulator usbSim = new UsbChargerSimulator();
    _uut = new ChargeController(usbSim);
  }
}
