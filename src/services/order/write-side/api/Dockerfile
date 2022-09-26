#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/order/write-side/api/api.csproj", "src/services/order/write-side/api/"]
COPY ["src/services/order/write-side/application/application.csproj", "src/services/order/write-side/application/"]
COPY ["src/services/order/write-side/domain/domain.csproj", "src/services/order/write-side/domain/"]
COPY ["src/core/core-domain/core-domain.csproj", "src/core/core-domain/"]
COPY ["src/services/order/write-side/infrastructure/infrastructure.csproj", "src/services/order/write-side/infrastructure/"]
COPY ["src/core/core-message/core-messages.csproj", "src/core/core-message/"]
COPY ["src/core/core-infrastructure/core-infrastructure.csproj", "src/core/core-infrastructure/"]
COPY ["src/core/core-application/core-application.csproj", "src/core/core-application/"]
RUN dotnet restore "src/services/order/write-side/api/api.csproj"
COPY . .
WORKDIR "/src/src/services/order/write-side/api"
RUN dotnet build "api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api.dll"]
