using UnityEngine;

namespace UnityCheatTemplate.Data.Json;

/// <summary>
/// Abstract base class for JSON file management.
/// Provides basic file path and name properties for JSON serialization.
/// </summary>
internal abstract class JsonFile
{
    /// <summary>
    /// Gets the directory path where the JSON file is stored.
    /// </summary>
    internal abstract string FilePath { get; }

    /// <summary>
    /// Gets the name of the JSON file (including extension).
    /// </summary>
    internal abstract string FileName { get; }

    /// <summary>
    /// Gets the full path to the JSON file (combination of FilePath and FileName).
    /// </summary>
    internal string FullPath => Path.Combine(FilePath, FileName);
}

/// <summary>
/// Generic abstract base class for strongly-typed JSON file management.
/// Provides load and save functionality using Unity's JsonUtility.
/// </summary>
/// <typeparam name="T">The specific JsonFile type that inherits from this class.</typeparam>
internal abstract class JsonFile<T> : JsonFile where T : JsonFile
{
    /// <summary>
    /// Loads JSON data from the file system and deserializes it into an instance of type T.
    /// If the file doesn't exist, creates it by saving the current instance.
    /// </summary>
    /// <param name="callback">Optional callback to receive the loaded data.</param>
    internal void Load(Action<T>? callback = null)
    {
        if (File.Exists(FullPath))
        {
            string json = File.ReadAllText(FullPath);
            T data = JsonUtility.FromJson<T>(json);
            if (data != null)
            {
                callback?.Invoke(data);
            }
        }
        else
        {
            Save();
        }
    }

    /// <summary>
    /// Serializes the current instance to JSON and saves it to the file system.
    /// </summary>
    internal void Save()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(FullPath, json);
    }
}