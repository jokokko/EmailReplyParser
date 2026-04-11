@echo off

dotnet tool restore
dotnet paket restore
if errorlevel 1 (
  exit /b %errorlevel%
)

setlocal
dotnet fake run build.fsx %*
endlocal