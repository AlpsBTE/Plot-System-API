FROM mcr.microsoft.com/dotnet/sdk:9.0

RUN mkdir /src
WORKDIR /src
ADD . /src
RUN dotnet restore
RUN ["dotnet", "dev-certs", "https"]
RUN ["dotnet", "dev-certs", "https", "--trust"]
RUN ["dotnet", "build"]

EXPOSE 5000
EXPOSE 5001

ENV ASPNETCORE_URLS="http://+:5000;https://+:5001"
ENV ASPNETCORE_ENVIRONMENT="Production"

ENTRYPOINT ["dotnet", "run", "--project", "PlotSystem-API"]