using UnityCheatTemplate.Interfaces;

namespace UnityCheatTemplate.Modules;

internal class Singleton<T> where T : ISingleton, new()
{
    private static T? _instance;
    internal static T Instance
    {
        get
        {
            _instance ??= new();
            return _instance;
        }
    }

    internal static void SetInstance(T instance)
    {
        _instance = instance;
    }

    internal static void Dispose()
    {
        _instance = default;
    }
}
