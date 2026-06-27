FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish "./src/openprocurement-agent/openprocurement-agent.csproj"

FROM base AS final
ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN mkdir -p /opt/openprocurement-agent/bin
RUN mkdir -p /opt/openprocurement-agent/etc/openprocurement-agent
WORKDIR /opt/openprocurement-agent/bin
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "openprocurement-agent.dll"]