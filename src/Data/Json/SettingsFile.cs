using UnityCheatTemplate.Utilities;

namespace UnityCheatTemplate.Data.Json;

[Serializable]
internal class SettingsFile : JsonFile<SettingsFile>
{
    internal override string FilePath => Utils.GetPathToGame();

    internal override string FileName => $"{CheatInfo.Name.ToLower().Trim()}_settings.json";

    // Menu
    public Color c_Theme = Color.white;

    // ESP
    public bool b_Esp;
    public float f_EspRange = 500f;
    public bool b_Tracer;
}
