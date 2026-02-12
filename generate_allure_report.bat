
@echo off
setlocal
chcp 65001 >nul
pushd "%~dp0"

rem === НАСТРОЙКА ===
set "TFM=net10.0"
set "CS_PROJ=TesterBudAutomationFramework.csproj"
set "ALLURE_EXE=D:\Tools\allure-2.36.0\bin\allure.bat"
set "ALLURE_RESULTS=.\bin\Debug\%TFM%\allure-results"
set "ALLURE_REPORT=.\bin\Debug\%TFM%\allure-report"
set "SERILOG_DIR=.\bin\Debug\%TFM%\logs"

rem === Чистим прошлые данные ===
if exist "%ALLURE_REPORT%"  rmdir /s /q "%ALLURE_REPORT%"
if exist "%ALLURE_RESULTS%" rmdir /s /q "%ALLURE_RESULTS%"
if exist "%SERILOG_DIR%"    rmdir /s /q "%SERILOG_DIR%"

mkdir "%ALLURE_RESULTS%" >nul 2>&1
mkdir "%SERILOG_DIR%"    >nul 2>&1

rem === Запуск тестов ===
dotnet test "%CS_PROJ%" -c Debug

rem === Allure serve ===
call "%ALLURE_EXE%" serve "%ALLURE_RESULTS%"

pause
popd
exit /b 0
