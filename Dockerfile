FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS="http://+:5000"
ENV ASPNETCORE_ENVIRONMENT="Production"

EXPOSE 5000
EXPOSE 5001

ENTRYPOINT ["dotnet", "PlotSystem-API.dll"]