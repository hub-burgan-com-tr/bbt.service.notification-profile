FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5679 --disabled-password --gecos "" notificationuser && chown -R notificationuser:notificationuser /app
USER notificationuser

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build

WORKDIR /src
COPY ["bbt.service.notification-profile.csproj", "./"]
RUN dotnet restore "bbt.service.notification-profile.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "bbt.service.notification-profile.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "bbt.service.notification-profile.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:5000
ENTRYPOINT ["dotnet", "bbt.service.notification-profile.dll"]
