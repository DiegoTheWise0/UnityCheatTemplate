using System.Reflection;
using UnityCheatTemplate.Enums;
using UnityCheatTemplate.Interfaces;
using UnityCheatTemplate.Utilities;
using UnityEngine;

namespace UnityCheatTemplate.Modules.Menu.Core;

internal class CheatMenuUI : ILoadable, ISingleton
{
    private bool _loaded;
    private bool _isOpen;

    private static List<CheatMenuTab> _allTabs = [];
    private static UiTabs _currentTab;

    private GUIStyle? _style;
    internal Rect WindowRect = new(50f, 50f, 545f, 715f);

    // Add min/max size constraints
    private readonly Vector2 _minWindowSize = new(400f, 400f);
    private readonly Vector2 _maxWindowSize = new(1200f, 900f);

    // For tracking window dragging and resizing
    private Rect _resizeHandle = new(0, 0, 20, 20);
    private bool _isResizing;

    public void Load()
    {
        _currentTab = UiTabs.About;
        LoadTabs();
        _loaded = true;
    }

    public void Unload()
    {
        _loaded = false;
        _allTabs.Clear();
        _style = null;
    }

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

            _allTabs = _allTabs.OrderBy(tab => tab.UiTabType).ToList();
        }
        catch (Exception ex)
        {
            CheatLogger.Error($"Failed to load tabs: {ex}");
        }
    }

    internal void Update()
    {
        if (Input.GetKeyDown(KeyCode.Insert))
        {
            _isOpen = !_isOpen;
        }
    }

    internal void OnGUI()
    {
        if (!_loaded) return;

        GUI.skin = Theme.Skin;
        _style ??= new GUIStyle(GUI.skin.label)
        {
            normal = { textColor = Color.white },
            fontStyle = FontStyle.Bold
        };

        DrawWatermark();

        if (_isOpen)
        {
            WindowRect = GUILayout.Window(
                0,
                WindowRect,
                MenuContent,
                CheatInfo.Name
            );

            _resizeHandle = new Rect(
                WindowRect.x + WindowRect.width - 20,
                WindowRect.y + WindowRect.height - 20,
                20,
                20
            );

            GUI.Box(_resizeHandle, "↙", _style);
            HandleResize();
        }
    }

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

    private void DrawWatermark()
    {
        if (_style == null) return;

        GUI.backgroundColor = new Color(23f / 255f, 23f / 255f, 23f / 255f, 1f);
        GUI.contentColor = Color.white;
        GUI.color = Color.white;

        string watermark = $"{CheatInfo.Name} | v{CheatInfo.Version}";
        if (!_isOpen)
        {
            watermark += " | Press INSERT";
        }

        GUI.Label(new Rect(10f, 5f, 500f, 25f), watermark, _style);
    }

    internal void DrawTooltip()
    {
        if (string.IsNullOrEmpty(UI.Tooltip))
            return;

        GUIStyle tooltipStyle = GUI.skin.label;
        GUIContent tooltipContent = new(UI.Tooltip);
        float tooltipWidth = tooltipStyle.CalcSize(tooltipContent).x + 10f;
        float tooltipHeight = tooltipStyle.CalcHeight(tooltipContent, tooltipWidth - 10f) + 10f;

        Vector2 mousePos = Event.current.mousePosition;
        var theme = Color.white;
        GUI.color = new Color(theme.r, theme.g, theme.b, 0.8f);

        Rect tooltipRect = new(mousePos.x + 20f, mousePos.y + 20f, tooltipWidth, tooltipHeight);
        GUI.Box(tooltipRect, GUIContent.none);

        GUI.color = Color.white;
        GUI.Label(new Rect(tooltipRect.x + 5f, tooltipRect.y + 5f, tooltipWidth - 10f, tooltipHeight - 10f), UI.Tooltip);
    }

    private Vector2 _menuScrollPos;
    private void MenuContent(int windowID)
    {
        GUI.DragWindow(new Rect(0, 0, WindowRect.width, 20));
        GUILayout.BeginHorizontal();
        foreach (var tab in _allTabs)
        {
            UI.Tab(tab.TabName, ref _currentTab, tab.UiTabType);
        }
        GUILayout.EndHorizontal();

        _menuScrollPos = GUILayout.BeginScrollView(_menuScrollPos);
        UI.ResetTooltip();

        var currentTab = _allTabs.FirstOrDefault(tab => tab.UiTabType == _currentTab);
        if (currentTab != null)
        {
            currentTab.OnGUI();
        }
        else
        {
            _currentTab = UiTabs.About;
        }

        GUILayout.EndScrollView();
        DrawTooltip();

        GUI.DragWindow(new Rect(0, 0, 0, 20));
    }
}