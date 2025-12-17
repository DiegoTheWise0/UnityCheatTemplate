using UnityCheatTemplate.Data.Json;
using UnityCheatTemplate.Interfaces;

namespace UnityCheatTemplate.Data;

internal class DataManager : ILoadable, ISingleton
{
    internal SettingsFile SettingsFile { get; private set; }

    public void Load()
    {
        SettingsFile = new();
        SettingsFile.Load(data =>
        {
            SettingsFile = data;
        });
    }

    public void Unload()
    {
        SettingsFile = null;
    }

    internal void Save()
    {
        SettingsFile?.Save();
    }
}
