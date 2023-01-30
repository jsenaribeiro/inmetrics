@echo off

if "%1"=="" (
	echo "waiting for a database (ex.: ef-reset mysql)"
	goto :ENDLOOP
)

set CONNECTION=
cd ../src/Desafio.Infrastructure

:LOOP
if "%1"=="" goto :ENDLOOP
set DATABASE=%1

echo ----------------------------- %1 ----------------------------------
dotnet ef database drop -f
rd Migrations /s /q 
dotnet ef migrations add initialDb || goto :ENDLOOP
dotnet ef database update || goto :ENDLOOP

shift
goto LOOP

:ENDLOOP
cd ../../run
exit /b %errorlevel%
