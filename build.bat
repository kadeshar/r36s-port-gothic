@echo off
setlocal enabledelayedexpansion

echo ========================================================
echo Building Gothic Port Preparer for R36S (.NET Framework 4.8)
echo ========================================================

set "MSBUILD_PATH="

for %%p in (
    "C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe"
    "C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe"
    "C:\Program Files\Microsoft Visual Studio\2022\Professional\MSBuild\Current\Bin\MSBuild.exe"
    "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\MSBuild\Current\Bin\MSBuild.exe"
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
) do (
    if exist %%p (
        set "MSBUILD_PATH=%%~p"
        goto :found_msbuild
    )
)

:found_msbuild
if "%MSBUILD_PATH%"=="" (
    echo [ERROR] MSBuild was not found. Please install Visual Studio or .NET Build Tools.
    exit /b 1
)

echo Using MSBuild: %MSBUILD_PATH%

cd /d "%~dp0"

"%MSBUILD_PATH%" "GothicPortPreparer\GothicPortPreparer.csproj" /t:Restore,Build /p:Configuration=Release
if errorlevel 1 (
    echo [ERROR] Build failed.
    exit /b %errorlevel%
)

echo.
echo Packaging distribution folder...
if not exist "dist" mkdir "dist"
if not exist "dist\GothicPortPreparer" mkdir "dist\GothicPortPreparer"

xcopy /y /e /i "GothicPortPreparer\bin\Release\net48\*" "dist\GothicPortPreparer\"

echo.
echo ========================================================
echo Build and packaging successful!
echo Binary ready at: dist\GothicPortPreparer\GothicPortPreparer.exe
echo echo ========================================================
