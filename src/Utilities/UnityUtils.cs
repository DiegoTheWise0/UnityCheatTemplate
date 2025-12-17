using UnityEngine;

namespace UnityCheatTemplate.Utilities;

internal static class UnityUtils
{
    internal static bool WorldToScreen(Camera camera, Vector3 world, out Vector3 screen)
    {
        screen = camera.WorldToViewportPoint(world);
        screen.x *= Screen.width;
        screen.y *= Screen.height;
        screen.y = Screen.height - screen.y;
        return screen.z > 0f;
    }

    internal static float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        return (float)Math.Round((double)Vector3.Distance(pos1, pos2));
    }
}
