FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY wait-for-it.sh .
RUN chmod +x wait-for-it.sh

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Server/BP_OnlineDOD.Server.csproj", "Server/"]
RUN dotnet restore "Server/BP_OnlineDOD.Server.csproj"
COPY . .
WORKDIR "/src/Server"

RUN dotnet build "BP_OnlineDOD.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BP_OnlineDOD.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BP_OnlineDOD.Server.dll"]