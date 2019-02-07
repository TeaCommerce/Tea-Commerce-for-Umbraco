@ECHO off
SETLOCAL

REM Parse command line arg if present
SET ARG1=%~1

REM If command line arg present, set the BUILD_CONFIG
REM otherwise, prompt the user
IF NOT "%ARG1%" == "" SET BUILD_CONFIG=%ARG1:~-1%
IF "%ARG1%" == "" SET /P BUILD_CONFIG=Please select the build configuration to use (r = Release, d = Debug [Default]):

REM Covert build config flag to an actual config string
if "%BUILD_CONFIG%" == "r" (
  SET BUILD_CONFIG=Release
) else (
  SET BUILD_CONFIG=Debug
)

REM Trigger the build
CALL Build\Tools\NuGet\NuGet.exe restore Source\TeaCommerceForUmbraco.sln
CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MsBuild.exe" Build\Build.proj

ENDLOCAL
IF %ERRORLEVEL% NEQ 0 GOTO err
EXIT /B 0
:err
PAUSE
EXIT /B 1

