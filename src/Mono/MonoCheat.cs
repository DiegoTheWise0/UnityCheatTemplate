#pragma warning disable CS8618

using HarmonyLib;
using UnityCheatTemplate.Modules;
using UnityCheatTemplate.Modules.Cheats;
using UnityCheatTemplate.Modules.Menu.Core;
using UnityEngine;

namespace UnityCheatTemplate.Mono;

/// <summary>
/// Main MonoBehaviour component that serves as the entry point and manager for the cheat system.
/// Handles initialization, lifecycle management, and coordination of all cheat modules.
/// This component is automatically created and persisted across scene loads.
/// </summary>
internal sealed class MonoCheat : MonoBehaviour
{
    /// <summary>
    /// Gets the singleton instance of the MonoCheat component.
    /// </summary>
    internal static MonoCheat Instance { get; private set; }

    /// <summary>
    /// Harmony patcher instance used for method patching and detouring.
    /// </summary>
    internal static Harmony? harmony;

    /// <summary>
    /// Initializes and loads the entire cheat system.
    /// Creates the necessary GameObject with MonoCheat component, initializes all singletons,
    /// and prepares the cheat for operation.
    /// </summary>
    internal static void Load()
    {
        var obj = new GameObject($"{CheatInfo.Name.Trim()}{nameof(MonoCheat)}");
        var monoCheat = obj.AddComponent<MonoCheat>();
        DontDestroyOnLoad(monoCheat);

        Singleton<CheatMenuUI>.Instance.Load();
        Singleton<KeyBinder>.Instance.Load();
    }

    /// <summary>
    /// Safely unloads and cleans up the entire cheat system.
    /// Destroys the MonoCheat GameObject, unloads all modules, and disposes of all singleton instances.
    /// </summary>
    internal static void Unload()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance.gameObject);
        }

        Singleton<CheatMenuUI>.Instance.Unload();
        Singleton<CheatMenuUI>.Dispose();
        Singleton<KeyBinder>.Instance.Unload();
        Singleton<KeyBinder>.Dispose();
        Singleton<EspCheat>.Dispose();
        Singleton<TriggerCheat>.Dispose();
    }

    /// <summary>
    /// Unity MonoBehaviour callback called when the component is initialized.
    /// Ensures singleton pattern, initializes Harmony patching, and sets up the instance.
    /// </summary>
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        harmony = new(CheatInfo.GUID);
        harmony.PatchAll();
    }

    /// <summary>
    /// Unity MonoBehaviour callback called when the component is being destroyed.
    /// Cleans up Harmony patches and resets the harmony instance.
    /// </summary>
    public void OnDestroy()
    {
        harmony?.UnpatchSelf();
        harmony = null;
    }

    /// <summary>
    /// Unity MonoBehaviour callback called every frame.
    /// Updates all cheat modules that require per-frame processing.
    /// </summary>
    public void Update()
    {
        Singleton<KeyBinder>.Instance.Update();
        Singleton<CheatMenuUI>.Instance.Update();
        Singleton<TriggerCheat>.Instance.Update();
    }

    /// <summary>
    /// Unity MonoBehaviour callback for rendering GUI elements.
    /// Renders all cheat modules that have GUI components (menu, ESP, etc.).
    /// </summary>
    public void OnGUI()
    {
        Singleton<CheatMenuUI>.Instance.OnGUI();
        Singleton<EspCheat>.Instance.OnGUI();
    }
}