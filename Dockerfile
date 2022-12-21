FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/Host/Host.csproj", "src/Host/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Client/Client.csproj", "src/Client/"]
COPY ["src/Client.Infrastructure/Client.Infrastructure.csproj", "src/Client.Infrastructure/"]

RUN dotnet restore "src/Host/Host.csproj"

COPY . .
WORKDIR "/src/src/Host"

RUN dotnet publish "Host.csproj" -c Release --no-restore -o /app/publish

# Build the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/publish .

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


ENV ASPNETCORE_URLS=https://+:5002;http://+:5003
ENV ApiBaseUrl=https://localhost:5050/
EXPOSE 5002
EXPOSE 5003

ENTRYPOINT ["dotnet", "Edu.BlazorWebAssembly.Host.dll"]