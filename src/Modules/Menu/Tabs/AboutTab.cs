using UnityCheatTemplate.Enums;
using UnityCheatTemplate.Modules.Menu.Core;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Menu.Tabs;

internal class AboutTab : CheatMenuTab
{
    internal override string TabName => "About";
    internal override UiTabs UiTabType => UiTabs.About;

    internal override void OnGUI()
    {
        string info = "";
        GUILayout.Label(info, GUI.skin.textArea, GUILayout.ExpandHeight(true));
    }
}
