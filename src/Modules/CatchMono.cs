#pragma warning disable CS8602

using System.Collections.ObjectModel;
using UnityEngine;

namespace UnityCheatTemplate.Modules;

internal static class CatchMono<T> where T : MonoBehaviour, new()
{
    internal static ReadOnlyCollection<T> AllMonos = _allMonos.AsReadOnly();
    private readonly static List<T> _allMonos = [];

    internal static void Register(T mono)
    {
        if (!_allMonos.Contains(mono))
        {
            _allMonos.Add(mono);
        }
    }

    internal static void Unregister(T mono)
    {
        _allMonos.Remove(mono);
    }
}
