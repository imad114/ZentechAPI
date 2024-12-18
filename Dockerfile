# �tape 1 : Image de base pour la construction
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# D�finir le r�pertoire de travail
WORKDIR /src

# Copier le fichier .csproj et restaurer les d�pendances
COPY ["ZentechAPI.csproj", "./"]
RUN dotnet restore "./ZentechAPI.csproj"

ENV ASPNETCORE_ENVIRONMENT=Production

# Copier tout le code source dans le conteneur
COPY . .

# Publier l'application dans le r�pertoire de sortie
RUN dotnet publish "./ZentechAPI.csproj" -c Release -o /app/publish

# �tape 2 : Image d'ex�cution (plus l�g�re, pour ex�cuter l'application)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# D�finir le r�pertoire de travail
WORKDIR /app

# Copier les fichiers publi�s depuis l'�tape de construction
COPY --from=build /app/publish .

# Exposer le port 5033 pour l'application
EXPOSE 5033

# D�finir l'ex�cution de l'application
ENTRYPOINT ["dotnet", "ZentechAPI.dll"]
