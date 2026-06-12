# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["spms/spms.csproj", "spms/"]
RUN dotnet restore "spms/spms.csproj"
COPY . .
WORKDIR "/src/spms"
RUN dotnet publish "spms.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Explicitly ensure wwwroot is copied if it wasn't captured correctly
# COPY --from=build /src/spms/wwwroot ./wwwroot

ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000
ENTRYPOINT ["dotnet", "spms.dll"]
