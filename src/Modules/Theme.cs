#pragma warning disable CS8618
#pragma warning disable CS8603

using UnityEngine;

namespace UnityCheatTemplate.Modules;

internal class Theme
{
    // Window Colors
    public static Color WindowBackground { get; set; } = new Color(0.1f, 0.1f, 0.1f, 0.95f);
    public static Color WindowActiveBackground { get; set; } = new Color(0.12f, 0.12f, 0.12f, 0.95f);
    public static Color WindowHoverBackground { get; set; } = new Color(0.11f, 0.11f, 0.11f, 0.95f);
    public static Color WindowTextColor { get; set; } = new Color(0.9f, 0.9f, 0.9f, 1f);
    public static Color WindowActiveTextColor { get; set; } = Color.white;

    // Button Colors
    public static Color ButtonNormal { get; set; } = new Color(0.2f, 0.5f, 0.8f, 1f);
    public static Color ButtonHover { get; set; } = new Color(0.3f, 0.6f, 0.9f, 1f);
    public static Color ButtonActive { get; set; } = new Color(0.1f, 0.4f, 0.7f, 1f);
    public static Color ButtonTextColor { get; set; } = Color.white;

    // Toggle Colors
    public static Color ToggleTextColor { get; set; } = new Color(0.9f, 0.9f, 0.9f, 1f);
    public static Color ToggleOnColor { get; set; } = new Color(0.2f, 0.8f, 0.2f, 1f);
    public static Color ToggleOnHoverColor { get; set; } = new Color(0.3f, 0.9f, 0.3f, 1f);

    // Label Colors
    public static Color LabelTextColor { get; set; } = new Color(0.8f, 0.8f, 0.8f, 1f);

    // Input Field Colors
    public static Color InputFieldBackground { get; set; } = new Color(0.15f, 0.15f, 0.15f, 1f);
    public static Color InputFieldHoverBackground { get; set; } = new Color(0.2f, 0.2f, 0.2f, 1f);
    public static Color InputFieldTextColor { get; set; } = new Color(0.9f, 0.9f, 0.9f, 1f);

    // Box Colors
    public static Color BoxBackground { get; set; } = new Color(0.15f, 0.15f, 0.15f, 0.8f);
    public static Color BoxTextColor { get; set; } = new Color(0.8f, 0.8f, 0.8f, 1f);

    // Slider Colors
    public static Color SliderTrackColor { get; set; } = new Color(0.3f, 0.3f, 0.3f, 1f);
    public static Color SliderThumbNormal { get; set; } = new Color(0.2f, 0.5f, 0.8f, 1f);
    public static Color SliderThumbHover { get; set; } = new Color(0.3f, 0.6f, 0.9f, 1f);
    public static Color SliderThumbActive { get; set; } = new Color(0.1f, 0.4f, 0.7f, 1f);

    // Scrollbar Colors
    public static Color ScrollbarBackground { get; set; } = new Color(0.2f, 0.2f, 0.2f, 1f);
    public static Color ScrollbarThumbNormal { get; set; } = new Color(0.4f, 0.4f, 0.4f, 1f);
    public static Color ScrollbarThumbHover { get; set; } = new Color(0.5f, 0.5f, 0.5f, 1f);
    public static Color ScrollbarThumbActive { get; set; } = new Color(0.3f, 0.3f, 0.3f, 1f);
    public static Color ScrollbarButtonNormal { get; set; } = new Color(0.3f, 0.3f, 0.3f, 1f);
    public static Color ScrollbarButtonHover { get; set; } = new Color(0.4f, 0.4f, 0.4f, 1f);
    public static Color ScrollbarButtonActive { get; set; } = new Color(0.2f, 0.2f, 0.2f, 1f);

