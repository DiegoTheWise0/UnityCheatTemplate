#pragma warning disable CS8602

using System.Collections.ObjectModel;
using UnityEngine;

namespace UnityCheatTemplate.Modules;

/// <summary>
/// Provides a registry system for tracking all instances of a specific MonoBehaviour type.
/// This generic class maintains a centralized collection of MonoBehaviour instances for easy access and management.
/// </summary>
/// <typeparam name="T">The type of MonoBehaviour to track. Must be a MonoBehaviour-derived class with a parameterless constructor.</typeparam>
internal static class CatchMono<T> where T : MonoBehaviour, new()
{
    /// <summary>
    /// Gets a read-only collection containing all registered instances of type T.
    /// </summary>
    /// <value>A read-only collection of all currently registered MonoBehaviour instances of type T.</value>
    internal static ReadOnlyCollection<T> AllMonos = _allMonos.AsReadOnly();

    private readonly static List<T> _allMonos = [];

    /// <summary>
    /// Registers a MonoBehaviour instance to the tracking system.
    /// </summary>
    /// <param name="mono">The MonoBehaviour instance to register.</param>
    internal static void Register(T mono)
    {
        if (!_allMonos.Contains(mono))
        {
            _allMonos.Add(mono);
        }
    }

    /// <summary>
    /// Unregisters a MonoBehaviour instance from the tracking system.
    /// </summary>
    /// <param name="mono">The MonoBehaviour instance to unregister.</param>
    internal static void Unregister(T mono)
    {
        _allMonos.Remove(mono);
    }
}