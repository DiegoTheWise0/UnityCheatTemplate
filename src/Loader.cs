using UnityCheatTemplate.Data;
using UnityCheatTemplate.Modules;
using UnityCheatTemplate.Modules.Menu.Core;
using UnityCheatTemplate.Mono;

namespace UnityCheatTemplate;

public static class Loader
{
    public static void Load()
    {
        CheatLogger.Load();
        DependencyResolver.Load(() =>
        {
            Singleton<DataManager>.Instance.Load();
            Singleton<CheatMenuUI>.Instance.Load();
            MonoCheat.Load();
        });
    }

    public static void Unload()
    {
        MonoCheat.Unload();
        Singleton<CheatMenuUI>.Instance.Unload();
        Singleton<CheatMenuUI>.Dispose();
        Singleton<DataManager>.Instance.Unload();
        Singleton<DataManager>.Dispose();
        DependencyResolver.Unload();
    }
}