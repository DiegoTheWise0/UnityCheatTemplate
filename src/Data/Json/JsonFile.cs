using UnityEngine;

namespace UnityCheatTemplate.Data.Json;

internal abstract class JsonFile
{
    internal abstract string FilePath { get; }
    internal abstract string FileName { get; }
    internal string FullPath => Path.Combine(FilePath, FileName);
}

internal abstract class JsonFile<T> : JsonFile where T : JsonFile
{
    internal void Load(Action<T>? callback = null)
    {
        if (File.Exists(FullPath))
        {
            string json = File.ReadAllText(FullPath);
            T data = JsonUtility.FromJson<T>(json);
            callback?.Invoke(data);
        }
        else
        {
            Save();
        }
    }

    internal void Save()
    {
        string json = JsonUtility.ToJson(this, true);
        File.WriteAllText(FullPath, json);
    }
}
