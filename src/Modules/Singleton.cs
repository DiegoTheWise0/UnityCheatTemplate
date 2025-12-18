using UnityCheatTemplate.Interfaces;

namespace UnityCheatTemplate.Modules;

/// <summary>
/// Provides a generic singleton wrapper for classes implementing the <see cref="ISingleton"/> interface.
/// This class ensures that only one instance of type T exists and provides global access to it.
/// </summary>
/// <typeparam name="T">The type of the singleton class, which must implement <see cref="ISingleton"/> and have a parameterless constructor.</typeparam>
internal class Singleton<T> where T : ISingleton, new()
{
    private static T? _instance;

    /// <summary>
    /// Gets the singleton instance of type T.
    /// Creates a new instance on first access if one doesn't already exist.
    /// </summary>
    /// <value>The single instance of type T.</value>
    internal static T Instance
    {
        get
        {
            _instance ??= new();
            return _instance;
        }
    }

    /// <summary>
    /// Sets a custom instance as the singleton instance.
    /// This can be used for dependency injection or testing scenarios.
    /// </summary>
    /// <param name="instance">The instance to set as the singleton.</param>
    internal static void SetInstance(T instance)
    {
        _instance = instance;
    }

    /// <summary>
    /// Disposes of the current singleton instance, allowing a new instance to be created on next access.
    /// This method clears the reference to the current instance without calling any cleanup methods on it.
    /// </summary>
    internal static void Dispose()
    {
        _instance = default;
    }
}