FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /src

COPY ["Client/BP_OnlineDOD.Client.csproj", "Client/"]
RUN dotnet restore "Client/BP_OnlineDOD.Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "BP_OnlineDOD.Client.csproj" -c Release -o /build

FROM build-env AS publish
RUN dotnet publish "BP_OnlineDOD.Client.csproj" -c Release -o /publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
COPY Client/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
EXPOSE 443