using UsbSimulator;

namespace ChargingStationLibrary;

public class ChargeController : IChargeController
{
  private UsbChargerSimulator _usbCharger;

  public ChargeController(UsbChargerSimulator usbCharger)
  {
    _usbCharger = usbCharger;
  }

  public event EventHandler<ChargerConnectEvent>? ConnectionStatusEvent;
  public void Connect()
  {
    throw new NotImplementedException();
  }

  public void Disconnect()
  {
    throw new NotImplementedException();
  }

  public void ChargingMessages()
  {
    throw new NotImplementedException();
  }
}