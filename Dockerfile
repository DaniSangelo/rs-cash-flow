FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
# creates the folder (if it does not exist) and gets inside it
WORKDIR /app

COPY src/ .

# if folder exists then only gets inside it
WORKDIR /app/CashFlow.API

RUN dotnet restore
RUN dotnet publish -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# copy files from build-env image, defined at the top of Dockerfile
COPY --from=build-env /app/out .

ENTRYPOINT [ "dotnet", "CashFlow.API.dll" ]