using System.Runtime.InteropServices;
using UnityEngine;

namespace UnityCheatTemplate.Utilities;

internal static class Utils
{
    [DllImport("User32.dll")]
    internal static extern short GetAsyncKeyState(int key);

    [DllImport("User32.dll")]
    internal static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    internal static void ShowMessageBox(string message)
    {
        MessageBox(IntPtr.Zero, message, CheatInfo.Name, 0);
    }

    internal static string GetPathToGame() => Path.GetDirectoryName(Application.dataPath) ?? Application.dataPath;
}