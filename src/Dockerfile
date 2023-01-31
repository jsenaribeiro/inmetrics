FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish
WORKDIR /src
COPY . .

RUN dotnet restore "Desafio.Application/Desafio.Application.csproj"
RUN dotnet publish "Desafio.Application/Desafio.Application.csproj" -c Release -o /bin

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443
EXPOSE 5000
EXPOSE 5001

COPY --from=publish /bin .
ENTRYPOINT ["dotnet", "Desafio.Application.dll"]
