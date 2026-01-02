using System.Reflection;
using UnityCheatTemplate.Data;
using UnityCheatTemplate.Interfaces;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Menu.Core;

/// <summary>
/// Main cheat menu UI system that provides a resizable window with tabbed interface.
/// Implements both ILoadable and ISingleton interfaces for managed lifecycle and singleton access.
/// </summary>
internal sealed class CheatMenuUI : ILoadable, ISingleton
{
    private bool _loaded;
    private bool _isOpen;

    private static List<CheatMenuTab> _allTabs = [];
    private static uint _currentTab;

    internal GUIStyle? Style;
    private GUIStyle? _resizeStyle;

    /// <summary>
    /// The current position and size of the cheat menu window.
    /// </summary>
    internal Rect WindowRect = new(50f, 50f, 650f, 750f);

    private readonly Vector2 _minWindowSize = new(500f, 500f);
    private readonly Vector2 _maxWindowSize = new(1200f, 900f);

    private Rect _resizeHandle = new(0, 0, 20, 20);
    private bool _isResizing;
    private Vector2 _menuScrollPos;
    private Vector2 _tabScrollPos;

    /// <summary>
    /// Initializes and loads the cheat menu system.
    /// </summary>
    public void Load()
    {
        _currentTab = 0;
        LoadTabs();
        _loaded = true;
    }

    /// <summary>
    /// Unloads and cleans up the cheat menu system.
    /// </summary>
    public void Unload()
    {
        _loaded = false;
        _allTabs.Clear();
        Style = null;
        _resizeStyle = null;
    }

