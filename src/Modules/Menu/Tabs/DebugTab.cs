using UnityCheatTemplate.Modules.Menu.Core;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Menu.Tabs;

internal sealed class DebugTab : CheatMenuTab
{
    private Vector2 _scrollPosition = Vector2.zero;

    internal override string TabName => "Debug";
    internal override uint TabIndex => 2;

    internal override void OnGUI()
    {
        UI.Header("Console");

        _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
        GUILayout.Label(string.Join("\n", CheatLogger.Logs), GUI.skin.textArea);
        GUILayout.EndScrollView();
    }
}