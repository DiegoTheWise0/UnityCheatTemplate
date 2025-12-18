using UnityEngine;

namespace UnityCheatTemplate.Modules.Menu.Core;

/// <summary>
/// Provides a comprehensive set of UI components and layout utilities for creating cheat menus and interfaces.
/// </summary>
internal static class UI
{
    /// <summary>
    /// Gets the current tooltip text to display, if any.
    /// </summary>
    internal static string? Tooltip { get; private set; }

    private static readonly Dictionary<string, DropdownState> _dropdownStates = new();

    /// <summary>
    /// Represents the state of a dropdown UI element.
    /// </summary>
    private class DropdownState
    {
        /// <summary>
        /// Whether the dropdown is currently expanded.
        /// </summary>
        public bool IsExpanded = false;
        /// <summary>
        /// The scroll position within the dropdown list.
        /// </summary>
        public Vector2 ScrollPosition = Vector2.zero;
    }

    /// <summary>
    /// Clears the current tooltip text.
    /// </summary>
    internal static void ResetTooltip() => Tooltip = null;

    /// <summary>
    /// Updates the tooltip text if the mouse is over the specified control rectangle.
    /// </summary>
    /// <param name="tooltip">The tooltip text to display.</param>
    /// <param name="controlRect">The rectangle area of the control that triggers the tooltip.</param>
    private static void UpdateTooltip(string tooltip, Rect controlRect)
    {
        if (controlRect.Contains(Event.current.mousePosition))
            Tooltip = tooltip;
    }

    /// <summary>
    /// Begins a horizontal layout group.
    /// </summary>
    /// <param name="style">The GUI style to apply to the layout group.</param>
    /// <param name="spacing">Additional spacing at the beginning of the layout.</param>
    internal static void BeginHorizontal(string style = "", float spacing = 0)
    {
        GUILayout.BeginHorizontal(style != "" ? style : GUIStyle.none);
        if (spacing > 0) GUILayout.Space(spacing);
    }

    /// <summary>
    /// Ends a horizontal layout group.
    /// </summary>
    /// <param name="spacing">Additional spacing at the end of the layout.</param>
    internal static void EndHorizontal(float spacing = 0)
    {
        if (spacing > 0) GUILayout.Space(spacing);
        GUILayout.EndHorizontal();
    }

    /// <summary>
    /// Begins a vertical layout group.
    /// </summary>
    /// <param name="style">The GUI style to apply to the layout group.</param>
    /// <param name="spacing">Additional spacing at the beginning of the layout.</param>
    internal static void BeginVertical(string style = "box", float spacing = 0)
    {
        GUILayout.BeginVertical(style != "" ? style : GUI.skin.box);
        if (spacing > 0) GUILayout.Space(spacing);
    }

    /// <summary>
    /// Ends a vertical layout group.
    /// </summary>
    /// <param name="spacing">Additional spacing at the end of the layout.</param>
    internal static void EndVertical(float spacing = 0)
    {
        if (spacing > 0) GUILayout.Space(spacing);
        GUILayout.EndVertical();
    }

    /// <summary>
    /// Adds vertical spacing between UI elements.
    /// </summary>
    /// <param name="height">The height of the spacer in pixels.</param>
    internal static void Spacer(float height = 10)
    {
        GUILayout.Space(height);
    }

    /// <summary>
    /// Creates a centered button.
    /// </summary>
    /// <param name="label">The text displayed on the button.</param>
    /// <param name="width">The width of the button in pixels.</param>
    /// <param name="height">The height of the button in pixels.</param>
    /// <returns>True if the button was clicked in the current frame; otherwise false.</returns>
    internal static bool CenteredButton(string label, float width = 120, float height = 25)
    {
        BeginHorizontal();
        GUILayout.FlexibleSpace();
        bool clicked = GUILayout.Button(label, GUILayout.Width(width), GUILayout.Height(height));
        GUILayout.FlexibleSpace();
        EndHorizontal();
        return clicked;
    }

