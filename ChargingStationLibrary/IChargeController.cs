namespace ChargingStationLibrary;

public class ChargerConnectEvent : EventArgs
{
  public bool ChargerIsConnected { get; set; } = false;
}
public interface IChargeController
{
  event EventHandler<ChargerConnectEvent> ConnectionStatusEvent;
  void Connect();
  void Disconnect();

  void ChargingMessages();

  /**/
}