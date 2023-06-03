FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env
COPY ./shared-data shared-data
COPY ./main-service main-service
WORKDIR /src
COPY *.csproj .
WORKDIR /main-service
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /publish
FROM mcr.microsoft.com/dotnet/aspnet:7.0 as runtime
WORKDIR /publish
COPY --from=build-env /publish .
ENV ASPNETCORE_URLS=http://+:5082
EXPOSE 5082
ENTRYPOINT ["dotnet", "MainService.dll"]