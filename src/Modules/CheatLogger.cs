using UnityCheatTemplate.Utilities;

namespace UnityCheatTemplate.Modules;

internal static class CheatLogger
{
    internal static string FileName => $"{CheatInfo.Name.ToLower().Trim()}.log";
    internal static string FilePath => Path.Combine(Utils.GetPathToGame(), FileName);

    internal static void Load()
    {
        File.WriteAllText(FilePath, string.Empty);
    }

    internal static void Unload()
    {
    }

    internal static void Info(string msg, string tag = "Log")
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var format = $"{timestamp} [{tag}]: {msg}{Environment.NewLine}";
        File.AppendAllText(FilePath, format);
    }

    internal static void Warning(string msg) => Info(msg, "Warning");

    internal static void Error(string msg) => Info(msg, "Error");

    internal static void Error(Exception ex) => Error(ex.ToString());
}
