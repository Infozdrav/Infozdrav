FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Infozdrav.Web/Infozdrav.Web.csproj Infozdrav.Web/
RUN dotnet restore Infozdrav.Web/Infozdrav.Web.csproj
COPY . .
WORKDIR /src/Infozdrav.Web
RUN dotnet build Infozdrav.Web.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Infozdrav.Web.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
VOLUME /app/uploads
ENTRYPOINT ["dotnet", "Infozdrav.Web.dll"]
