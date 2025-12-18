using System.Collections;
using System.Collections.Concurrent;
using System.Reflection;

namespace UnityCheatTemplate.Extensions;

/// <summary>
/// Provides reflection-based extension methods for accessing private and internal members of objects and types.
/// This class uses caching to improve performance for repeated reflection operations.
/// </summary>
internal static class ReflectExtension
{
    private const BindingFlags NonPublicFlags = BindingFlags.NonPublic | BindingFlags.Instance;
    private const BindingFlags NonPublicStaticFlags = BindingFlags.NonPublic | BindingFlags.Static;

    // Caches with tuple keys for better performance and type safety
    private static readonly ConcurrentDictionary<(Type, string), FieldInfo> _fieldCache = new();
    private static readonly ConcurrentDictionary<(Type, string), PropertyInfo> _propertyCache = new();
    private static readonly ConcurrentDictionary<(Type, string, Type[]?), MethodInfo> _methodCache = new();
    private static readonly ConcurrentDictionary<(Type, string), Type> _nestedTypeCache = new();

    /// <summary>
    /// Gets the value of a non-public instance field from an object.
    /// </summary>
    /// <typeparam name="T">The type to cast the field value to.</typeparam>
    /// <param name="obj">The object instance containing the field.</param>
    /// <param name="name">The name of the field to access.</param>
    /// <returns>The field value cast to type T, or default(T) if the object is null or the cast fails.</returns>
    /// <exception cref="MissingFieldException">Thrown when the field is not found.</exception>
    internal static T? GetField<T>(this object obj, string name)
    {
        if (obj == null) return default;

        var field = GetFieldInfo(obj.GetType(), name, isStatic: false);
        var value = field.GetValue(obj);

        return value is T typedValue ? typedValue : default;
    }

    /// <summary>
    /// Sets the value of a non-public instance field on an object.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="obj">The object instance containing the field.</param>
    /// <param name="name">The name of the field to set.</param>
    /// <param name="value">The value to set the field to.</param>
    /// <exception cref="MissingFieldException">Thrown when the field is not found.</exception>
    internal static void SetField<T>(this object obj, string name, T value)
    {
        if (obj == null) return;

        var field = GetFieldInfo(obj.GetType(), name, isStatic: false);
        field.SetValue(obj, value);
    }

    /// <summary>
    /// Gets the value of a non-public static field from a type.
    /// </summary>
    /// <typeparam name="T">The type to cast the field value to.</typeparam>
    /// <param name="type">The type containing the static field.</param>
    /// <param name="name">The name of the static field to access.</param>
    /// <returns>The field value cast to type T.</returns>
    /// <exception cref="MissingFieldException">Thrown when the field is not found.</exception>
    /// <exception cref="InvalidCastException">Thrown when the field value cannot be cast to type T.</exception>
    internal static T GetStaticField<T>(this Type type, string name)
    {
        var field = GetFieldInfo(type, name, isStatic: true);
        var value = field.GetValue(null);

        if (value is T typedValue) return typedValue;
        throw new InvalidCastException($"Cannot cast {value?.GetType().Name ?? "null"} to {typeof(T).Name}");
    }

    /// <summary>
    /// Sets the value of a non-public static field on a type.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="type">The type containing the static field.</param>
    /// <param name="name">The name of the static field to set.</param>
    /// <param name="value">The value to set the field to.</param>
    /// <exception cref="MissingFieldException">Thrown when the field is not found.</exception>
    internal static void SetStaticField<T>(this Type type, string name, T value)
    {
        var field = GetFieldInfo(type, name, isStatic: true);
        field.SetValue(null, value);
    }

    /// <summary>
    /// Gets the FieldInfo for a field with caching.
    /// </summary>
    /// <param name="type">The type containing the field.</param>
    /// <param name="name">The name of the field.</param>
    /// <param name="isStatic">Whether the field is static.</param>
    /// <returns>The FieldInfo object for the specified field.</returns>
    /// <exception cref="MissingFieldException">Thrown when the field is not found.</exception>
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

