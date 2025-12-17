using UnityEngine;

namespace UnityCheatTemplate.Utilities;

internal static class UI
{
    internal static string? Tooltip { get; private set; }

    internal static void ResetTooltip() => Tooltip = null;

    private static void UpdateTooltip(string tooltip, Rect controlRect)
    {
        if (controlRect.Contains(Event.current.mousePosition))
            Tooltip = tooltip;
    }

    internal static bool CenteredButton(string label)
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool clicked = GUILayout.Button(label);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        return clicked;
    }

    internal static void Tab<T>(string label, ref T currentTab, T tabValue, bool centered = false)
    {
        if ((centered ? CenteredButton(label) : GUILayout.Button(label)))
            currentTab = tabValue;
    }

    internal static void Header(string text, int fontSize = 14)
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold,
            fontSize = fontSize
        };

        GUILayout.Space(10);
        GUILayout.Label(text, style);
        GUILayout.Space(5);
    }

    internal static void Divider(float thickness = 1f, Color? color = null)
    {
        var dividerColor = color ?? new Color(0.5f, 0.5f, 0.5f, 0.5f);

        GUILayout.Space(10);

        var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(thickness));
        rect.xMin = 0;
        rect.xMax = Screen.width;

        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = dividerColor;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUILayout.Space(10);
    }

    internal static void ColorPicker(ref Color color, string label, Action<Color>? callback = null)
    {
        Color previousColor = color;

        GUILayout.BeginVertical(GUI.skin.box);

        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(100));

        Rect previewRect = GUILayoutUtility.GetRect(60, 35);
        GUI.DrawTexture(previewRect, CreateColorTexture(Color.black));
        Rect innerRect = new(previewRect.x + 2, previewRect.y + 2, previewRect.width - 4, previewRect.height - 4);
        GUI.DrawTexture(innerRect, CreateColorTexture(color));

        GUILayout.Label($"#{ColorToHex(color)}", GUILayout.Width(70));

        GUILayout.EndHorizontal();

        GUILayout.Space(5);

        color.r = EnhancedColorSlider("Red", color.r);
        color.g = EnhancedColorSlider("Green", color.g);
        color.b = EnhancedColorSlider("Blue", color.b);

        GUILayout.EndVertical();

        if (previousColor != color)
            callback?.Invoke(color);
    }

    private static float EnhancedColorSlider(string label, float value)
    {
        GUILayout.BeginHorizontal();

        // Color label with icon/spacer
        GUILayout.Label(GetColorIcon(label), GUILayout.Width(20));
        GUILayout.Label(label[0].ToString(), GUILayout.Width(20));

        // Slider
        float newValue = GUILayout.HorizontalSlider(value, 0f, 1f, GUILayout.Width(100));

        // Numeric input with better styling
        GUILayout.BeginVertical(GUILayout.Width(50));

        // Display both 0-255 and 0.00-1.00 values
        int intValue = Mathf.RoundToInt(newValue * 255);
        string displayText = GUILayout.TextField(intValue.ToString(), GUILayout.Width(40));

        if (int.TryParse(displayText, out int parsedValue))
        {
            newValue = Mathf.Clamp01(parsedValue / 255f);
        }

        // Show normalized value below
        GUILayout.Label(newValue.ToString("F2"), GUILayout.Width(40));
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        return newValue;
    }

    private static string GetColorIcon(string label)
    {
        // Return different symbols based on color component
        return label.ToLower() switch
        {
            "red" => "●",
            "green" => "●",
            "blue" => "●",
            _ => "●",
        };
    }

    private static Texture2D CreateColorTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    private static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        return $"{r:X2}{g:X2}{b:X2}";
    }

    internal static bool Checkbox(ref bool value, string label, string tooltip = "", System.Action<bool>? callback = null)
    {
        return ToggleControl(ref value, label, tooltip, callback);
    }

    internal static bool Toggle(ref bool value, string label, string tooltip = "", System.Action<bool>? callback = null)
    {
        return ToggleControl(ref value, label, tooltip, callback);
    }

    private static bool ToggleControl(ref bool value, string label, string tooltip, System.Action<bool>? callback)
    {
        bool previousValue = value;
        value = GUILayout.Toggle(value, label);

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (previousValue != value)
            callback?.Invoke(value);

        return value != previousValue;
    }

    internal static void Button(string label, string tooltip, System.Action callback)
    {
        if (GUILayout.Button(label))
            callback.Invoke();

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);
    }

    internal static int IntSlider(ref int value, int min, int max, string label, string tooltip = "", Action<int>? callback = null)
    {
        float temp = value;
        value = Mathf.RoundToInt(FloatSlider(ref temp, min, max, label, tooltip, newValue =>
        {
            callback?.Invoke(Mathf.RoundToInt(newValue));
        }));
        return value;
    }

    internal static float FloatSlider(ref float value, float min, float max, string label, string tooltip = "", Action<float>? callback = null)
    {
        return Slider(ref value, min, max, label, false, tooltip, callback);
    }

    private static float Slider(ref float value, float min, float max, string label, bool isInteger, string tooltip, Action<float>? callback)
    {
        float previousValue = value;

        GUILayout.BeginHorizontal();
        {
            string displayValue = isInteger ? $"{(int)value}" : $"{value:F2}";
            GUILayout.Label($"{label}: ", GUILayout.ExpandWidth(false));

            string textValue = GUILayout.TextField(displayValue, GUILayout.Width(60));
            if (float.TryParse(textValue, out float parsedValue))
            {
                value = Mathf.Clamp(parsedValue, min, max);
                if (isInteger) value = Mathf.Round(value);
            }

            float newValue = GUILayout.HorizontalSlider(value, min, max);
            if (isInteger) newValue = Mathf.Round(newValue);
            value = newValue;
        }
        GUILayout.EndHorizontal();

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (Mathf.Abs(previousValue - value) > 0.001f)
            callback?.Invoke(value);

        return value;
    }

    internal static string TextField(ref string text, string label = "", int width = 200, string tooltip = "", System.Action<string>? callback = null)
    {
        return TextBox(ref text, label, width, 0, tooltip, callback);
    }

    internal static string TextArea(ref string text, string label = "", int width = 200, int height = 100, string tooltip = "", System.Action<string>? callback = null)
    {
        return TextBox(ref text, label, width, height, tooltip, callback);
    }

    private static string TextBox(ref string text, string label, int width, int height, string tooltip, System.Action<string>? callback)
    {
        string previousText = text;

        GUILayout.BeginHorizontal();
        {
            if (!string.IsNullOrEmpty(label))
                GUILayout.Label(label, GUILayout.ExpandWidth(false));

            string newText = height > 0
                ? GUILayout.TextArea(text, GUILayout.Width(width), GUILayout.Height(height))
                : GUILayout.TextField(text, GUILayout.Width(width));

            if (newText != text)
                text = newText;
        }
        GUILayout.EndHorizontal();

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (previousText != text)
            callback?.Invoke(text);

        return text;
    }

    private static readonly Dictionary<string, DropdownState> _dropdownStates = new();

    private class DropdownState
    {
        public bool IsExpanded = false;
        public Vector2 ScrollPosition = Vector2.zero;
    }

    internal static int Dropdown(ref int selectedIndex, string label, string[] options, string tooltip = "", System.Action<int>? callback = null)
    {
        if (options == null || options.Length == 0)
        {
            selectedIndex = -1;
            return selectedIndex;
        }

        selectedIndex = Mathf.Clamp(selectedIndex, 0, options.Length - 1);
        int previousIndex = selectedIndex;

        string dropdownId = $"{label}-{options.Length}";

        if (!_dropdownStates.TryGetValue(dropdownId, out DropdownState state))
        {
            state = new DropdownState();
            _dropdownStates[dropdownId] = state;
        }

        GUILayout.BeginVertical();
        {
            GUILayout.Label(label);

            GUILayout.BeginHorizontal();
            string buttonLabel = selectedIndex >= 0 ? options[selectedIndex] : "Select...";

            if (GUILayout.Button(buttonLabel, GUILayout.ExpandWidth(true)))
                state.IsExpanded = !state.IsExpanded;

            GUILayout.Label(state.IsExpanded ? "▲" : "▼", GUILayout.Width(20));
            GUILayout.EndHorizontal();

            Rect buttonRect = GUILayoutUtility.GetLastRect();
            UpdateTooltip(tooltip, buttonRect);

            if (state.IsExpanded)
            {
                float maxHeight = Mathf.Min(options.Length * 25, 200);

                GUILayout.BeginVertical(GUI.skin.box);
                state.ScrollPosition = GUILayout.BeginScrollView(state.ScrollPosition,
                    GUILayout.Height(maxHeight));

                for (int i = 0; i < options.Length; i++)
                {
                    if (GUILayout.Button(options[i]))
                    {
                        selectedIndex = i;
                        state.IsExpanded = false;

                        if (Event.current != null)
                            Event.current.Use();
                    }
                }

                GUILayout.EndScrollView();
                GUILayout.EndVertical();

                if (Event.current?.type == EventType.MouseDown &&
                    !GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
                {
                    state.IsExpanded = false;
                }
            }
        }
        GUILayout.EndVertical();

        if (previousIndex != selectedIndex)
            callback?.Invoke(selectedIndex);

        return selectedIndex;
    }

    internal static Texture2D CreateSolidTexture(int width, int height, Color color)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = color;

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }

    internal static Texture2D CreateGradientTexture(int width, int height, Color startColor, Color endColor, bool horizontal = true)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float t = horizontal ? (float)x / (width - 1) : (float)y / (height - 1);
                pixels[y * width + x] = Color.Lerp(startColor, endColor, t);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
        return texture;
    }
}