@echo off
cd /D "%~dp0"
cd ..\src
dotnet test --logger:"console;verbosity=detailed"
cd ..\run