    // Custom Style Colors
    public static Color HeaderBackground { get; set; } = new Color(0.25f, 0.25f, 0.25f, 1f);
    public static Color HeaderTextColor { get; set; } = new Color(0.9f, 0.9f, 0.9f, 1f);
    public static Color TooltipBackground { get; set; } = new Color(0.1f, 0.1f, 0.1f, 0.95f);
    public static Color TooltipTextColor { get; set; } = new Color(0.8f, 0.8f, 0.8f, 1f);
    public static Color SuccessTextColor { get; set; } = new Color(0.2f, 0.8f, 0.2f, 1f);
    public static Color WarningTextColor { get; set; } = new Color(1f, 0.8f, 0.2f, 1f);
    public static Color ErrorTextColor { get; set; } = new Color(1f, 0.2f, 0.2f, 1f);

    private static GUISkin _skin;
    internal static GUISkin Skin
    {
        get
        {
            if (_skin == null)
                Load();
            return _skin;
        }
        private set => _skin = value;
    }

    private static void Load()
    {
        Skin = ScriptableObject.CreateInstance<GUISkin>();

        // Create a clean, modern font style
        Skin.font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        // Window style
        Skin.window = CreateWindowStyle();

        // Button style
        Skin.button = CreateButtonStyle();

        // Toggle style
        Skin.toggle = CreateToggleStyle();

        // Label style
        Skin.label = CreateLabelStyle();

        // Text field style
        Skin.textField = CreateTextFieldStyle();

        // Text area style
        Skin.textArea = CreateTextAreaStyle();

        // Box style
        Skin.box = CreateBoxStyle();

        // Horizontal slider styles
        Skin.horizontalSlider = CreateHorizontalSliderStyle();
        Skin.horizontalSliderThumb = CreateHorizontalSliderThumbStyle();

        // Vertical slider styles
        Skin.verticalSlider = CreateVerticalSliderStyle();
        Skin.verticalSliderThumb = CreateVerticalSliderThumbStyle();

        // Scrollbar styles
        Skin.horizontalScrollbar = CreateHorizontalScrollbarStyle();
        Skin.horizontalScrollbarThumb = CreateHorizontalScrollbarThumbStyle();
        Skin.horizontalScrollbarLeftButton = CreateScrollbarButtonStyle();
        Skin.horizontalScrollbarRightButton = CreateScrollbarButtonStyle();
        Skin.verticalScrollbar = CreateVerticalScrollbarStyle();
        Skin.verticalScrollbarThumb = CreateVerticalScrollbarThumbStyle();
        Skin.verticalScrollbarUpButton = CreateScrollbarButtonStyle();
        Skin.verticalScrollbarDownButton = CreateScrollbarButtonStyle();

        // Custom styles for modern look
        Skin.customStyles = CreateCustomStyles();
    }

    private static GUIStyle CreateWindowStyle()
    {
        var style = new GUIStyle();

        // Background textures for all states
        style.normal.background = CreateTexture(2, 2, WindowBackground);

        style.onNormal.background = style.normal.background;
        style.active.background = CreateTexture(2, 2, WindowActiveBackground);
        style.onActive.background = style.active.background;
        style.focused.background = style.normal.background;
        style.onFocused.background = style.normal.background;
        style.hover.background = CreateTexture(2, 2, WindowHoverBackground);
        style.onHover.background = style.hover.background;

        // Text colors for ALL states
        style.normal.textColor = WindowTextColor;
        style.onNormal.textColor = style.normal.textColor;
        style.active.textColor = WindowActiveTextColor;
        style.onActive.textColor = WindowActiveTextColor;
        style.focused.textColor = style.normal.textColor;
        style.onFocused.textColor = style.normal.textColor;
        style.hover.textColor = Color.white;
        style.onHover.textColor = Color.white;

        int borderSize = 8;
        style.border = new RectOffset(borderSize, borderSize, 30, borderSize);
        style.padding = new RectOffset(10, 10, 24, 10);
        style.contentOffset = new Vector2(0, -20);

        // Text styling
        style.alignment = TextAnchor.UpperCenter;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 14;

        return style;
    }

    private static GUIStyle CreateButtonStyle()
    {
        var style = new GUIStyle();

        style.normal.background = CreateTexture(2, 2, ButtonNormal);
        style.hover.background = CreateTexture(2, 2, ButtonHover);
        style.active.background = CreateTexture(2, 2, ButtonActive);

        int borderSize = 6;
        style.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);

