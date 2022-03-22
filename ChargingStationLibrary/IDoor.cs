namespace ChargingStationLibrary;

public class DoorEventArgs : EventArgs
{
  public bool DoorIsOpen { get; set; }

}
public interface IDoor
{
  event EventHandler<DoorEventArgs> DoorChanged;
  void Lock();
  void Unlock();

}