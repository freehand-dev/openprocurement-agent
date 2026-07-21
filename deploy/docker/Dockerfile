FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet tool restore
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN cd src/openprocurement-agent && /root/.dotnet/tools/libman restore
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish "./src/openprocurement-agent/openprocurement-agent.csproj"

FROM base AS final
ENV TZ=Europe/Kyiv
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN mkdir -p /opt/openprocurement-agent/bin
RUN mkdir -p /opt/openprocurement-agent/etc/openprocurement-agent
WORKDIR /opt/openprocurement-agent/bin
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "openprocurement-agent.dll"]