    /// <summary>
    /// Dynamically loads all CheatMenuTab implementations from the assembly.
    /// </summary>
    private static void LoadTabs()
    {
        try
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            Type[] allTypes = assembly.GetTypes();
            List<Type> tabTypes = allTypes
                .Where(type =>
                    !type.IsAbstract &&
                    typeof(CheatMenuTab).IsAssignableFrom(type))
                .ToList();

            foreach (Type tabType in tabTypes)
            {
                try
                {
                    CheatMenuTab tabInstance = (CheatMenuTab)Activator.CreateInstance(tabType);
                    _allTabs.Add(tabInstance);
                }
                catch (Exception ex)
                {
                    CheatLogger.Error($"Failed to create instance of {tabType.Name}: {ex}");
                }
            }

            _allTabs = _allTabs.OrderBy(tab => tab.TabIndex).ToList();
        }
        catch (Exception ex)
        {
            CheatLogger.Error($"Failed to load tabs: {ex}");
        }
    }

    /// <summary>
    /// Updates the cheat menu state each frame.
    /// </summary>
    internal void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            _isOpen = !_isOpen;
        }
    }

    /// <summary>
    /// Renders the cheat menu GUI elements.
    /// </summary>
    internal void OnGUI()
    {
        if (!_loaded) return;


        Style ??= new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = Color.white },
            fontStyle = FontStyle.Bold,
            fontSize = 12,
            padding = new RectOffset(4, 4, 2, 2)
        };

        _resizeStyle ??= new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = new Color(0.7f, 0.7f, 0.7f, 0.8f) },
            alignment = TextAnchor.MiddleCenter,
            fontSize = 10,
            padding = new RectOffset(0, 0, 0, 0)
        };

        DrawWatermark();

        if (_isOpen)
        {

            WindowRect.width = Mathf.Clamp(WindowRect.width, _minWindowSize.x, _maxWindowSize.x);
            WindowRect.height = Mathf.Clamp(WindowRect.height, _minWindowSize.y, _maxWindowSize.y);

            WindowRect.x = Mathf.Clamp(WindowRect.x, 0, Screen.width - 50);
            WindowRect.y = Mathf.Clamp(WindowRect.y, 0, Screen.height - 50);

            WindowRect = GUILayout.Window(
                0,
                WindowRect,
                MenuContent,
                CheatInfo.Name,
                GUILayout.MinWidth(_minWindowSize.x),
                GUILayout.MinHeight(_minWindowSize.y)
            );

            _resizeHandle = new Rect(
                WindowRect.x + WindowRect.width - 24,
                WindowRect.y + WindowRect.height - 24,
                24,
                24
            );

            GUI.Box(_resizeHandle, "↘", _resizeStyle);
            HandleResize();
        }
    }

    /// <summary>
    /// Handles window resizing logic and mouse interaction.
    /// </summary>
    private void HandleResize()
    {
        Event currentEvent = Event.current;
        Vector2 mousePos = currentEvent.mousePosition;

        bool mouseOverHandle = _resizeHandle.Contains(mousePos);

        if (mouseOverHandle)
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        if (currentEvent.type == EventType.MouseDown && mouseOverHandle)
        {
            _isResizing = true;
            currentEvent.Use();
        }

        if (currentEvent.type == EventType.MouseUp)
        {
            _isResizing = false;
        }

        if (_isResizing && currentEvent.type == EventType.MouseDrag)
        {
            float newWidth = Mathf.Clamp(
                mousePos.x - WindowRect.x,
                _minWindowSize.x,
                _maxWindowSize.x
            );

            float newHeight = Mathf.Clamp(
                mousePos.y - WindowRect.y,
                _minWindowSize.y,
                _maxWindowSize.y
            );

            WindowRect = new Rect(
                WindowRect.x,
                WindowRect.y,
                newWidth,
                newHeight
            );

            currentEvent.Use();
        }
    }

    /// <summary>
    /// Draws the cheat watermark in the corner of the screen.
    /// </summary>
    private void DrawWatermark()
    {
        if (Style == null) return;

        GUI.backgroundColor = new Color(23f / 255f, 23f / 255f, 23f / 255f, 1f);
        GUI.contentColor = Color.white;
        GUI.color = Singleton<DataManager>.Instance.SettingsFile.c_Theme;

        string watermark = $"{CheatInfo.Name} | v{CheatInfo.Version}";
        if (!_isOpen)
        {
            watermark += " | Press INSERT";
        }

        GUI.Label(new Rect(10f, 5f, 500f, 25f), watermark, Style);
    }

    /// <summary>
    /// Draws a tooltip near the mouse cursor if tooltip text is available.
    /// </summary>
    internal void DrawTooltip()
    {
        if (string.IsNullOrEmpty(UI.Tooltip))
            return;

        GUIStyle tooltipStyle = GUI.skin.label;
        GUIContent tooltipContent = new(UI.Tooltip);

        float maxWidth = 300f;
        float tooltipWidth = Mathf.Min(tooltipStyle.CalcSize(tooltipContent).x + 20f, maxWidth);
        float tooltipHeight = tooltipStyle.CalcHeight(tooltipContent, tooltipWidth - 20f) + 20f;

        Vector2 mousePos = Event.current.mousePosition;

        float x = mousePos.x + 20f;
        float y = mousePos.y + 20f;

        if (x + tooltipWidth > Screen.width)
            x = Screen.width - tooltipWidth - 10f;

        if (y + tooltipHeight > Screen.height)
            y = mousePos.y - tooltipHeight - 10f;

        Rect tooltipRect = new(x, y, tooltipWidth, tooltipHeight);

        GUI.Box(tooltipRect, GUIContent.none, tooltipStyle);
        GUI.Label(new Rect(tooltipRect.x + 10f, tooltipRect.y + 10f,
            tooltipWidth - 20f, tooltipHeight - 20f), UI.Tooltip, tooltipStyle);
    }

    /// <summary>
    /// Renders the main menu content including tabs and current tab content.
    /// </summary>
    /// <param name="windowID">The Unity GUI window identifier.</param>
    private void MenuContent(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, WindowRect.width, 20));

        GUILayout.BeginVertical();
        GUILayout.Space(5);

        if (_allTabs.Count > 6)

        {
            _tabScrollPos = GUILayout.BeginScrollView(_tabScrollPos, false, true,
                GUILayout.Height(40), GUILayout.ExpandWidth(true));

            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            foreach (var tab in _allTabs)
            {

                UI.Tab(tab.TabName, ref _currentTab, tab.TabIndex, false, 120, 32);
                GUILayout.Space(2);

            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }
        else
        {
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();

            foreach (var tab in _allTabs)
            {
                UI.Tab(tab.TabName, ref _currentTab, tab.TabIndex, false, 100, 32);
                GUILayout.Space(4);

            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(2);

        UI.Divider(1.5f);
        GUILayout.Space(2);

        _menuScrollPos = GUILayout.BeginScrollView(_menuScrollPos,
            GUI.skin.scrollView, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        UI.ResetTooltip();

        var currentTab = _allTabs.FirstOrDefault(tab => tab.TabIndex == _currentTab);
        if (currentTab != null)
        {

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            GUILayout.Space(8);

            currentTab.OnGUI();

            GUILayout.Space(8);
            GUILayout.EndVertical();
        }
        else
        {
            _currentTab = 0;
            GUILayout.Label("No tab selected", GUI.skin.label);
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        DrawTooltip();
    }
}