FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gorgonix-Back.Api/Gorgonix-Back.Api.csproj", "Gorgonix-Back.Api/"]
COPY ["Gorgonix-Back.Application/Gorgonix-Back.Application.csproj", "Gorgonix-Back.Application/"]
COPY ["Gorgonix-Back.Domain/Gorgonix-Back.Domain.csproj", "Gorgonix-Back.Domain/"]
COPY ["Gorgonix-Back.Infrastructure/Gorgonix-Back.Infrastructure.csproj", "Gorgonix-Back.Infrastructure/"]
RUN dotnet restore "Gorgonix-Back.Api/Gorgonix-Back.Api.csproj"
COPY . .
WORKDIR "/src/Gorgonix-Back.Api"
RUN dotnet build "./Gorgonix-Back.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gorgonix-Back.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gorgonix-Back.Api.dll"]
