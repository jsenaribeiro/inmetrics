FROM mcr.microsoft.com/dotnet/sdk:6.0 AS publish
WORKDIR /src
COPY . .
RUN dotnet restore "Application/T3mplat3.Application.csproj"
RUN dotnet publish "Application/T3mplat3.Application.csproj" -c Release -o /bin

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 1433
COPY --from=publish /bin .
ENTRYPOINT ["dotnet", "T3mplat3.Application.dll"]
