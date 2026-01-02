using UnityEngine;

namespace UnityCheatTemplate.Utilities;

/// <summary>
/// Provides mathematical utility functions for 3D to 2D coordinate transformations and distance calculations.
/// </summary>
internal static class MathUtils
{
    /// <summary>
    /// Converts a world position to screen coordinates relative to the camera's view, with Y-axis inverted for GUI systems.
    /// </summary>
    /// <param name="camera">The camera from which to perform the transformation.</param>
    /// <param name="worldPosition">The position in world space to convert to screen coordinates.</param>
    /// <returns>
    internal static Vector3 WorldToEyesPoint(this Camera camera, Vector3 worldPosition)
    {
        Vector3 screen = camera.WorldToViewportPoint(worldPosition);
        screen.x *= Screen.width;
        screen.y *= Screen.height;
        screen.y = Screen.height - screen.y;

        return screen;
    }

    /// <summary>
    /// Converts a world space position to screen space coordinates.
    /// </summary>
    /// <param name="camera">The camera used for the perspective transformation.</param>
    /// <param name="world">The world space position to convert.</param>
    /// <param name="screen">The resulting screen space coordinates (in pixels).</param>
    /// <returns>True if the world point is in front of the camera (visible); false otherwise.</returns>
    internal static bool WorldToScreen(Camera camera, Vector3 world, out Vector3 screen)
    {
        screen = camera.WorldToViewportPoint(world);
        screen.x *= Screen.width;
        screen.y *= Screen.height;
        screen.y = Screen.height - screen.y;
        return screen.z > 0f;
    }

    /// <summary>
    /// Calculates the rounded distance between two 3D points.
    /// </summary>
    /// <param name="pos1">The first 3D point.</param>
    /// <param name="pos2">The second 3D point.</param>
    /// <returns>The distance between the two points, rounded to the nearest whole number.</returns>
    internal static float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return (float)Math.Round((double)Vector3.Distance(pos1, pos2));
    }
}