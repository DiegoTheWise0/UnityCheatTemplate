#pragma warning disable CS8618
#pragma warning disable CS8602

using UnityCheatTemplate.Interfaces;
using UnityEngine;

namespace UnityCheatTemplate.Modules;

/// <summary>
/// Manages key bindings and keyboard input actions for the cheat system.
/// Allows binding keyboard keys to actions with optional modifier key requirements.
/// Implements both ILoadable and ISingleton interfaces for managed lifecycle and singleton access.
/// </summary>
internal class KeyBinder : ILoadable, ISingleton
{
    /// <summary>
    /// Represents a key binding with its configuration
    /// </summary>
    internal class Binding
    {
        public KeyCode PrimaryKey { get; set; }
        public KeyCode[] ModifierKeys { get; set; } = [];
        public Action Action { get; set; }
    }

    private readonly Dictionary<string, Binding> _bindings = [];
    private Binding? _listeningForNewKey;

    /// <summary>
    /// Initializes the key binder system.
    /// </summary>
    public void Load()
    {
    }

    /// <summary>
    /// Unloads and clears all key bindings.
    /// </summary>
    public void Unload()
    {
        _bindings.Clear();
        _listeningForNewKey = null;
    }

    /// <summary>
    /// Binds a keyboard key to an action with optional hold/modifier key requirements.
    /// </summary>
    /// <param name="inputName">A unique name identifier for this binding.</param>
    /// <param name="key">The primary key to trigger the action.</param>
    /// <param name="actionGetter">The action to execute when the key combination is pressed.</param>
    /// <param name="hold">Optional modifier keys that must be held down for the action to trigger.</param>
    internal Binding Bind(string inputName, KeyCode key, Func<Action> actionGetter, params KeyCode[] hold)
    {
        Unbind(inputName);

        var bind = new Binding
        {
            PrimaryKey = key,
            ModifierKeys = hold ?? [],
            Action = () =>
            {
                try
                {
                    actionGetter()?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error in key binding '{inputName}': {ex.Message}");
                }
            },
        };
        _bindings[inputName] = bind;
        return bind;
    }

    /// <summary>
    /// Removes a key binding by its unique name identifier.
    /// </summary>
    /// <param name="inputName">The unique name identifier of the binding to remove.</param>
    /// <returns>True if the binding was successfully removed; false if no binding was found with that name.</returns>
    internal bool Unbind(string inputName)
    {
        return _bindings.Remove(inputName);
    }

    /// <summary>
    /// Starts listening for a new key to assign to a binding
    /// </summary>
    /// <param name="binding">The binding to update with the new key</param>
    internal void StartListeningForNewKey(Binding binding)
    {
        _listeningForNewKey = binding;
    }

    /// <summary>
    /// Starts listening for a new key for a binding by name
    /// </summary>
    /// <param name="bindingName">Name of the binding to update</param>
    /// <returns>True if binding was found and listening started, false otherwise</returns>
    internal bool StartListeningForNewKey(string bindingName)
    {
        if (_bindings.TryGetValue(bindingName, out var binding))
        {
            StartListeningForNewKey(binding);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Stops listening for new key input
    /// </summary>
    internal void StopListeningForNewKey()
    {
        _listeningForNewKey = null;
    }

    /// <summary>
    /// Updates the key binder system and checks for triggered key bindings.
    /// Should be called every frame from a MonoBehaviour Update() method.
    /// </summary>
    internal void Update()
    {
        // Handle key listening mode
        if (_listeningForNewKey != null)
        {
            HandleKeyListening();
            return; // Don't process regular bindings while listening
        }

        // Process regular bindings
        ProcessBindings();
    }

    private void HandleKeyListening()
    {
        // Check for escape to cancel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopListeningForNewKey();
            return;
        }

        // Check all keys
        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                _listeningForNewKey.PrimaryKey = keyCode;
                StopListeningForNewKey();
                return;
            }
        }
    }

    private void ProcessBindings()
    {
        foreach (var binding in _bindings.Values)
        {
            if (Input.GetKeyDown(binding.PrimaryKey))
            {
                bool allModifiersPressed = true;
                foreach (var modifier in binding.ModifierKeys)
                {
                    if (!Input.GetKey(modifier))
                    {
                        allModifiersPressed = false;
                        break;
                    }
                }

                if (allModifiersPressed)
                {
                    binding.Action();
                }
            }
        }
    }

    /// <summary>
    /// Gets a binding by name
    /// </summary>
    internal Binding? GetBinding(string inputName)
    {
        _bindings.TryGetValue(inputName, out var binding);
        return binding;
    }
}