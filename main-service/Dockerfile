FROM mcr.microsoft.com/dotnet/sdk:7.0 AS base
WORKDIR /app
EXPOSE 5082

ENV ASPNETCORE_URLS=http://+:5082

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["main-service.csproj", "./"]
RUN dotnet restore "main-service.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "main-service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "main-service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "main-service.dll"]