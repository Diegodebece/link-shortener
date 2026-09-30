FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY LinkShortener.sln ./
COPY src/LinkShortener.Domain/LinkShortener.Domain.csproj src/LinkShortener.Domain/
COPY src/LinkShortener.Application/LinkShortener.Application.csproj src/LinkShortener.Application/
COPY src/LinkShortener.Infrastructure/LinkShortener.Infrastructure.csproj src/LinkShortener.Infrastructure/
COPY src/LinkShortener.Api/LinkShortener.Api.csproj src/LinkShortener.Api/

RUN dotnet restore src/LinkShortener.Api/LinkShortener.Api.csproj

COPY . .

RUN dotnet publish src/LinkShortener.Api/LinkShortener.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LinkShortener.Api.dll"]