        style.padding = new RectOffset(12, 12, 8, 8);
        style.margin = new RectOffset(4, 4, 4, 4);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;
        style.fontSize = 12;
        style.normal.textColor = ButtonTextColor;
        style.hover.textColor = new Color(1f, 1f, 1f, 1f);
        style.active.textColor = new Color(0.9f, 0.9f, 0.9f, 1f);
        return style;
    }

    private static GUIStyle CreateToggleStyle()
    {
        var style = new GUIStyle();
        style.normal.textColor = ToggleTextColor;
        style.hover.textColor = Color.white;
        style.active.textColor = style.normal.textColor;
        style.onNormal.textColor = ToggleOnColor;
        style.onHover.textColor = ToggleOnHoverColor;
        style.fontSize = 12;
        style.padding = new RectOffset(20, 0, 2, 2);
        return style;
    }

    private static GUIStyle CreateLabelStyle()
    {
        var style = new GUIStyle();
        style.normal.textColor = LabelTextColor;
        style.fontSize = 12;
        style.padding = new RectOffset(2, 2, 2, 2);
        style.margin = new RectOffset(2, 2, 2, 2);
        style.alignment = TextAnchor.MiddleLeft;
        return style;
    }

    private static GUIStyle CreateTextFieldStyle()
    {
        var style = new GUIStyle();

        style.normal.background = CreateTexture(2, 2, InputFieldBackground);
        style.hover.background = CreateTexture(2, 2, InputFieldHoverBackground);
        style.active.background = style.hover.background;
        style.focused.background = CreateTexture(2, 2, InputFieldBackground);

        style.normal.textColor = InputFieldTextColor;
        style.hover.textColor = Color.white;
        style.focused.textColor = Color.white;

        int borderSize = 4;
        style.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);

        style.padding = new RectOffset(8, 8, 6, 6);
        style.margin = new RectOffset(2, 2, 2, 2);
        style.fontSize = 12;
        return style;
    }

    private static GUIStyle CreateTextAreaStyle()
    {
        var style = CreateTextFieldStyle();
        style.wordWrap = true;
        return style;
    }

    private static GUIStyle CreateBoxStyle()
    {
        var style = new GUIStyle();

        style.normal.background = CreateTexture(2, 2, BoxBackground);

        int borderSize = 4;
        style.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);

        style.padding = new RectOffset(8, 8, 8, 8);
        style.margin = new RectOffset(4, 4, 4, 4);
        style.normal.textColor = BoxTextColor;
        style.fontSize = 12;
        return style;
    }

    private static GUIStyle CreateHorizontalSliderStyle()
    {
        var style = new GUIStyle();
        style.normal.background = CreateTexture(2, 2, SliderTrackColor);
        style.fixedHeight = 8;
        style.border = new RectOffset(2, 2, 2, 2);
        return style;
    }

    private static GUIStyle CreateHorizontalSliderThumbStyle()
    {
        var style = new GUIStyle();

        style.normal.background = CreateTexture(2, 2, SliderThumbNormal);
        style.hover.background = CreateTexture(2, 2, SliderThumbHover);
        style.active.background = CreateTexture(2, 2, SliderThumbActive);

        style.fixedWidth = 16;
        style.fixedHeight = 16;
        style.border = new RectOffset(4, 4, 4, 4);
        return style;
    }

    private static GUIStyle CreateVerticalSliderStyle()
    {
        return CreateHorizontalSliderStyle();
    }

    private static GUIStyle CreateVerticalSliderThumbStyle()
    {
        return CreateHorizontalSliderThumbStyle();
    }

    private static GUIStyle CreateHorizontalScrollbarStyle()
    {
        var style = new GUIStyle();
        style.normal.background = CreateTexture(2, 2, ScrollbarBackground);
        style.fixedHeight = 12;
        style.border = new RectOffset(2, 2, 2, 2);
        return style;
    }

    private static GUIStyle CreateHorizontalScrollbarThumbStyle()
    {
        var style = new GUIStyle();

        style.normal.background = CreateTexture(2, 2, ScrollbarThumbNormal);
        style.hover.background = CreateTexture(2, 2, ScrollbarThumbHover);
        style.active.background = CreateTexture(2, 2, ScrollbarThumbActive);

        style.border = new RectOffset(3, 3, 3, 3);
        return style;
    }

    private static GUIStyle CreateVerticalScrollbarStyle()
    {
        var style = CreateHorizontalScrollbarStyle();
        style.fixedWidth = 12;
        style.fixedHeight = 0;
        return style;
    }

    private static GUIStyle CreateVerticalScrollbarThumbStyle()
    {
        return CreateHorizontalScrollbarThumbStyle();
    }

    private static GUIStyle CreateScrollbarButtonStyle()
    {
        var style = new GUIStyle();

        style.normal.background = CreateTexture(2, 2, ScrollbarButtonNormal);
        style.hover.background = CreateTexture(2, 2, ScrollbarButtonHover);
        style.active.background = CreateTexture(2, 2, ScrollbarButtonActive);

        style.fixedWidth = 12;
        style.fixedHeight = 12;
        style.border = new RectOffset(2, 2, 2, 2);
        return style;
    }

    private static GUIStyle[] CreateCustomStyles()
    {
        // Create a custom style for section headers
        var headerStyle = new GUIStyle();

        headerStyle.normal.background = CreateTexture(2, 2, HeaderBackground);

        headerStyle.normal.textColor = HeaderTextColor;
        headerStyle.fontStyle = FontStyle.Bold;
        headerStyle.fontSize = 13;
        headerStyle.alignment = TextAnchor.MiddleCenter;

        int borderSize = 2;
        headerStyle.border = new RectOffset(borderSize, borderSize, borderSize, borderSize);

        headerStyle.padding = new RectOffset(10, 10, 6, 6);
        headerStyle.margin = new RectOffset(4, 4, 8, 4);

        // Create a custom style for tooltips
        var tooltipStyle = new GUIStyle();

        tooltipStyle.normal.background = CreateTexture(2, 2, TooltipBackground);

        tooltipStyle.normal.textColor = TooltipTextColor;
        tooltipStyle.fontSize = 11;
        tooltipStyle.wordWrap = true;
        tooltipStyle.alignment = TextAnchor.MiddleLeft;
        tooltipStyle.border = new RectOffset(4, 4, 4, 4);
        tooltipStyle.padding = new RectOffset(6, 6, 4, 4);

        // Create a custom style for success messages
        var successStyle = new GUIStyle();
        successStyle.normal.textColor = SuccessTextColor;
        successStyle.fontSize = 12;
        successStyle.alignment = TextAnchor.MiddleCenter;
        successStyle.padding = new RectOffset(10, 10, 6, 6);

        // Create a custom style for warning messages
        var warningStyle = new GUIStyle();
        warningStyle.normal.textColor = WarningTextColor;
        warningStyle.fontSize = 12;
        warningStyle.alignment = TextAnchor.MiddleCenter;
        warningStyle.padding = new RectOffset(10, 10, 6, 6);

        // Create a custom style for error messages
        var errorStyle = new GUIStyle();
        errorStyle.normal.textColor = ErrorTextColor;
        errorStyle.fontSize = 12;
        errorStyle.alignment = TextAnchor.MiddleCenter;
        errorStyle.padding = new RectOffset(10, 10, 6, 6);

        return
        [
            headerStyle,
            tooltipStyle,
            successStyle,
            warningStyle,
            errorStyle
        ];
    }


    private static Texture2D CreateTexture(int width, int height, Color color)
    {
        var texture = new Texture2D(width, height);
        var colors = new Color[width * height];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = color;
        }

        texture.SetPixels(colors);
        texture.Apply();

        texture.wrapMode = TextureWrapMode.Clamp;
        texture.filterMode = FilterMode.Bilinear;
        texture.hideFlags = HideFlags.DontSave;

        return texture;
    }
}