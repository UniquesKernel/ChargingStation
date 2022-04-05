using System.IO;

namespace ChargingStationLibrary;

public class LogSimulator : ILog
{
  private readonly string _filename = "logFile.txt";
  public string FilePath {get; private set;}

  public LogSimulator()
  {
    FilePath = Environment.CurrentDirectory + "\\" + _filename;
    CreateLogFile(FilePath);
  }

  private void CreateLogFile(string filepath)
  {
    if (!File.Exists(filepath))
    {
      var file = File.Create(filepath);
      file.Close();
    }
  }
  public string RecordMessage(string message)
  {
    var writer = File.AppendText(_filename);
    string logMessage = DateTime.Now + " : " + message;
    writer.WriteLine(logMessage);
    writer.Close();

    return logMessage;
  }
}
