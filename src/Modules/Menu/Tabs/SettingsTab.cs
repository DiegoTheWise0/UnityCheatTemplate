using UnityCheatTemplate.Data;
using UnityCheatTemplate.Modules.Menu.Core;
using UnityCheatTemplate.Utilities;

namespace UnityCheatTemplate.Modules.Menu.Tabs;

internal sealed class SettingsTab : CheatMenuTab
{
    internal override string TabName => "Settings";
    internal override uint TabIndex => 1;

    internal override void OnGUI()
    {
        UI.Button("Save Settings", "Save the current settings", Singleton<DataManager>.Instance.Save);
        UI.ColorPicker(ref Singleton<DataManager>.Instance.SettingsFile.c_Theme, "Menu Theme");
    }
}
