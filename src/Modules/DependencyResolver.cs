using System.Reflection;
using UnityEngine;

namespace UnityCheatTemplate.Modules;

/// <summary>
/// Handles loading and resolving embedded assembly dependencies for the cheat system.
/// This class manages embedded dependencies stored as resources and resolves them at runtime.
/// </summary>
internal static class DependencyResolver
{
    /// <summary>
    /// Serializable data structure for storing dependency information loaded from JSON.
    /// </summary>
    [Serializable]
    internal class DependenciesData
    {
        /// <summary>
        /// List of dependency file names to be loaded.
        /// </summary>
        public List<string> Dependencies = [];
    }

    private static List<string>? EmbeddedDependencies;

    /// <summary>
    /// Initializes the dependency resolver and loads all embedded dependencies.
    /// </summary>
    /// <param name="callback">Action to execute after dependencies are successfully loaded.</param>
    internal static void Load(Action callback)
    {
        CheatLogger.Info($"Loading embedded dependencies", nameof(DependencyResolver));
        AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedAssembly;
        LoadDependenciesList(callback);
    }

    /// <summary>
    /// Unloads and cleans up the dependency resolver.
    /// </summary>
    internal static void Unload()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= ResolveEmbeddedAssembly;
        EmbeddedDependencies = null;
    }

    /// <summary>
    /// Loads the dependencies list from the embedded JSON resource.
    /// </summary>
    /// <param name="callback">Action to execute after the dependencies list is loaded.</param>
    private static void LoadDependenciesList(Action callback)
    {
        try
        {
            string resourceName = $"{nameof(UnityCheatTemplate)}.Resources.dependencies.json";
            using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                CheatLogger.Error($"Failed to find dependencies.json at: {resourceName}");
                return;
            }

            using StreamReader reader = new(stream);
            string jsonContent = reader.ReadToEnd();

            DependenciesData dependenciesData = JsonUtility.FromJson<DependenciesData>(jsonContent);

            if (dependenciesData != null && dependenciesData.Dependencies != null)
            {
                EmbeddedDependencies = dependenciesData.Dependencies;
                callback();
                CheatLogger.Info($"Loaded {EmbeddedDependencies.Count} dependencies from JSON");
            }
            else
            {
                CheatLogger.Error("Failed to parse dependencies.json");
            }
        }
        catch (Exception ex)
        {
            CheatLogger.Error($"Failed to load dependencies.json: {ex}");
        }
    }

    /// <summary>
    /// Resolves embedded assemblies when they are requested by the runtime.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">Event data containing information about the assembly being resolved.</param>
    /// <returns>The resolved assembly, or null if the assembly could not be found or loaded.</returns>
    private static Assembly? ResolveEmbeddedAssembly(object sender, ResolveEventArgs args)
    {
        if (EmbeddedDependencies == null) return null;

        AssemblyName assemblyName = new(args.Name);

        foreach (string dep in EmbeddedDependencies)
        {
            if (assemblyName.Name == Path.GetFileNameWithoutExtension(dep))
            {
                string resourceName = $"{nameof(UnityCheatTemplate)}.Resources.Dependencies.{dep}";
                using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);

                if (stream == null)
                {
                    CheatLogger.Error($"Failed to find embedded resource: {resourceName}");
                    return null;
                }

                try
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    return Assembly.Load(buffer);
                }
                catch (Exception ex)
                {
                    CheatLogger.Error($"Failed to load assembly {resourceName}: {ex}");
                    return null;
                }
            }
        }

        return null;
    }
}