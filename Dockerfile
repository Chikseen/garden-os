FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
WORKDIR /src
COPY . .
RUN dotnet restore ./main-service/MainService.csproj
RUN dotnet publish ./main-service/MainService.csproj -c Release -o /publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENV ASPNETCORE_URLS=http://+:5082
EXPOSE 5082
ENTRYPOINT ["dotnet", "MainService.dll"]