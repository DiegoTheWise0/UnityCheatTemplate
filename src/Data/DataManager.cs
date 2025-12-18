#pragma warning disable CS8618
#pragma warning disable CS8625

using UnityCheatTemplate.Data.Json;
using UnityCheatTemplate.Interfaces;

namespace UnityCheatTemplate.Data;

/// <summary>
/// Manages the loading, saving, and access to cheat configuration data and settings.
/// Implements both ILoadable and ISingleton interfaces for managed lifecycle and singleton access.
/// </summary>
internal sealed class DataManager : ILoadable, ISingleton
{
    /// <summary>
    /// Gets the current settings configuration loaded from disk.
    /// </summary>
    internal SettingsFile SettingsFile { get; private set; }

    /// <summary>
    /// Loads the cheat settings and configuration data from disk.
    /// </summary>
    public void Load()
    {
        SettingsFile = new();
        SettingsFile.Load(data =>
        {
            SettingsFile = data;
        });
    }

    /// <summary>
    /// Unloads and cleans up the cheat settings and configuration data.
    /// </summary>
    public void Unload()
    {
        SettingsFile = null;
    }

    /// <summary>
    /// Saves the current cheat settings and configuration data to disk.
    /// </summary>
    internal void Save()
    {
        SettingsFile?.Save();
    }
}