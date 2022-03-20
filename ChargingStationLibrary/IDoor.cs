namespace ChargingStationLibrary;

public class DoorEventArgs : EventArgs
{
  public bool DoorIsOpen;
}
public interface IDoor
{
  event EventHandler<DoorEventArgs> CurrentDoorStatus;

  bool DoorIsOpenStatus { get; }
  bool DoorIsLocked { get; }
}