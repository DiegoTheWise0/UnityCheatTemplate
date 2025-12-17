using System.Reflection;
using UnityEngine;

namespace UnityCheatTemplate.Modules;
internal static class DependencyResolver
{
    [Serializable]
    internal class DependenciesData
    {
        public List<string> Dependencies = [];
    }

    private static List<string>? EmbeddedDependencies;

    internal static void Load(Action callback)
    {
        AppDomain.CurrentDomain.AssemblyResolve += ResolveEmbeddedAssembly;
        LoadDependenciesList(callback);
    }

    internal static void Unload()
    {
        AppDomain.CurrentDomain.AssemblyResolve -= ResolveEmbeddedAssembly;
        EmbeddedDependencies = null;
    }

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