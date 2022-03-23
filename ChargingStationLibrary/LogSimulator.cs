using System.IO;

namespace ChargingStationLibrary;

public class LogSimulator : ILog
{
  private readonly string _filename = "logFile.txt";

  public void Log(string message)
  {
    var writer = File.AppendText(_filename);
    writer.WriteLine(message);
  }

}
