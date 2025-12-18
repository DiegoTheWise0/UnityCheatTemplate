namespace UnityCheatTemplate.Modules.Menu.Core;

/// <summary>
/// Abstract base class for creating cheat menu tabs.
/// Inherit from this class to create custom tabbed interfaces in the cheat menu.
/// </summary>
internal abstract class CheatMenuTab
{
    /// <summary>
    /// Gets the display name of the tab shown in the tab bar.
    /// </summary>
    internal abstract string TabName { get; }

    /// <summary>
    /// Gets the unique index identifier for the tab.
    /// Used for tab ordering and selection.
    /// </summary>
    internal abstract uint TabIndex { get; }

    /// <summary>
    /// Called when the tab's GUI content needs to be rendered.
    /// Implement this method to draw your tab's content.
    /// </summary>
    internal abstract void OnGUI();

    /// <summary>
    /// Determines whether this tab should be drawn.
    /// Override this method to conditionally show/hide tabs.
    /// </summary>
    /// <returns>True if the tab should be drawn; otherwise false.</returns>
    internal virtual bool CanDraw() => true;
}