    /// <summary>
    /// Creates a tab button for tabbed interfaces.
    /// </summary>
    /// <param name="label">The text displayed on the tab.</param>
    /// <param name="currentTab">Reference to the currently selected tab value.</param>
    /// <param name="tabValue">The value this tab represents.</param>
    /// <param name="centered">Whether to center the tab within its container.</param>
    /// <param name="width">The width of the tab in pixels.</param>
    /// <param name="height">The height of the tab in pixels.</param>
    internal static void Tab(string label, ref uint currentTab, uint tabValue, bool centered = false, float width = 100, float height = 30)
    {
        GUIStyle style = new(GUI.skin.button)
        {
            margin = new RectOffset(2, 2, 2, 2),
            padding = new RectOffset(10, 10, 5, 5)
        };

        if (centered)
        {
            BeginHorizontal();
            GUILayout.FlexibleSpace();
        }

        if (GUILayout.Button(label, style, GUILayout.Width(width), GUILayout.Height(height)) && currentTab != tabValue)
        {
            currentTab = tabValue;
        }

        if (centered)
        {
            GUILayout.FlexibleSpace();
            EndHorizontal();
        }
    }

    /// <summary>
    /// Creates a header label with customizable styling.
    /// </summary>
    /// <param name="text">The header text to display.</param>
    /// <param name="fontSize">The font size of the header.</param>
    /// <param name="alignment">The text alignment.</param>
    internal static void Header(string text, int fontSize = 16, TextAnchor alignment = TextAnchor.MiddleCenter)
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = alignment,
            fontStyle = FontStyle.Bold,
            fontSize = fontSize,
            normal = { textColor = Color.white }
        };

        Spacer(10);
        BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label(text, style);
        GUILayout.FlexibleSpace();
        EndHorizontal();
        Spacer(5);
    }

    /// <summary>
    /// Creates a subheader label with customizable styling.
    /// </summary>
    /// <param name="text">The subheader text to display.</param>
    /// <param name="fontSize">The font size of the subheader.</param>
    /// <param name="alignment">The text alignment.</param>
    internal static void Subheader(string text, int fontSize = 12, TextAnchor alignment = TextAnchor.MiddleLeft)
    {
        var style = new GUIStyle(GUI.skin.label)
        {
            alignment = alignment,
            fontStyle = FontStyle.Bold,
            fontSize = fontSize,
            normal = { textColor = new Color(0.8f, 0.8f, 0.8f) }
        };

        Spacer(5);
        GUILayout.Label(text, style);
        Spacer(2);
    }

    /// <summary>
    /// Creates a horizontal divider line.
    /// </summary>
    /// <param name="thickness">The thickness of the divider line in pixels.</param>
    /// <param name="color">The color of the divider line.</param>
    /// <param name="spacing">The spacing above and below the divider.</param>
    internal static void Divider(float thickness = 2f, Color? color = null, float spacing = 10)
    {
        var dividerColor = color ?? new Color(0.3f, 0.3f, 0.3f, 0.8f);

        GUILayout.Space(spacing);

        var rect = GUILayoutUtility.GetRect(GUIContent.none, GUIStyle.none, GUILayout.Height(thickness));
        rect.xMin = 0;
        rect.xMax = Screen.width;

        GUI.color = dividerColor;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUILayout.Space(spacing);
    }

    /// <summary>
    /// Adds vertical spacing between UI elements.
    /// </summary>
    /// <param name="spacing">The amount of spacing in pixels.</param>
    internal static void Separator(float spacing = 5)
    {
        GUILayout.Space(spacing);
    }

    /// <summary>
    /// Creates a checkbox control.
    /// </summary>
    /// <param name="value">Reference to the boolean value controlled by the checkbox.</param>
    /// <param name="label">The label text displayed next to the checkbox.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the checkbox.</param>
    /// <param name="callback">Optional callback invoked when the checkbox value changes.</param>
    /// <param name="boldLabel">Whether to display the label text in bold.</param>
    /// <returns>True if the checkbox value changed in the current frame; otherwise false.</returns>
    internal static bool Checkbox(ref bool value, string label, string tooltip = "", Action<bool>? callback = null, bool boldLabel = false)
    {
        bool previousValue = value;

        BeginHorizontal();

        GUILayout.Space(5);
        value = GUILayout.Toggle(value, "", GUILayout.Width(20));

        var labelStyle = boldLabel ? new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold } : GUI.skin.label;
        GUILayout.Label(label, labelStyle, GUILayout.ExpandWidth(true));

        EndHorizontal();

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (previousValue != value)
        {
            callback?.Invoke(value);
        }

        return value != previousValue;
    }

    /// <summary>
    /// Creates a toggle button that switches between two states.
    /// </summary>
    /// <param name="value">Reference to the boolean value controlled by the toggle button.</param>
    /// <param name="label">The text displayed on the button.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the button.</param>
    /// <param name="callback">Optional callback invoked when the toggle value changes.</param>
    /// <param name="width">The width of the button in pixels.</param>
    /// <returns>True if the toggle value changed in the current frame; otherwise false.</returns>
    internal static bool ToggleButton(ref bool value, string label, string tooltip = "", Action<bool>? callback = null, float width = 120)
    {
        bool previousValue = value;

        var style = new GUIStyle(GUI.skin.button)
        {
            padding = new RectOffset(15, 15, 5, 5),
            margin = new RectOffset(2, 2, 2, 2)
        };

        if (value)
        {
            style.normal = style.onNormal;
            style.hover = style.onHover;
        }

        if (GUILayout.Button(label, style, GUILayout.Width(width)))
        {
            value = !value;
        }

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (previousValue != value)
        {
            callback?.Invoke(value);
        }

        return value != previousValue;
    }

    /// <summary>
    /// Creates a standard button.
    /// </summary>
    /// <param name="label">The text displayed on the button.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the button.</param>
    /// <param name="callback">The action to invoke when the button is clicked.</param>
    /// <param name="width">The width of the button in pixels.</param>
    /// <param name="height">The height of the button in pixels.</param>
    /// <param name="expand">Whether the button should expand to fill available horizontal space.</param>
    internal static void Button(string label, string tooltip, Action callback, float width = 120, float height = 25, bool expand = false)
    {
        var style = new GUIStyle(GUI.skin.button)
        {
            padding = new RectOffset(15, 15, 5, 5),
            margin = new RectOffset(2, 2, 2, 2)
        };

        if (GUILayout.Button(label, style, expand ? GUILayout.ExpandWidth(true) : GUILayout.Width(width), GUILayout.Height(height)))
        {
            callback.Invoke();
        }

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);
    }

    /// <summary>
    /// Creates a slider control for float values.
    /// </summary>
    /// <param name="value">Reference to the float value controlled by the slider.</param>
    /// <param name="min">The minimum value of the slider.</param>
    /// <param name="max">The maximum value of the slider.</param>
    /// <param name="label">The label text displayed above the slider.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the slider.</param>
    /// <param name="callback">Optional callback invoked when the slider value changes.</param>
    /// <returns>The current slider value.</returns>
    internal static float FloatSlider(ref float value, float min, float max, string label, string tooltip = "", Action<float>? callback = null)
    {
        float previousValue = value;

        BeginVertical("box", 2);

        // Header row with label and value
        BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(100));

        string displayValue = $"{value:F2}";
        string textValue = GUILayout.TextField(displayValue, GUILayout.Width(60));

        if (float.TryParse(textValue, out float parsedValue))
        {
            value = Mathf.Clamp(parsedValue, min, max);
        }

        GUILayout.Label($"[{min:F1}-{max:F1}]", GUILayout.Width(60));
        EndHorizontal();

        BeginHorizontal();
        GUILayout.Label(min.ToString("F1"), GUILayout.Width(30));
        value = GUILayout.HorizontalSlider(value, min, max, GUILayout.ExpandWidth(true));
        GUILayout.Label(max.ToString("F1"), GUILayout.Width(30));
        EndHorizontal();

        EndVertical();

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (Mathf.Abs(previousValue - value) > 0.001f)
        {
            callback?.Invoke(value);
        }

        return value;
    }

    /// <summary>
    /// Creates a slider control for integer values.
    /// </summary>
    /// <param name="value">Reference to the integer value controlled by the slider.</param>
    /// <param name="min">The minimum value of the slider.</param>
    /// <param name="max">The maximum value of the slider.</param>
    /// <param name="label">The label text displayed above the slider.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the slider.</param>
    /// <param name="callback">Optional callback invoked when the slider value changes.</param>
    /// <returns>The current slider value.</returns>
    internal static int IntSlider(ref int value, int min, int max, string label, string tooltip = "", Action<int>? callback = null)
    {
        float temp = value;
        value = Mathf.RoundToInt(FloatSlider(ref temp, min, max, label, tooltip, newValue =>
        {
            callback?.Invoke(Mathf.RoundToInt(newValue));
        }));
        return value;
    }

    /// <summary>
    /// Creates a color picker control.
    /// </summary>
    /// <param name="color">Reference to the Color value controlled by the color picker.</param>
    /// <param name="label">The label text displayed above the color picker.</param>
    /// <param name="callback">Optional callback invoked when the color changes.</param>
    internal static void ColorPicker(ref Color color, string label, Action<Color>? callback = null)
    {
        Color previousColor = color;

        BeginVertical("box", 5);

        BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(100));

        Rect previewRect = GUILayoutUtility.GetRect(80, 25);
        GUI.DrawTexture(previewRect, CreateColorTexture(Color.black));
        Rect innerRect = new(previewRect.x + 1, previewRect.y + 1, previewRect.width - 2, previewRect.height - 2);
        GUI.DrawTexture(innerRect, CreateColorTexture(color));

        GUILayout.Label($"#{ColorToHex(color)}", GUILayout.Width(70));
        GUILayout.FlexibleSpace();
        EndHorizontal();

        Separator(5);

        BeginVertical();
        color.r = ColorSlider("R", color.r, Color.red);
        color.g = ColorSlider("G", color.g, Color.green);
        color.b = ColorSlider("B", color.b, Color.blue);
        EndVertical();

        EndVertical();

        if (previousColor != color)
        {
            callback?.Invoke(color);
        }
    }

    /// <summary>
    /// Creates a color channel slider for the color picker.
    /// </summary>
    /// <param name="label">The channel label (R, G, or B).</param>
    /// <param name="value">The current channel value (0-1).</param>
    /// <param name="labelColor">The color to use for the label.</param>
    /// <returns>The new channel value.</returns>
    private static float ColorSlider(string label, float value, Color labelColor)
    {
        BeginHorizontal();

        GUILayout.Label(label, GUILayout.Width(20));

        Rect colorRect = GUILayoutUtility.GetRect(10, 10);
        GUI.color = labelColor;
        GUI.DrawTexture(colorRect, Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUILayout.Space(5);

        float newValue = GUILayout.HorizontalSlider(value, 0f, 1f, GUILayout.ExpandWidth(true));

        GUILayout.Label($"{Mathf.RoundToInt(newValue * 255)}", GUILayout.Width(30));
        GUILayout.Label($"{newValue:F2}", GUILayout.Width(40));

        EndHorizontal();

        return newValue;
    }

    /// <summary>
    /// Creates a single-line text input field.
    /// </summary>
    /// <param name="text">Reference to the string value controlled by the text field.</param>
    /// <param name="label">Optional label text displayed above the text field.</param>
    /// <param name="width">The width of the text field in pixels.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the text field.</param>
    /// <param name="callback">Optional callback invoked when the text changes.</param>
    /// <returns>The current text value.</returns>
    internal static string TextField(ref string text, string label = "", int width = 200, string tooltip = "", Action<string>? callback = null)
    {
        return TextBox(ref text, label, width, 0, tooltip, callback);
    }

    /// <summary>
    /// Creates a multi-line text input area.
    /// </summary>
    /// <param name="text">Reference to the string value controlled by the text area.</param>
    /// <param name="label">Optional label text displayed above the text area.</param>
    /// <param name="width">The width of the text area in pixels.</param>
    /// <param name="height">The height of the text area in pixels.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the text area.</param>
    /// <param name="callback">Optional callback invoked when the text changes.</param>
    /// <returns>The current text value.</returns>
    internal static string TextArea(ref string text, string label = "", int width = 200, int height = 100, string tooltip = "", Action<string>? callback = null)
    {
        return TextBox(ref text, label, width, height, tooltip, callback);
    }

    /// <summary>
    /// Creates a text input control (single-line or multi-line).
    /// </summary>
    /// <param name="text">Reference to the string value controlled by the text input.</param>
    /// <param name="label">Optional label text displayed above the text input.</param>
    /// <param name="width">The width of the text input in pixels.</param>
    /// <param name="height">The height of the text input in pixels (0 for single-line).</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the text input.</param>
    /// <param name="callback">Optional callback invoked when the text changes.</param>
    /// <returns>The current text value.</returns>
    private static string TextBox(ref string text, string label, int width, int height, string tooltip, Action<string>? callback)
    {
        string previousText = text;

        BeginVertical("box", 2);

        if (!string.IsNullOrEmpty(label))
        {
            GUILayout.Label(label);
        }

        string newText = height > 0
            ? GUILayout.TextArea(text, GUILayout.Width(width), GUILayout.Height(height))
            : GUILayout.TextField(text, GUILayout.Width(width));

        if (newText != text)
        {
            text = newText;
        }

        EndVertical();

        Rect controlRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, controlRect);

        if (previousText != text)
        {
            callback?.Invoke(text);
        }

        return text;
    }

    /// <summary>
    /// Creates a dropdown selection control.
    /// </summary>
    /// <param name="selectedIndex">Reference to the currently selected index.</param>
    /// <param name="label">The label text displayed above the dropdown.</param>
    /// <param name="options">The array of option strings to display in the dropdown.</param>
    /// <param name="tooltip">The tooltip text displayed when hovering over the dropdown.</param>
    /// <param name="callback">Optional callback invoked when the selection changes.</param>
    /// <returns>The current selected index.</returns>
    internal static int Dropdown(ref int selectedIndex, string label, string[] options, string tooltip = "", Action<int>? callback = null)
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

        BeginVertical("box", 2);

        GUILayout.Label(label);

        BeginHorizontal();

        string buttonLabel = selectedIndex >= 0 ? options[selectedIndex] : "Select...";
        var buttonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 10, 5, 5)
        };

        if (GUILayout.Button(buttonLabel, buttonStyle, GUILayout.ExpandWidth(true)))
        {
            state.IsExpanded = !state.IsExpanded;
        }

        GUILayout.Label(state.IsExpanded ? "▲" : "▼", GUILayout.Width(20));

        EndHorizontal();

        Rect buttonRect = GUILayoutUtility.GetLastRect();
        UpdateTooltip(tooltip, buttonRect);

        if (state.IsExpanded)
        {
            float maxHeight = Mathf.Min(options.Length * 25, 200);

            BeginVertical("box");
            state.ScrollPosition = GUILayout.BeginScrollView(state.ScrollPosition,
                GUILayout.Height(maxHeight));

            var optionStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleLeft,
                margin = new RectOffset(0, 0, 1, 1),
                padding = new RectOffset(10, 10, 5, 5)
            };

            for (int i = 0; i < options.Length; i++)
            {
                if (GUILayout.Button(options[i], optionStyle))
                {
                    selectedIndex = i;
                    state.IsExpanded = false;

                    if (Event.current != null)
                        Event.current.Use();
                }
            }

            GUILayout.EndScrollView();
            EndVertical();
        }

        EndVertical();

        if (previousIndex != selectedIndex)
        {
            callback?.Invoke(selectedIndex);
        }

        return selectedIndex;
    }

    /// <summary>
    /// Creates a 1x1 texture with a solid color.
    /// </summary>
    /// <param name="color">The color for the texture.</param>
    /// <returns>A 1x1 texture filled with the specified color.</returns>
    private static Texture2D CreateColorTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }

    /// <summary>
    /// Converts a Color to a hexadecimal string representation.
    /// </summary>
    /// <param name="color">The color to convert.</param>
    /// <returns>A hexadecimal string in RRGGBB format.</returns>
    private static string ColorToHex(Color color)
    {
        int r = Mathf.RoundToInt(color.r * 255);
        int g = Mathf.RoundToInt(color.g * 255);
        int b = Mathf.RoundToInt(color.b * 255);
        return $"{r:X2}{g:X2}{b:X2}";
    }

    /// <summary>
    /// Creates a solid color texture of the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="color">The color to fill the texture with.</param>
    /// <returns>A texture filled with the specified color.</returns>
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

    /// <summary>
    /// Creates a gradient texture of the specified dimensions.
    /// </summary>
    /// <param name="width">The width of the texture.</param>
    /// <param name="height">The height of the texture.</param>
    /// <param name="startColor">The starting color of the gradient.</param>
    /// <param name="endColor">The ending color of the gradient.</param>
    /// <param name="horizontal">True for a horizontal gradient; false for vertical.</param>
    /// <returns>A texture with a gradient fill.</returns>
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