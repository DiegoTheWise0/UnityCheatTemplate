using UnityCheatTemplate.Data;
using UnityCheatTemplate.Extensions;
using UnityCheatTemplate.Modules;
using UnityCheatTemplate.Mono;

namespace UnityCheatTemplate;

/// <summary>
/// Provides static methods for loading and unloading the entire cheat system.
/// This class serves as the primary entry point for initializing and cleaning up all cheat components.
/// </summary>
public static class Loader
{
    /// <summary>
    /// Initializes and loads the complete cheat system with all its dependencies.
    /// This method should be called to start the cheat functionality.
    /// </summary>
    public static void Load()
    {
        CheatLogger.Load();
        DependencyResolver.Load(() =>
        {
            CheatLogger.Info($"Loading {CheatInfo.Name} v{CheatInfo.Version}");
            Singleton<DataManager>.Instance.Load();
            MonoCheat.Load();
            CheatLogger.Info($"{CheatInfo.Name} Successfully injected!");
        });
    }

    /// <summary>
    /// Safely unloads and cleans up the entire cheat system.
    /// This method should be called to properly disable and remove all cheat functionality.
    /// </summary>
    /// <remarks>
    public static void Unload()
    {
        CheatLogger.Info($"Unloading {CheatInfo.Name} v{CheatInfo.Version}");
        MonoCheat.Unload();
        ReflectExtension.ClearCaches();
        Singleton<DataManager>.Instance.Unload();
        Singleton<DataManager>.Dispose();
        CheatLogger.Unload();
        DependencyResolver.Unload();
    }
}