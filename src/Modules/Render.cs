using UnityEngine;

namespace UnityCheatTemplate.Modules;

/// <summary>
/// Provides utility methods for rendering 2D graphics and UI elements in Unity's GUI system.
/// This static class includes methods for drawing lines, boxes, text, circles, and other shapes.
/// </summary>
internal static class Render
{
    /// <summary>
    /// Helper class for generating circle point positions.
    /// </summary>
    private class RingArray
    {
        /// <summary>
        /// Gets the array of pre-calculated circle point positions.
        /// </summary>
        internal Vector2[] Positions { get; private set; }

        /// <summary>
        /// Initializes a new RingArray with the specified number of segments.
        /// </summary>
        /// <param name="numSegments">The number of segments to divide the circle into.</param>
        internal RingArray(int numSegments)
        {
            Positions = new Vector2[numSegments];
            var stepSize = 360f / numSegments;
            for (int i = 0; i < numSegments; i++)
            {
                var rad = Mathf.Deg2Rad * stepSize * i;
                Positions[i] = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
            }
        }
    }

    /// <summary>
    /// Gets or sets the current GUI drawing color.
    /// </summary>
    internal static Color Color
    {
        get { return GUI.color; }
        set { GUI.color = value; }
    }

    /// <summary>
    /// Draws a line from one point to another with specified thickness and color.
    /// </summary>
    /// <param name="from">The starting point of the line.</param>
    /// <param name="to">The ending point of the line.</param>
    /// <param name="thickness">The thickness of the line in pixels.</param>
    /// <param name="color">The color of the line.</param>
    internal static void Line(Vector2 from, Vector2 to, float thickness, Color color)
    {
        Color = color;
        Line(from, to, thickness);
    }

    /// <summary>
    /// Draws a line from one point to another with specified thickness using the current color.
    /// </summary>
    /// <param name="from">The starting point of the line.</param>
    /// <param name="to">The ending point of the line.</param>
    /// <param name="thickness">The thickness of the line in pixels.</param>
    internal static void Line(Vector2 from, Vector2 to, float thickness)
    {
        var delta = (to - from).normalized;
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        GUIUtility.RotateAroundPivot(angle, from);
        Box(from, Vector2.right * (from - to).magnitude, thickness, false);
        GUIUtility.RotateAroundPivot(-angle, from);
    }

    /// <summary>
    /// Draws a rectangular box with specified position, size, thickness, and color.
    /// </summary>
    /// <param name="position">The position of the box.</param>
    /// <param name="size">The size of the box.</param>
    /// <param name="thickness">The thickness of the box border in pixels.</param>
    /// <param name="color">The color of the box.</param>
    /// <param name="centered">Whether the position represents the center of the box (true) or the top-left corner (false).</param>
    internal static void Box(Vector2 position, Vector2 size, float thickness, Color color, bool centered = true)
    {
        Color = color;
        Box(position, size, thickness, centered);
    }

