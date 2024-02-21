FROM mcr.microsoft.com/dotnet/sdk:8.0 as build-env
WORKDIR /src
COPY ./GardenOS.Api/ /GardenOS.Api/
COPY ./GardenOS.Shared/ /GardenOS.Shared/
RUN dotnet restore /GardenOS.Api/API.csproj
RUN dotnet publish /GardenOS.Api/API.csproj -c Release -o /publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENV ASPNETCORE_URLS=http://+:5082
EXPOSE 5082
ENTRYPOINT ["dotnet", "API.dll"]