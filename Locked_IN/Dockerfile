FROM node:20 AS build-frontend
WORKDIR /app-frontend

COPY locked_in_frontend/package*.json ./
RUN npm install

COPY locked_in_frontend/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-backend
WORKDIR /src

COPY ["Locked_IN_Backend/Locked_IN_Backend.csproj", "Locked_IN_Backend/"]
RUN dotnet restore "Locked_IN_Backend/Locked_IN_Backend.csproj"

COPY Locked_IN_Backend/ Locked_IN_Backend/
WORKDIR "/src/Locked_IN_Backend"
RUN dotnet build "Locked_IN_Backend.csproj" -c Release -o /app/build

FROM build-backend AS publish
RUN dotnet publish "Locked_IN_Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=publish /app/publish .

COPY --from=build-frontend /app-frontend/dist ./wwwroot

EXPOSE 8080

ENTRYPOINT ["dotnet", "Locked_IN_Backend.dll"]