    /// <summary>
    /// Draws a rectangular box with specified position, size, and thickness using the current color.
    /// </summary>
    /// <param name="position">The position of the box.</param>
    /// <param name="size">The size of the box.</param>
    /// <param name="thickness">The thickness of the box border in pixels.</param>
    /// <param name="centered">Whether the position represents the center of the box (true) or the top-left corner (false).</param>
    internal static void Box(Vector2 position, Vector2 size, float thickness, bool centered = true)
    {
        var upperLeft = centered ? position - size / 2f : position;
        GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness), Texture2D.whiteTexture);
    }

    /// <summary>
    /// Draws a cross shape (plus sign) with specified position, size, thickness, and color.
    /// </summary>
    /// <param name="position">The center position of the cross.</param>
    /// <param name="size">The size of each arm of the cross.</param>
    /// <param name="thickness">The thickness of the cross lines in pixels.</param>
    /// <param name="color">The color of the cross.</param>
    internal static void Cross(Vector2 position, Vector2 size, float thickness, Color color)
    {
        Color = color;
        Cross(position, size, thickness);
    }

    /// <summary>
    /// Draws a cross shape (plus sign) with specified position, size, and thickness using the current color.
    /// </summary>
    /// <param name="position">The center position of the cross.</param>
    /// <param name="size">The size of each arm of the cross.</param>
    /// <param name="thickness">The thickness of the cross lines in pixels.</param>
    internal static void Cross(Vector2 position, Vector2 size, float thickness)
    {
        GUI.DrawTexture(new Rect(position.x - size.x / 2f, position.y, size.x, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x, position.y - size.y / 2f, thickness, size.y), Texture2D.whiteTexture);
    }

    /// <summary>
    /// Draws a single pixel dot at the specified position with the specified color.
    /// </summary>
    /// <param name="position">The position to draw the dot.</param>
    /// <param name="color">The color of the dot.</param>
    internal static void Dot(Vector2 position, Color color)
    {
        Color = color;
        Dot(position);
    }

    /// <summary>
    /// Draws a single pixel dot at the specified position using the current color.
    /// </summary>
    /// <param name="position">The position to draw the dot.</param>
    internal static void Dot(Vector2 position)
    {
        Box(position - Vector2.one, Vector2.one * 2f, 1f);
    }

    /// <summary>
    /// Draws text with optional styling, positioning, and coloring options.
    /// </summary>
    /// <param name="Style">The GUIStyle to use for rendering the text.</param>
    /// <param name="X">The X coordinate position for the text.</param>
    /// <param name="Y">The Y coordinate position for the text.</param>
    /// <param name="W">The width of the text bounds.</param>
    /// <param name="H">The height of the text bounds.</param>
    /// <param name="str">The string text to render.</param>
    /// <param name="col">The color of the text.</param>
    /// <param name="centerx">Whether to center the text horizontally around the X position.</param>
    /// <param name="centery">Whether to center the text vertically around the Y position.</param>
    internal static void String(GUIStyle Style, float X, float Y, float W, float H, string str, Color col, bool centerx = false, bool centery = false)
    {
        GUIContent content = new GUIContent(str);

        Vector2 size = Style.CalcSize(content);
        float fX = centerx ? (X - size.x / 2f) : X,
            fY = centery ? (Y - size.y / 2f) : Y;

        Style.normal.textColor = Color.black;
        GUI.Label(new Rect(fX, fY, size.x, H), str, Style);

        Style.normal.textColor = col;
        GUI.Label(new Rect(fX + 1f, fY + 1f, size.x, H), str, Style);
    }

    /// <summary>
    /// Draws an outlined circle with specified center, radius, thickness, and color.
    /// </summary>
    /// <param name="center">The center position of the circle.</param>
    /// <param name="radius">The radius of the circle in pixels.</param>
    /// <param name="thickness">The thickness of the circle outline in pixels.</param>
    /// <param name="color">The color of the circle outline.</param>
    internal static void Circle(Vector2 center, float radius, float thickness, Color color)
    {
        Color = color;
        Vector2 previousPoint = center + new Vector2(radius, 0);

        for (int i = 1; i <= 360; i++)
        {
            float angle = i * Mathf.Deg2Rad;
            Vector2 nextPoint = center + new Vector2(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
            Line(previousPoint, nextPoint, thickness);
            previousPoint = nextPoint;
        }
    }

    /// <summary>
    /// Draws a filled circle with specified center, radius, and color.
    /// </summary>
    /// <param name="center">The center position of the circle.</param>
    /// <param name="radius">The radius of the circle in pixels.</param>
    /// <param name="color">The fill color of the circle.</param>
    internal static void FilledCircle(Vector2 center, float radius, Color color)
    {
        Color = color;
        float sqrRadius = radius * radius;

        for (float y = -radius; y <= radius; y++)
            for (float x = -radius; x <= radius; x++)
                if (x * x + y * y <= sqrRadius)
                    Line(center + new Vector2(x, y), center + new Vector2(x + 1, y), 1f);
    }
}