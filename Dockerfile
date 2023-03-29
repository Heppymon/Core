FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["MyBotCore/MyBotCore.csproj", "MyBotCore/"]
COPY ["MyBotDb/MyBotDb.csproj", "MyBotDb/"]
RUN dotnet restore "MyBotCore/MyBotCore.csproj"

COPY . .
WORKDIR "/src/MyBotCore"
RUN dotnet build "MyBotCore.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyBotCore.csproj" -c Release -o /app/publish
FROM base AS final
EXPOSE 80
EXPOSE 8443
WORKDIR /app
COPY --from=publish /app/publish .
# CMD ASPNETCORE_URLS=http://*:$PORT dotnet TGbot.Api.dll
ENTRYPOINT ["dotnet", "MyBotCore.dll"]

# docker tag tgbot cr.yandex/crp8262lrit0arum0q0d/tgbot:initial