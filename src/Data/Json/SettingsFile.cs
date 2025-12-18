using UnityCheatTemplate.Utilities;
using UnityEngine;

namespace UnityCheatTemplate.Data.Json;

/// <summary>
/// Represents the cheat settings configuration file with serializable properties.
/// Contains all user-configurable settings for the cheat application.
/// </summary>
[Serializable]
internal sealed class SettingsFile : JsonFile<SettingsFile>
{
    /// <summary>
    /// Gets the directory path where the settings file is stored (game installation directory).
    /// </summary>
    internal override string FilePath => Utils.GetPathToGame();

    /// <summary>
    /// Gets the name of the settings JSON file.
    /// </summary>
    internal override string FileName => $"{CheatInfo.Name.ToLower().Trim()}_settings.json";

    // Menu Settings

    public Color c_Theme = Color.white;

    // ESP Settings

    public bool b_Esp;
    public float f_EspRange = 500f;
    public bool b_Tracer;
}