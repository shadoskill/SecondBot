#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["NuGet.Config", ""]
COPY ["CLI/BetterSecondBot.csproj", "CLI/"]
COPY ["Core/BSB.csproj", "Core/"]
COPY ["BSBshared/BSBshared.csproj", "BSBshared/"]
COPY ["libremetaverse-core/Libremetaverse/LibreMetaverse-core.csproj", "libremetaverse-core/Libremetaverse/"]
COPY ["libremetaverse-core/Libremetaverse.structureddata/LibreMetaverse-core.StructuredData.csproj", "libremetaverse-core/Libremetaverse.structureddata/"]
COPY ["libremetaverse-core/Libremetaverse.types/LibreMetaverse-core.Types.csproj", "libremetaverse-core/Libremetaverse.types/"]
RUN dotnet restore "CLI/BetterSecondBot.csproj"
COPY . .
WORKDIR "/src/CLI"
RUN dotnet build "BetterSecondBot.csproj" -c DockerBuild -o /app/build

FROM build AS publish
RUN dotnet publish "BetterSecondBot.csproj" -c DockerBuild -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# --- Update your settings ---

ENV Basic_BotUserName=''
ENV Basic_BotPassword=''
ENV Basic_HomeRegions='http://maps.secondlife.com/secondlife/Viserion/50/140/23'
ENV Basic_LoginLocation='home'

ENV Security_MasterUsername='Madpeter Zond'
ENV Security_SubMasters='X X,Y Y,Z Z'
ENV Security_SignedCommandkey=''
ENV Security_WebUIKey=''

ENV Setting_AllowRLV='false'
ENV Setting_AllowFunds='false'
ENV Setting_LogCommands='false'
ENV Setting_RelayImToAvatarUUID=''
ENV Setting_DefaultSit_UUID=''
ENV Setting_loginURI='secondlife'

ENV DiscordRelay_URL=''
ENV DiscordRelay_GroupUUID=''
ENV DiscordFull_Enable='false'
ENV DiscordFull_Token=''
ENV DiscordFull_ServerID=''
ENV DiscordFull_Keep_GroupChat='false'

ENV Http_Enable='false'
ENV Http_Host='docker'

ENV Name2Key_Enable='false'
ENV Name2Key_Url='http://localhost:1234'
ENV Name2Key_Key='magic'

ENV DiscordTTS_Enable='false'
ENV DiscordTTS_server_id=''
ENV DiscordTTS_channel_name=''
ENV DiscordTTS_avatar_uuid=''
ENV DiscordTTS_Nickname=''

ENV Log2File_Enable='false'
ENV Log2File_Level=1

EXPOSE 80
ENV ASPNETCORE_URLS http://+:80

# --- End of settings ---

ENTRYPOINT ["/app/BetterSecondBot"]
