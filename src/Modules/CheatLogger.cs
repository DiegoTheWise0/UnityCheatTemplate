using UnityCheatTemplate.Utilities;

namespace UnityCheatTemplate.Modules;

/// <summary>
/// Provides logging functionality for the cheat system with file-based output.
/// All log messages are written to a timestamped log file in the game directory.
/// </summary>
internal static class CheatLogger
{
    /// <summary>
    /// Gets the log file name based on the cheat name.
    /// </summary>
    internal static string FileName => $"{CheatInfo.Name.ToLower().Trim()}.log";

    /// <summary>
    /// Gets the full file path to the log file in the game directory.
    /// </summary>
    internal static string FilePath => Path.Combine(Utils.GetPathToGame(), FileName);

    /// <summary>
    /// Initializes the logger by clearing any existing log file.
    /// </summary>
    internal static void Load()
    {
        File.WriteAllText(FilePath, string.Empty);
    }

    /// <summary>
    /// Unloads the logger (currently does nothing, reserved for future use).
    /// </summary>
    internal static void Unload()
    {
    }

    /// <summary>
    /// Logs an informational message with a custom tag.
    /// </summary>
    /// <param name="msg">The message to log.</param>
    /// <param name="tag">The tag/category for the log message (default: "Log").</param>
    internal static void Info(string msg, string tag = "Log")
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var format = $"{timestamp} [{tag}]: {msg}{Environment.NewLine}";
        File.AppendAllText(FilePath, format);
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="msg">The warning message to log.</param>
    internal static void Warning(string msg) => Info(msg, "Warning");

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="msg">The error message to log.</param>
    internal static void Error(string msg) => Info(msg, "Error");

    /// <summary>
    /// Logs an exception with full stack trace.
    /// </summary>
    /// <param name="ex">The exception to log.</param>
    internal static void Error(Exception ex) => Error(ex.ToString());
}