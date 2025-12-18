namespace UnityCheatTemplate.Interfaces;

/// <summary>
/// Provides a standardized interface for components that require explicit loading and unloading.
/// Implement this interface to create components with managed lifecycle that can be properly
/// initialized and cleaned up.
/// </summary>
internal interface ILoadable
{
    /// <summary>
    /// Initializes and loads the component, preparing it for use.
    /// This method should be called before using any functionality of the implementing class.
    /// </summary>
    void Load();

    /// <summary>
    /// Safely unloads and cleans up the component, releasing all resources.
    /// This method should be called when the component is no longer needed.
    /// </summary>
    void Unload();
}