    /// <summary>
    /// Gets the value of a non-public instance property from an object.
    /// </summary>
    /// <typeparam name="T">The type to cast the property value to.</typeparam>
    /// <param name="obj">The object instance containing the property.</param>
    /// <param name="name">The name of the property to access.</param>
    /// <returns>The property value cast to type T, or default(T) if the object is null or the cast fails.</returns>
    /// <exception cref="MissingMemberException">Thrown when the property is not found.</exception>
    internal static T? GetProperty<T>(this object obj, string name)
    {
        if (obj == null) return default;

        var property = GetPropertyInfo(obj.GetType(), name, isStatic: false);
        var value = property.GetValue(obj);

        return value is T typedValue ? typedValue : default;
    }

    /// <summary>
    /// Sets the value of a non-public instance property on an object.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="obj">The object instance containing the property.</param>
    /// <param name="name">The name of the property to set.</param>
    /// <param name="value">The value to set the property to.</param>
    /// <exception cref="MissingMemberException">Thrown when the property is not found.</exception>
    internal static void SetProperty<T>(this object obj, string name, T value)
    {
        if (obj == null) return;

        var property = GetPropertyInfo(obj.GetType(), name, isStatic: false);
        property.SetValue(obj, value);
    }

    /// <summary>
    /// Gets the value of a non-public static property from a type.
    /// </summary>
    /// <typeparam name="T">The type to cast the property value to.</typeparam>
    /// <param name="type">The type containing the static property.</param>
    /// <param name="name">The name of the static property to access.</param>
    /// <returns>The property value cast to type T.</returns>
    /// <exception cref="MissingMemberException">Thrown when the property is not found.</exception>
    /// <exception cref="InvalidCastException">Thrown when the property value cannot be cast to type T.</exception>
    internal static T GetStaticProperty<T>(this Type type, string name)
    {
        var property = GetPropertyInfo(type, name, isStatic: true);
        var value = property.GetValue(null);

        if (value is T typedValue) return typedValue;
        throw new InvalidCastException($"Cannot cast {value?.GetType().Name ?? "null"} to {typeof(T).Name}");
    }

    /// <summary>
    /// Sets the value of a non-public static property on a type.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="type">The type containing the static property.</param>
    /// <param name="name">The name of the static property to set.</param>
    /// <param name="value">The value to set the property to.</param>
    /// <exception cref="MissingMemberException">Thrown when the property is not found.</exception>
    internal static void SetStaticProperty<T>(this Type type, string name, T value)
    {
        var property = GetPropertyInfo(type, name, isStatic: true);
        property.SetValue(null, value);
    }

    /// <summary>
    /// Gets the PropertyInfo for a property with caching.
    /// </summary>
    /// <param name="type">The type containing the property.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="isStatic">Whether the property is static.</param>
    /// <returns>The PropertyInfo object for the specified property.</returns>
    /// <exception cref="MissingMemberException">Thrown when the property is not found.</exception>
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

    /// <summary>
    /// Invokes a non-public instance method that returns an IEnumerator (coroutine).
    /// </summary>
    /// <param name="obj">The object instance containing the method.</param>
    /// <param name="name">The name of the method to invoke.</param>
    /// <param name="parameters">Parameters to pass to the method.</param>
    /// <returns>An IEnumerator for the coroutine, or null if the object is null or the method doesn't return IEnumerator.</returns>
    /// <exception cref="MissingMethodException">Thrown when the method is not found.</exception>
    internal static IEnumerator? GetCoroutine(this object obj, string name, params object[] parameters)
    {
        if (obj == null) return null;

        var result = obj.InvokeMethod(name, parameters);
        return result as IEnumerator;
    }

    /// <summary>
    /// Invokes a non-public static method that returns an IEnumerator (coroutine).
    /// </summary>
    /// <param name="type">The type containing the static method.</param>
    /// <param name="name">The name of the static method to invoke.</param>
    /// <param name="parameters">Parameters to pass to the method.</param>
    /// <returns>An IEnumerator for the coroutine, or null if the method doesn't return IEnumerator.</returns>
    /// <exception cref="MissingMethodException">Thrown when the method is not found.</exception>
    internal static IEnumerator? GetStaticCoroutine(this Type type, string name, params object[] parameters)
    {
        var result = type.InvokeStaticMethod(name, parameters);
        return result as IEnumerator;
    }

