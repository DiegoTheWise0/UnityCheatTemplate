using UnityCheatTemplate.Data;
using UnityCheatTemplate.Enums;
using UnityCheatTemplate.Modules.Menu.Core;
using UnityCheatTemplate.Utilities;

namespace UnityCheatTemplate.Modules.Menu.Tabs;

internal class SettingsTab : CheatMenuTab
{
    internal override string TabName => "Settings";
    internal override UiTabs UiTabType => UiTabs.Settings;

    internal override void OnGUI()
    {
        UI.Button("Save Settings", "Save the current settings", Singleton<DataManager>.Instance.Save);
    }
}
