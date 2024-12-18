# Utiliser l'image de base pour la construction de l'application .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# D�finir le r�pertoire de travail dans l'image Docker
WORKDIR /app

# Copier les fichiers du projet vers le conteneur Docker
# Assure-toi que tu copies les fichiers .csproj et le reste du projet dans le conteneur
COPY ["ZentechAPI.csproj", "./"]

# Restaurer les d�pendances
RUN dotnet restore "ZentechAPI.csproj"

# Copier tout le code source
COPY . .

# Publier l'application
RUN dotnet publish "ZentechAPI.csproj" -c Release -o /app/publish

# Cr�er l'image d'ex�cution avec l'image .NET ASP.NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5033

# Copier les fichiers publi�s depuis l'�tape de construction
COPY --from=build /app/publish .

# D�marrer l'application
ENTRYPOINT ["dotnet", "ZentechAPI.dll"]
