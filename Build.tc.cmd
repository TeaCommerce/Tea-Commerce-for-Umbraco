@ECHO off
SETLOCAL

SET ARG1=%~1
SET ARG2=%~2
SET ARG3=%~3
SET ARG3=%~4

SET /P NA1=Have you pulled down the latest source for each repo?
SET /P NA2=Have your updated the version.txt file in each repo?

IF NOT "%ARG1%" == "" SET BUILD_CONFIG=%ARG1:~-1%
IF "%ARG1%" == "" SET /P BUILD_CONFIG=Please select the build configuration to use (r = Release, d = Debug [Default]):
IF "%BUILD_CONFIG%" == "" SET BUILD_CONFIG=d

IF NOT "%ARG2%" == "" SET BUILD_TARGET=%ARG2:~-1%
IF "%ARG2%" == "" SET /P BUILD_TARGET=Please select the build target to use (b = build only [Default], u = build and package umbraco, n = build and package nuget, a = build and package all):
IF "%BUILD_TARGET%" == "" SET BUILD_TARGET=b

IF NOT "%ARG3%" == "" SET BUILD_PAYMENT_PROVIDERS=%ARG3:~-1%
IF "%ARG3%" == "" SET /P BUILD_PAYMENT_PROVIDERS=Should the TC Payment Providers project be compiled? (y = Yes, n = No [Default]):
IF "%BUILD_PAYMENT_PROVIDERS%" == "p" SET BUILD_PAYMENT_PROVIDERS=y

IF NOT "%ARG4%" == "" SET BUILD_CORE=%ARG4:~-1%
IF "%ARG4%" == "" SET /P BUILD_CORE=Should the TC Core project be compiled? (y = Yes, n = No [Default]):
IF "%BUILD_CORE%" == "c" SET BUILD_CORE=y

IF "%BUILD_PAYMENT_PROVIDERS%" == "y" (
    DEL /q Source\Lib\TeaCommercePaymentProviders\*.dll
    CD "%~dp0..\PaymentProviders\"
    CALL Build.cmd %BUILD_CONFIG% %BUILD_TARGET%
    CD "%~dp0..\TeaCommerceForUmbraco\"
    COPY "%~dp0..\PaymentProviders\Artifacts\Files\bin\TeaCommerce.PaymentProviders.dll" Source\Lib\TeaCommercePaymentProviders\TeaCommerce.PaymentProviders.dll /Y
)

IF "%BUILD_CORE%" == "y" (
    DEL /q Source\Lib\TeaCommerce\*.dll
    CD "%~dp0..\TeaCommerce\"
    CALL Build.cmd %BUILD_CONFIG% %BUILD_TARGET%
    CD "%~dp0..\TeaCommerceForUmbraco\"
    XCOPY "%~dp0..\TeaCommerce\Artifacts\Files\bin" Source\Lib\TeaCommerce /E /Y
)

CALL Build.cmd %BUILD_CONFIG% %BUILD_TARGET%

ENDLOCAL
IF %ERRORLEVEL% NEQ 0 GOTO err
EXIT /B 0
:err
PAUSE
EXIT /B 1

