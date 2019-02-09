@ECHO off
SETLOCAL

REM Parse command line arg if present
SET ARG1=%~1
SET ARG2=%~2

REM If command line arg present, set the BUILD_CONFIG
REM otherwise, prompt the user
IF NOT "%ARG1%" == "" SET BUILD_CONFIG=%ARG1:~-1%
IF "%ARG1%" == "" SET /P BUILD_CONFIG=Please select the build configuration to use (r = Release, d = Debug [Default]):

IF NOT "%ARG2%" == "" SET BUILD_TARGET=%ARG2:~-1%
IF "%ARG2%" == "" SET /P BUILD_TARGET=Please select the build target to use (b = build only [Default], u = build and package umbraco, n = build and package nuget, a = build and package all):
IF "%BUILD_TARGET%" == "" SET BUILD_TARGET=b

REM Covert build config flag to an actual config string
if "%BUILD_CONFIG%" == "r" (
  SET BUILD_CONFIG=Release
) else (
  SET BUILD_CONFIG=Debug
)

REM Covert build target flag to an actual config string
if "%BUILD_TARGET%" == "a" (
  SET BUILD_TARGET=BuildAndPackageAll
) else (
  if "%BUILD_TARGET%" == "n" (
    SET BUILD_TARGET=BuildAndPackageNuget
  ) else (
    if "%BUILD_TARGET%" == "u" (
      SET BUILD_TARGET=BuildAndPackageUmbraco
    ) else (
      SET BUILD_TARGET=BuildOnly
    )
  )
)

REM Trigger the build
CALL Build\Tools\NuGet\NuGet.exe restore Source\TeaCommerceForUmbraco.sln
CALL "%programfiles(x86)%\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\amd64\MsBuild.exe" Build\Build.proj -target:%BUILD_TARGET%

ENDLOCAL
IF %ERRORLEVEL% NEQ 0 GOTO err
EXIT /B 0
:err
PAUSE
EXIT /B 1

