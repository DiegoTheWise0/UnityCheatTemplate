using UnityEngine;

namespace UnityCheatTemplate.Modules;

internal static class Render
{
    private class RingArray
    {
        internal Vector2[] Positions { get; private set; }

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

    internal static Color Color
    {
        get { return GUI.color; }
        set { GUI.color = value; }
    }

    internal static void Line(Vector2 from, Vector2 to, float thickness, Color color)
    {
        Color = color;
        Line(from, to, thickness);
    }
    internal static void Line(Vector2 from, Vector2 to, float thickness)
    {
        var delta = (to - from).normalized;
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        GUIUtility.RotateAroundPivot(angle, from);
        Box(from, Vector2.right * (from - to).magnitude, thickness, false);
        GUIUtility.RotateAroundPivot(-angle, from);
    }

    internal static void Box(Vector2 position, Vector2 size, float thickness, Color color, bool centered = true)
    {
        Color = color;
        Box(position, size, thickness, centered);
    }
    internal static void Box(Vector2 position, Vector2 size, float thickness, bool centered = true)
    {
        var upperLeft = centered ? position - size / 2f : position;
        GUI.DrawTexture(new Rect(position.x, position.y, size.x, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x, position.y, thickness, size.y), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x + size.x, position.y, thickness, size.y), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x, position.y + size.y, size.x + thickness, thickness), Texture2D.whiteTexture);
    }

    internal static void Cross(Vector2 position, Vector2 size, float thickness, Color color)
    {
        Color = color;
        Cross(position, size, thickness);
    }
    internal static void Cross(Vector2 position, Vector2 size, float thickness)
    {
        GUI.DrawTexture(new Rect(position.x - size.x / 2f, position.y, size.x, thickness), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(position.x, position.y - size.y / 2f, thickness, size.y), Texture2D.whiteTexture);
    }

    internal static void Dot(Vector2 position, Color color)
    {
        Color = color;
        Dot(position);
    }
    internal static void Dot(Vector2 position)
    {
        Box(position - Vector2.one, Vector2.one * 2f, 1f);
    }

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
