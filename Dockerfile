FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --runtime linux-x64 --output /app/publish -c Release "./openprocurement-agent/openprocurement-agent.csproj"

FROM base AS final
ENV TZ=Europe/Kiev
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
RUN mkdir -p /opt/openprocurement-agent/bin
RUN mkdir -p /opt/openprocurement-agent/etc/openprocurement-agent
WORKDIR /opt/openprocurement-agent/bin
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "openprocurement-agent.dll"]