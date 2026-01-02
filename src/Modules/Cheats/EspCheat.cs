using UnityCheatTemplate.Interfaces;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Cheats;

/// <summary>
/// Provides ESP rendering functionality for visualizing game objects in 3D space.
/// This class implements the singleton pattern to ensure only one instance exists throughout the application.
/// </summary>
internal sealed class EspCheat : ISingleton
{
    /// <summary>
    /// Called by Unity's GUI system to render GUI elements.
    /// This method should be called from OnGUI() in your main cheat window or renderer.
    /// </summary>
    internal void OnGUI()
    {
    }

    /// <summary>
    /// Renders an ESP element for a game object at the specified world position.
    /// </summary>
    /// <param name="worldPos">The world position of the object to render ESP for.</param>
    /// <param name="size">The size multipliers for the ESP box (x = width multiplier, y = height multiplier).</param>
    /// <param name="name">The name or label to display above the ESP element.</param>
    /// <param name="color">The color of the ESP element.</param>
    /// <param name="Range">The maximum render distance for the ESP element. Default is float.MaxValue (unlimited).</param>
    /// <param name="tracer">Whether to draw a tracer line from screen center to the ESP element.</param>
    internal void RenderEspElement(Vector3 worldPos, (float x, float y) size, string name, Color color, float Range = float.MaxValue, bool tracer = false)
    {
        var cam = Camera.main;
        Vector3 viewportPos = cam.WorldToViewportPoint(worldPos);

        float distance = Mathf.Round(Vector3.Distance(worldPos, cam.transform.parent.position));
        if (viewportPos.z < 0 || distance > Range) return;

        Vector2 screenPos = new(
            viewportPos.x * Screen.width,
            (1 - viewportPos.y) * Screen.height
        );

        float baseBoxSize = Mathf.Clamp(1000f / distance, 10f, 50f);

        float boxWidth = baseBoxSize * size.x;
        float boxHeight = baseBoxSize * size.y;

        bool isInView = viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <= 1;

        if (isInView)
        {
            Vector2 boxTopLeft = new(
                screenPos.x - boxWidth * 0.5f,
                screenPos.y - boxHeight * 0.5f
            );

            Render.DrawBox(
                boxTopLeft,
                new Vector2(boxWidth, boxHeight),
                2f,
                color
            );

            if (!string.IsNullOrEmpty(name))
            {
                Render.DrawLabel(
                    GUI.skin.label,
                    new Vector2(screenPos.x, screenPos.y - boxHeight * 0.5f - 20f),
                    name,
                    Color.white
                );
            }
        }

        if (tracer)
        {
            Vector2 screenCenter = new(Screen.width / 2f, Screen.height / 2f);
            Vector2 tracerEndPos = isInView ? screenPos : new Vector2(
                Mathf.Clamp(viewportPos.x * Screen.width, 0, Screen.width),
                Mathf.Clamp((1 - viewportPos.y) * Screen.height, 0, Screen.height)
            );
            Render.DrawLine(screenCenter, tracerEndPos, 1f, color);
        }
    }
}