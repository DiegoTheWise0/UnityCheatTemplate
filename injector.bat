@echo off
setlocal enabledelayedexpansion

set project_name=UnityCheatTemplate
set game_name=GameName
set injector_path=submodules\SharpMonoInjector\dist\SharpMonoInjector.exe
set dll_path=src\bin\%project_name%.dll
set project_path=src\%project_name%.csproj
set auto_build=true

title SharpMonoInjector Manager
mode con: cols=70 lines=25

:main_menu
cls
echo.
echo    ========================================
echo         SHARPMONOINJECTOR MANAGER
echo    ========================================
echo.
echo        Project: %project_name%
echo        Target:  %game_name%
echo.
echo    ========================================
echo.
echo        [1] Inject DLL into Process
echo        [2] Build Project Only
echo        [3] Rebuild Injector
echo        [4] Update Settings
echo        [X] Exit
echo.
echo    ========================================
echo.
set /p choice=    Select option [1-4, X]: 

if /i "!choice!"=="1" goto inject
if /i "!choice!"=="2" goto build_only
if /i "!choice!"=="3" goto rebuild_injector
if /i "!choice!"=="4" goto update_settings
if /i "!choice!"=="X" goto exit_program
goto main_menu

:inject
cls
echo.
echo    ========================================
echo              INJECTING DLL
echo    ========================================
echo.

REM Check if we should auto-build
if /i "!auto_build!"=="true" (
    echo    Auto-build enabled. Building project...
    if not exist "!project_path!" (
        echo    [ERROR] Project not found: !project_path!
        echo.
        pause
        goto main_menu
    )
    
    dotnet build "!project_path!" -c Release --nologo
    if errorlevel 1 (
        echo    [ERROR] Build failed!
        echo.
        pause
        goto main_menu
    )
    
    if not exist "!dll_path!" (
        echo    [ERROR] DLL not found after build: !dll_path!
        echo.
        pause
        goto main_menu
    )
    echo    Build completed.
) else (
    echo    Auto-build disabled. Using existing DLL.
    if not exist "!dll_path!" (
        echo    [ERROR] DLL not found: !dll_path!
        echo    Enable auto-build or build manually first.
        echo.
        pause
        goto main_menu
    )
)

echo    Checking for %game_name% process...
tasklist /FI "IMAGENAME eq %game_name%.exe" 2>nul | find /I "%game_name%.exe" >nul
if errorlevel 1 (
    echo    [WARNING] %game_name% is not running.
    echo    Start the game and try again.
    echo.
    pause
    goto main_menu
)

echo    Injecting %project_name%.dll...
if not exist "!injector_path!" call :ensure_injector
if not exist "!injector_path!" (
    echo    [ERROR] SharpMonoInjector.exe not found!
    echo.
    pause
    goto main_menu
)

"!injector_path!" inject -p "%game_name%" -a "!dll_path!" -n %project_name% -c Loader -m Load
if errorlevel 1 (
    echo    [ERROR] Injection failed!
) else (
    echo    [SUCCESS] Injection completed!
)

echo.
pause
color
goto main_menu

:build_only
cls
echo.
echo    ========================================
echo              BUILDING PROJECT
echo    ========================================
echo.
echo    Building %project_name%...

if not exist "!project_path!" (
    echo    [ERROR] Project not found: !project_path!
) else (
    dotnet build "!project_path!" -c Release --nologo
    if errorlevel 1 (
        echo    [ERROR] Build failed!
    ) else (
        echo    [SUCCESS] Build completed!
    )
)
echo.
pause
goto main_menu

:rebuild_injector
cls
echo.
echo    ========================================
echo           REBUILDING INJECTOR
echo    ========================================
echo.
call :ensure_injector force
echo.
pause
goto main_menu

:update_settings
cls
echo.
echo    ========================================
echo             UPDATE SETTINGS
echo    ========================================
echo.
echo    Current Project Name: %project_name%
set /p new_project=    New Project Name (Enter to keep): 
if not "!new_project!"=="" set project_name=!new_project!

echo.
echo    Current Game Name: %game_name%
set /p new_game=    New Game Name (Enter to keep): 
if not "!new_game!"=="" set game_name=!new_game!

set injector_path=submodules\SharpMonoInjector\dist\SharpMonoInjector.exe
set dll_path=src\bin\%project_name%.dll
echo.
echo    [SUCCESS] Settings updated!
echo.
pause
goto main_menu

:exit_program
cls
echo.
echo    ========================================
echo                 GOODBYE
echo    ========================================
echo.
echo    Closing SharpMonoInjector Manager...
echo.
timeout /t 2 /nobreak >nul
exit /b 0

:ensure_injector
if "%~1"=="force" (
    echo    Forcing rebuild of SharpMonoInjector...
    if exist "submodules\SharpMonoInjector" rmdir /s /q "submodules\SharpMonoInjector" 2>nul
)

if exist "!injector_path!" (
    if not "%~1"=="force" (
        echo    SharpMonoInjector.exe already exists.
        exit /b 0
    )
)

echo    Ensuring SharpMonoInjector is available...
if not exist "submodules\SharpMonoInjector\SharpMonoInjector.csproj" (
    echo    Downloading SharpMonoInjectorCore...
    powershell -Command "Invoke-WebRequest -Uri 'https://github.com/winstxnhdw/SharpMonoInjectorCore/archive/refs/heads/main.zip' -OutFile 'temp.zip'" >nul 2>&1
    if errorlevel 1 (
        echo    [ERROR] Download failed.
        exit /b 1
    )
    
    echo    Extracting...
    powershell -Command "Expand-Archive -Path 'temp.zip' -DestinationPath '.' -Force" >nul 2>&1
    
    if not exist "submodules\SharpMonoInjector" mkdir "submodules\SharpMonoInjector"
    
    if exist "SharpMonoInjectorCore-main" (
        xcopy /E /I /Y "SharpMonoInjectorCore-main\*" "submodules\SharpMonoInjector\" >nul
        rmdir /s /q "SharpMonoInjectorCore-main" 2>nul
    ) else (
        xcopy /E /I /Y "SharpMonoInjector-main\*" "submodules\SharpMonoInjector\" >nul
        rmdir /s /q "SharpMonoInjector-main" 2>nul
    )
    del "temp.zip" 2>nul
)

echo    Building SharpMonoInjector...
dotnet --info >nul 2>&1
if errorlevel 1 (
    echo    [ERROR] .NET SDK required.
    exit /b 1
)

cd "submodules\SharpMonoInjector" && dotnet publish --verbosity quiet >nul 2>&1
if errorlevel 1 cd "submodules\SharpMonoInjector" && dotnet build >nul 2>&1
cd ..\..

if exist "!injector_path!" (
    echo    [SUCCESS] SharpMonoInjector ready!
) else (
    echo    [ERROR] Failed to build SharpMonoInjector.
)
exit /b 0