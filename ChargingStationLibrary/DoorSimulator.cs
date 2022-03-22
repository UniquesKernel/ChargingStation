using System.Timers;

namespace ChargingStationLibrary;

public class DoorSimulator : IDoor
{

  // Constants
  public const int TimeTickerInterval = 250;

  public DoorSimulator()
  {
    _timer = new System.Timers.Timer();
    _timer.Enabled = false;
    _timer.Interval = TimeTickerInterval;
    _timer.Elapsed += TimerOnElapsed;
  }
  
  #region Properties
  
  private System.Timers.Timer _timer;
  public bool IsOpen { get; private set; } = false;
  public bool IsLocked { get; private set; } = false;
  public event EventHandler<DoorEventArgs>? DoorChanged;
  
  #endregion
  
  #region Methods
  private void TimerOnElapsed(object sender, ElapsedEventArgs e)
  {
    OnDoorChanged();
  }

  public void Lock()
  {
    if (!IsLocked)
    {
      IsLocked = true;
    }
  }

  public void Unlock()
  {
    if (IsLocked)
    {
      IsLocked = false;
    }
  }

  public void SimulateOpenDoor()
  {
    if (!IsOpen)
    {
      IsOpen = true;
    }
  }
  public void SimulateClosedDoor()
  {
    if (IsOpen)
    {
      IsOpen = false;
    }
  }

  public void OpenDoor()
  {
    SimulateOpenDoor();
    OnDoorChanged();
    _timer.Start();
  }

  public void CloseDoor()
  { 
    SimulateClosedDoor();
    OnDoorChanged();
    _timer.Stop();
  }

  private void OnDoorChanged()
  {
    DoorChanged?.Invoke(this, new DoorEventArgs() {DoorIsOpen = IsOpen});
  }

  #endregion
 }