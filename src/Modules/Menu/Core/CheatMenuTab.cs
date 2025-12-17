using UnityCheatTemplate.Enums;

namespace UnityCheatTemplate.Modules.Menu.Core;

internal abstract class CheatMenuTab
{
    internal abstract string TabName { get; }
    internal abstract UiTabs UiTabType { get; }
    internal abstract void OnGUI();
    internal virtual bool CanDraw() => true;
}