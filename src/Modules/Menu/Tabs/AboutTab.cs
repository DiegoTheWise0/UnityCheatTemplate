using UnityCheatTemplate.Modules.Menu.Core;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Menu.Tabs;

internal sealed class AboutTab : CheatMenuTab
{
    internal override string TabName => "About";
    internal override uint TabIndex => 0;

    internal override void OnGUI()
    {
        string info = "";
        GUILayout.Label(info, GUI.skin.textArea, GUILayout.ExpandHeight(true));
    }
}
