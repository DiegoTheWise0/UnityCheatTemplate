using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityCheatTemplate.Utilities;

/// <summary>
/// Provides platform utility functions including Windows API calls and file system operations.
/// </summary>
internal static class Utils
{
    /// <summary>
    /// Displays a modal dialog box that contains a system icon, a set of buttons, and a brief application-specific message.
    /// </summary>
    /// <param name="hWnd">A handle to the owner window of the message box to be created.</param>
    /// <param name="text">The message to be displayed in the message box.</param>
    /// <param name="caption">The dialog box title.</param>
    /// <param name="type">The contents and behavior of the dialog box.</param>
    /// <returns>An integer value indicating which button the user clicked.</returns>
    [DllImport("User32.dll")]
    internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    /// <summary>
    /// Displays a simple message box with the specified message and the cheat name as the title.
    /// </summary>
    /// <param name="message">The message text to display in the message box.</param>
    internal static void ShowMessageBox(string message)
    {
        MessageBox(IntPtr.Zero, message, CheatInfo.Name, 0);
    }

    /// <summary>
    /// Gets the file system path to the game's installation directory.
    /// </summary>
    /// <returns>The directory path containing the game's data folder, or the data folder path if the parent directory cannot be determined.</returns>
    internal static string GetPathToGame() => Path.GetDirectoryName(Application.dataPath) ?? Application.dataPath;
}