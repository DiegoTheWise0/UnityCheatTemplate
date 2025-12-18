# Unity Cheat Template

A clean template for Unity game cheats with ESP, menu system, and hotkeys.

## 🔧 **Essential Modifications**

### **1. Basic Info** (`CheatInfo.cs`)
```csharp
// Change these 3 values:
internal const string Name = "YOUR_CHEAT_NAME";
internal static Version Version = new(1, 0, 0);
internal const string GUID = "com.yourname.cheat";
```

### **2. Add Your Settings** (`Data/Json/SettingsFile.cs`)
```csharp
// Add new variables here:
public bool b_YourToggle = true;
public float f_YourValue = 50f;
```

### **3. Create Menu Tabs** (`Modules/Menu/Tabs`)
```csharp
// Create new class:
internal class YourTab : CheatMenuTab
{
    internal override string TabName => "Your Tab";
    internal override uint TabIndex => 1;
    
    internal override void OnGUI()
    {
        UI.Checkbox(ref b_YourToggle, "Enable Feature");
        UI.FloatSlider(ref f_YourValue, 0, 100, "Slider");
    }
}
```

### **4. Add Your Cheat Logic** (`Modules/Cheats/`)
```csharp
internal sealed class YourCheat : ISingleton
{
    internal void Update() { /* Game logic */ }
    internal void OnGUI() { /* ESP/Drawing */ }
}
```

### **5. Register in MonoCheat**
```csharp
public void Update()
{
    Singleton<YourCheat>.Instance.Update(); // Add this
}
```

## 🎯 **Project Setup**

### **Target Framework**
You'll need to find out what .NET version the game uses.
Check the game's `Assembly-CSharp.dll` with a tool like dnSpy or ILSpy.

### **CSProj Configuration** (`src/UnityCheatTemplate.csproj`)
```xml
<TargetFramework>net48</TargetFramework> <!-- Change to match game's .NET version -->
```

### **Reference Folders**
Add required DLLs:
```
src/
├── References/
│   ├── Game/         # Game-specific DLLs (Assembly-CSharp.dll, etc.)
│   ├── Unity/        # Unity engine DLLs (UnityEngine.dll, UnityEngine.CoreModule.dll, etc.)
│   └── Dependencies/ # Dependency DLLs that aren't in the original game (0Harmony.dll, etc.)
```

## 📦 **Game Object Tracking** (`CatchMono.cs`)
Track all instances of specific MonoBehaviour types:
```csharp
// In your component's Awake():
CatchMono<PlayerController>.Register(this);

// In OnDestroy():
CatchMono<PlayerController>.Unregister(this);

// Access all instances:
foreach (var player in CatchMono<PlayerController>.AllMonos)
{
    // Do something with each player
}
```

## 🔌 **Dependency System** (`DependencyResolver.cs`)
Embed external DLLs in your cheat:
1. Place DLLs in `Resources/Dependencies/` folder
2. Add to `dependencies.json`:
```json
{
  "Dependencies": [
    "0Harmony.dll",
    "MonoMod.RuntimeDetour.dll"
  ]
}
```
3. DLLs auto-load at runtime

## 🛠 **Injector Script** (`injector.bat`)
One-click build and injection:

**Features:**
- Auto-builds your project
- Injects DLL into running game process
- Downloads SharpMonoInjector if missing
- Configurable settings

**Setup:**
```batch
# Edit these lines in injector.bat:
set project_name=UnityCheatTemplate    # Your project name
set game_name=GameName                  # Game's .exe name (without .exe)
```

**Usage:**
1. Run `injector.bat`
2. Select `[1] Inject DLL into Process`
3. It builds your cheat and injects it automatically

## 🎯 **Key Systems**
- **CatchMono<T>** - Track all instances of MonoBehaviour types
- **Singleton<T>** - Global access: `Singleton<YourClass>.Instance`
- **UI Utilities** - Pre-made checkboxes, sliders, color pickers
- **ESP System** - `RenderEspElement()` for boxes and tracers
- **Hotkeys** - `KeyBinder.Bind()` with modifier keys
- **Reflection** - Access private fields/methods safely
- **Dependency Injection** - Embed external DLLs
- **Auto Injector** - One-click build & inject

## 📁 **File Structure**
```
src/
├── Data/              - Settings & JSON
├── Extensions/        - Reflection tools
├── Modules/
│   ├── Cheats/        - ESP, Trigger, etc.
│   └── Menu/          - UI system
├── References/        # ADD REQUIRED DLLS HERE
│   ├── Game/          # Assembly-CSharp.dll, GameAssembly.dll
│   └── Unity/         # UnityEngine.dll, UnityEditor.dll
├── Resources/
│   └── Dependencies/  # Place external DLLs here
└── injector.bat       # Build & inject script
```

**Get started:**
1. **Find game's .NET version** (check Assembly-CSharp.dll)
2. **Update .csproj** with correct `TargetFramework`
3. **Add required DLLs** to References/Game and References/Unity
4. Change `CheatInfo.cs`
5. Add settings in `SettingsFile.cs`
6. Create menu tabs
7. Add cheat logic
8. Run `injector.bat`
