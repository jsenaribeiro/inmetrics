@echo off

cd /D "%~dp0"
docker-compose down

if "%1"=="--build" (
   cd ..\src
   docker build . -t desafio-webapi
   cd ..\run
)

docker-compose up