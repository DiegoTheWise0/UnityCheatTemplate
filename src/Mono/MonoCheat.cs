#pragma warning disable CS8618

using HarmonyLib;
using UnityCheatTemplate.Modules;
using UnityCheatTemplate.Modules.Menu.Core;
using UnityEngine;

namespace UnityCheatTemplate.Mono;

internal class MonoCheat : MonoBehaviour
{
    internal static MonoCheat Instance { get; private set; }
    internal static Harmony? harmony;

    internal static void Load()
    {
        var obj = new GameObject($"{CheatInfo.Name.Trim()}{nameof(MonoCheat)}");
        var monoCheat = obj.AddComponent<MonoCheat>();
        DontDestroyOnLoad(monoCheat);
    }

    internal static void Unload()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance.gameObject);
        }
    }

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

    public void OnDestroy()
    {
        harmony?.UnpatchSelf();
        harmony = null;
    }

    public void Update()
    {
        Singleton<CheatMenuUI>.Instance.Update();
    }

    public void OnGUI()
    {
        Singleton<CheatMenuUI>.Instance.OnGUI();
    }
}