using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace UnityCheatTemplate.Extensions;

internal static class ReflectExtension
{
    private const BindingFlags NonPublicFlags = BindingFlags.NonPublic | BindingFlags.Instance;
    private const BindingFlags NonPublicStaticFlags = BindingFlags.NonPublic | BindingFlags.Static;

    // Caches with tuple keys for better performance and type safety
    private static readonly ConcurrentDictionary<(Type, string), FieldInfo> _fieldCache = new();
    private static readonly ConcurrentDictionary<(Type, string), PropertyInfo> _propertyCache = new();
    private static readonly ConcurrentDictionary<(Type, string, Type[]?), MethodInfo> _methodCache = new();
    private static readonly ConcurrentDictionary<(Type, string), Type> _nestedTypeCache = new();

    // ==== Field Operations (Cached) ====
    internal static T? GetField<T>(this object obj, string name)
    {
        if (obj == null) return default;

        var field = GetFieldInfo(obj.GetType(), name, isStatic: false);
        var value = field.GetValue(obj);

        return value is T typedValue ? typedValue : default;
    }

    internal static void SetField<T>(this object obj, string name, T value)
    {
        if (obj == null) return;

        var field = GetFieldInfo(obj.GetType(), name, isStatic: false);
        field.SetValue(obj, value);
    }

    // ==== Static Field Operations (Cached) ====
    internal static T GetStaticField<T>(this Type type, string name)
    {
        var field = GetFieldInfo(type, name, isStatic: true);
        var value = field.GetValue(null);

        if (value is T typedValue) return typedValue;
        throw new InvalidCastException($"Cannot cast {value?.GetType().Name ?? "null"} to {typeof(T).Name}");
    }

    internal static void SetStaticField<T>(this Type type, string name, T value)
    {
        var field = GetFieldInfo(type, name, isStatic: true);
        field.SetValue(null, value);
    }

    private static FieldInfo GetFieldInfo(Type type, string name, bool isStatic)
    {
        var cacheKey = isStatic ? $"static:{name}" : name;
        var key = (type, cacheKey);

        return _fieldCache.GetOrAdd(key, k =>
        {
            var flags = isStatic ? NonPublicStaticFlags : NonPublicFlags;
            var field = k.Item1.GetField(isStatic ? name : k.Item2, flags);

            return field ?? throw new MissingFieldException(
                $"{(isStatic ? "Static" : "")} field '{name}' not found in {k.Item1.Name}");
        });
    }

    // ==== Property Operations (Cached) ====
    internal static T? GetProperty<T>(this object obj, string name)
    {
        if (obj == null) return default;

        var property = GetPropertyInfo(obj.GetType(), name, isStatic: false);
        var value = property.GetValue(obj);

        return value is T typedValue ? typedValue : default;
    }

    internal static void SetProperty<T>(this object obj, string name, T value)
    {
        if (obj == null) return;

        var property = GetPropertyInfo(obj.GetType(), name, isStatic: false);
        property.SetValue(obj, value);
    }

    // ==== Static Property Operations (Cached) ====
    internal static T GetStaticProperty<T>(this Type type, string name)
    {
        var property = GetPropertyInfo(type, name, isStatic: true);
        var value = property.GetValue(null);

        if (value is T typedValue) return typedValue;
        throw new InvalidCastException($"Cannot cast {value?.GetType().Name ?? "null"} to {typeof(T).Name}");
    }

    internal static void SetStaticProperty<T>(this Type type, string name, T value)
    {
        var property = GetPropertyInfo(type, name, isStatic: true);
        property.SetValue(null, value);
    }

    private static PropertyInfo GetPropertyInfo(Type type, string name, bool isStatic)
    {
        var cacheKey = isStatic ? $"static:{name}" : name;
        var key = (type, cacheKey);

        return _propertyCache.GetOrAdd(key, k =>
        {
            var flags = isStatic ? NonPublicStaticFlags : NonPublicFlags;
            var property = k.Item1.GetProperty(isStatic ? name : k.Item2, flags);

            return property ?? throw new MissingMemberException(
                $"{(isStatic ? "Static" : "")} property '{name}' not found in {k.Item1.Name}");
        });
    }

    // ==== Coroutine Helpers ====
    internal static IEnumerator? GetCoroutine(this object obj, string name, params object[] parameters)
    {
        if (obj == null) return null;

        var result = obj.InvokeMethod(name, parameters);
        return result as IEnumerator;
    }

    internal static IEnumerator? GetStaticCoroutine(this Type type, string name, params object[] parameters)
    {
        var result = type.InvokeStaticMethod(name, parameters);
        return result as IEnumerator;
    }

    // ==== Method Operations (Cached) ====
    internal static object? InvokeMethod(this object obj, string name, params object[] parameters)
    {
        if (obj == null) return null;

        var paramTypes = GetParameterTypes(parameters);
        var method = GetMethodInfo(obj.GetType(), name, paramTypes, isStatic: false);

        return method.Invoke(obj, parameters);
    }

    internal static object? InvokeStaticMethod(this Type type, string name, params object[] parameters)
    {
        var paramTypes = GetParameterTypes(parameters);
        var method = GetMethodInfo(type, name, paramTypes, isStatic: true);

        return method.Invoke(null, parameters);
    }

    private static Type[] GetParameterTypes(object[] parameters)
    {
        if (parameters == null || parameters.Length == 0)
            return Type.EmptyTypes;

        return parameters.Select(p => p?.GetType() ?? typeof(object)).ToArray();
    }

    private static MethodInfo GetMethodInfo(Type type, string name, Type[] paramTypes, bool isStatic)
    {
        var key = (type, name, paramTypes.Length > 0 ? paramTypes : null);

        return _methodCache.GetOrAdd(key, k =>
        {
            var flags = isStatic ? NonPublicStaticFlags : NonPublicFlags;
            var method = k.Item1.GetMethod(k.Item2, flags, null, k.Item3 ?? Type.EmptyTypes, null);

            return method ?? throw new MissingMethodException(
                $"{(isStatic ? "Static" : "")} method '{k.Item2}' not found in {k.Item1.Name}");
        });
    }

    // ==== Nested Type Lookup (Cached) ====
    internal static Type GetNestedType(this Type type, string name)
    {
        var key = (type, name);

        return _nestedTypeCache.GetOrAdd(key, k =>
        {
            var nestedType = k.Item1.GetNestedType(k.Item2, BindingFlags.NonPublic);
            return nestedType ?? throw new TypeLoadException(
                $"Nested type '{k.Item2}' not found in {k.Item1.Name}");
        });
    }

    // Optional: Generic method invocation with type safety
    internal static T? InvokeMethod<T>(this object obj, string name, params object[] parameters)
    {
        var result = obj.InvokeMethod(name, parameters);
        return result is T typedResult ? typedResult : default;
    }

    internal static T InvokeStaticMethod<T>(this Type type, string name, params object[] parameters)
    {
        var result = type.InvokeStaticMethod(name, parameters);
        if (result is T typedResult) return typedResult;
        throw new InvalidCastException($"Cannot cast result to {typeof(T).Name}");
    }

    // ==== Utility Methods ====
    public static void ClearCaches()
    {
        _fieldCache.Clear();
        _propertyCache.Clear();
        _methodCache.Clear();
        _nestedTypeCache.Clear();
    }
}