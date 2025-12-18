namespace UnityCheatTemplate;

/// <summary>
/// Provides static information about the cheat.
/// Contains constants and version information used throughout the cheat system.
/// </summary>
internal static class CheatInfo
{
    /// <summary>
    /// The display name of the cheat.
    /// </summary>
    internal const string Name = "UnityCheatTemplate";

    /// <summary>
    /// The current version of the cheat.
    /// </summary>
    internal static Version Version = new(1, 0, 0);

    /// <summary>
    /// The unique identifier (GUID) for the cheat.
    /// Used for Harmony patching and other identification purposes.
    /// </summary>
    internal const string GUID = "com.name.unitycheattemplate";
}