#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["sources/presentation/Synapse.Demo.WebServer/Synapse.Demo.WebServer.csproj", "sources/presentation/Synapse.Demo.WebServer/"]
COPY ["sources/infrastructure/Synapse.Demo.Infrastructure/Synapse.Demo.Infrastructure.csproj", "sources/infrastructure/Synapse.Demo.Infrastructure/"]
COPY ["sources/core/Synapse.Demo.Application/Synapse.Demo.Application.csproj", "sources/core/Synapse.Demo.Application/"]
COPY ["sources/core/Synapse.Demo.Common/Synapse.Demo.Common.csproj", "sources/core/Synapse.Demo.Common/"]
COPY ["sources/core/Synapse.Demo.Domain/Synapse.Demo.Domain.csproj", "sources/core/Synapse.Demo.Domain/"]
COPY ["sources/core/Synapse.Demo.Integration/Synapse.Demo.Integration.csproj", "sources/core/Synapse.Demo.Integration/"]
COPY ["sources/infrastructure/Synapse.Demo.Persistence/Synapse.Demo.Persistence.csproj", "sources/infrastructure/Synapse.Demo.Persistence/"]
COPY ["sources/presentation/Synapse.Demo.Api.Rest/Synapse.Demo.Api.Rest.csproj", "sources/presentation/Synapse.Demo.Api.Rest/"]
COPY ["sources/presentation/Synapse.Demo.Api.WebSocket/Synapse.Demo.Api.WebSocket.csproj", "sources/presentation/Synapse.Demo.Api.WebSocket/"]
COPY ["sources/presentation/Synapse.Demo.WebUI/Synapse.Demo.WebUI.csproj", "sources/presentation/Synapse.Demo.WebUI/"]
COPY ["sources/presentation/Synapse.Demo.Client.Rest/Synapse.Demo.Client.Rest.csproj", "sources/presentation/Synapse.Demo.Client.Rest/"]
RUN dotnet restore "sources/presentation/Synapse.Demo.WebServer/Synapse.Demo.WebServer.csproj"
COPY . .
WORKDIR "/src/sources/presentation/Synapse.Demo.WebServer"
RUN dotnet build "Synapse.Demo.WebServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Synapse.Demo.WebServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Synapse.Demo.WebServer.dll"]