@ECHO off
SETLOCAL

SET ARG1=%~1
SET ARG2=%~2
SET ARG3=%~3

IF NOT "%ARG1%" == "" SET BUILD_CONFIG=%ARG1:~-1%
IF "%ARG1%" == "" SET /P BUILD_CONFIG=Please select the build configuration to use (r = Release, d = Debug [Default]):
IF "%BUILD_CONFIG%" == "" SET BUILD_CONFIG=d

IF NOT "%ARG2%" == "" SET BUILD_PAYMENT_PROVIDERS=%ARG2:~-1%
IF "%ARG2%" == "" SET /P BUILD_PAYMENT_PROVIDERS=Should the TC Payment Providers project be compiled? (y = Yes, n = No [Default]):
IF "%BUILD_PAYMENT_PROVIDERS%" == "p" SET BUILD_PAYMENT_PROVIDERS=y

IF NOT "%ARG3%" == "" SET BUILD_CORE=%ARG3:~-1%
IF "%ARG3%" == "" SET /P BUILD_CORE=Should the TC Core project be compiled? (y = Yes, n = No [Default]):
IF "%BUILD_CORE%" == "c" SET BUILD_CORE=y

IF "%BUILD_PAYMENT_PROVIDERS%" == "y" (
    DEL /q Source\Lib\TeaCommercePaymentProviders\*.dll
    CD "%~dp0..\PaymentProviders\"
    CALL Build.cmd %BUILD_CONFIG%
    CD "%~dp0..\TeaCommerceForUmbraco\"
    XCOPY "%~dp0..\PaymentProviders\Artifacts" Source\Lib\TeaCommercePaymentProviders /E
)

IF "%BUILD_CORE%" == "y" (
    DEL /q Source\Lib\TeaCommerce\*.dll
    CD "%~dp0..\TeaCommerce\"
    CALL Build.cmd %BUILD_CONFIG%
    CD "%~dp0..\TeaCommerceForUmbraco\"
    XCOPY "%~dp0..\TeaCommerce\Artifacts" Source\Lib\TeaCommerce /E
)

CALL Build.cmd %BUILD_CONFIG%

ENDLOCAL
IF %ERRORLEVEL% NEQ 0 GOTO err
EXIT /B 0
:err
PAUSE
EXIT /B 1

