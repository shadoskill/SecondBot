#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BetterSecondbot/BetterSecondbot.csproj", "BetterSecondbot/"]
COPY ["Core/Core.csproj", "Core/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["LibreMetaverse/LibreMetaverse.csproj", "LibreMetaverse/"]
COPY ["LibreMetaverse.StructuredData/LibreMetaverse.StructuredData.csproj", "LibreMetaverse.StructuredData/"]
COPY ["LibreMetaverseTypes/LibreMetaverse.Types.csproj", "LibreMetaverseTypes/"]
RUN dotnet restore "BetterSecondbot/BetterSecondbot.csproj"
COPY . .
WORKDIR "/src/BetterSecondbot"
RUN dotnet build "BetterSecondbot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BetterSecondbot.csproj" -c Release -o /app/publish

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

ENTRYPOINT ["/app/BetterSecondbot"]