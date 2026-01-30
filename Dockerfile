# ETAPA 1: BUILD (Construcción)
# Usamos la imagen del SDK de .NET 8 para compilar el código
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copiamos los archivos de proyecto (.csproj) y restauramos dependencias
# Esto se hace antes de copiar todo el código para aprovechar la caché de Docker
COPY *.sln .
COPY Gorgonix-Back.Api/*.csproj ./Gorgonix-Back.Api/
COPY Gorgonix-Back.Application/*.csproj ./Gorgonix-Back.Application/
COPY Gorgonix-Back.Domain/*.csproj ./Gorgonix-Back.Domain/
COPY Gorgonix-Back.Infrastructure/*.csproj ./Gorgonix-Back.Infrastructure/

RUN dotnet restore

# Copiamos el resto del código fuente
COPY . .

# Publicamos la aplicación en modo Release
WORKDIR /app/Gorgonix-Back.Api
RUN dotnet publish -c Release -o /app/out

# ETAPA 2: RUNTIME (Ejecución)
# Usamos una imagen más ligera solo con lo necesario para correr la app (sin compilador)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiamos los archivos compilados de la etapa anterior
COPY --from=build /app/out .

# Exponemos los puertos (8080 es el estándar en .NET 8 containers)
EXPOSE 8080
EXPOSE 8081

# Definimos el punto de entrada
ENTRYPOINT ["dotnet", "Gorgonix-Back.Api.dll"]