    /// <summary>
    /// Invokes a non-public instance method on an object.
    /// </summary>
    /// <param name="obj">The object instance containing the method.</param>
    /// <param name="name">The name of the method to invoke.</param>
    /// <param name="parameters">Parameters to pass to the method.</param>
    /// <returns>The return value of the method, or null if the object is null.</returns>
    /// <exception cref="MissingMethodException">Thrown when the method is not found.</exception>
    internal static object? InvokeMethod(this object obj, string name, params object[] parameters)
    {
        if (obj == null) return null;

        var paramTypes = GetParameterTypes(parameters);
        var method = GetMethodInfo(obj.GetType(), name, paramTypes, isStatic: false);

        return method.Invoke(obj, parameters);
    }

    /// <summary>
    /// Invokes a non-public static method on a type.
    /// </summary>
    /// <param name="type">The type containing the static method.</param>
    /// <param name="name">The name of the static method to invoke.</param>
    /// <param name="parameters">Parameters to pass to the method.</param>
    /// <returns>The return value of the method.</returns>
    /// <exception cref="MissingMethodException">Thrown when the method is not found.</exception>
    internal static object? InvokeStaticMethod(this Type type, string name, params object[] parameters)
    {
        var paramTypes = GetParameterTypes(parameters);
        var method = GetMethodInfo(type, name, paramTypes, isStatic: true);

        return method.Invoke(null, parameters);
    }

    /// <summary>
    /// Gets the parameter types from an array of parameter values.
    /// </summary>
    /// <param name="parameters">The parameter values.</param>
    /// <returns>An array of parameter types.</returns>
    private static Type[] GetParameterTypes(object[] parameters)
    {
        if (parameters == null || parameters.Length == 0)
            return Type.EmptyTypes;

        return parameters.Select(p => p?.GetType() ?? typeof(object)).ToArray();
    }

    /// <summary>
    /// Gets the MethodInfo for a method with caching.
    /// </summary>
    /// <param name="type">The type containing the method.</param>
    /// <param name="name">The name of the method.</param>
    /// <param name="paramTypes">The parameter types of the method.</param>
    /// <param name="isStatic">Whether the method is static.</param>
    /// <returns>The MethodInfo object for the specified method.</returns>
    /// <exception cref="MissingMethodException">Thrown when the method is not found.</exception>
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

    /// <summary>
    /// Gets a non-public nested type from a type.
    /// </summary>
    /// <param name="type">The outer type containing the nested type.</param>
    /// <param name="name">The name of the nested type.</param>
    /// <returns>The nested Type object.</returns>
    /// <exception cref="TypeLoadException">Thrown when the nested type is not found.</exception>
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

    /// <summary>
    /// Invokes a non-public instance method with a typed return value.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="obj">The object instance containing the method.</param>
    /// <param name="name">The name of the method to invoke.</param>
    /// <param name="parameters">Parameters to pass to the method.</param>
    /// <returns>The return value cast to type T, or default(T) if the cast fails.</returns>
    internal static T? InvokeMethod<T>(this object obj, string name, params object[] parameters)
    {
        var result = obj.InvokeMethod(name, parameters);
        return result is T typedResult ? typedResult : default;
    }

    /// <summary>
    /// Invokes a non-public static method with a typed return value.
    /// </summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="type">The type containing the static method.</param>
    /// <param name="name">The name of the static method to invoke.</param>
    /// <param name="parameters">Parameters to pass to the method.</param>
    /// <returns>The return value cast to type T.</returns>
    /// <exception cref="InvalidCastException">Thrown when the return value cannot be cast to type T.</exception>
    internal static T InvokeStaticMethod<T>(this Type type, string name, params object[] parameters)
    {
        var result = type.InvokeStaticMethod(name, parameters);
        if (result is T typedResult) return typedResult;
        throw new InvalidCastException($"Cannot cast result to {typeof(T).Name}");
    }

    /// <summary>
    /// Clears all reflection caches.
    /// Use this method if types may have been reloaded (e.g., in dynamic assemblies).
    /// </summary>
    public static void ClearCaches()
    {
        _fieldCache.Clear();
        _propertyCache.Clear();
        _methodCache.Clear();
        _nestedTypeCache.Clear();
    }
}