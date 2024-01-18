using System.Diagnostics;
using Shared.Interfaces;

namespace Shared.Utilis;

public class Logger(string filePath) : ILogger
{
    private readonly string _filePath = filePath;

    public void Log(string message, string method)
    {
        try
        {
            var logMessage = $"{DateTime.Now}, method:{method} :: {message}";
            Debug.WriteLine(logMessage);

            using var sw = new StreamWriter(_filePath, true);
            sw.WriteLine(message);
        }
        catch (Exception ex) { Debug.WriteLine($"LOG ERROR! {DateTime.Now}, method:{nameof(Log)} :: {ex.Message}"); }
    }
}
