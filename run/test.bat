@echo off
cd ..\src
dotnet test --logger:"console;verbosity=detailed"
cd ..\run