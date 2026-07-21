# openprocurement-agent
> OpenProcurementAgent is a lightweight service that provides access to the open tenders database and enables advanced filtering and selection of procurement data.

## Docker Deployment

The application includes Docker support for easy deployment and scalability.

### Prerequisites

- [Docker](https://www.docker.com/get-started) (version 20.10+)

### Quick Start

**Run with Docker:**

Download and launch the `openprocurement-agent:latest` image with the following environment variables and ports:

```bash
docker volume create openprocurement-agent_data
docker run --detach --restart=always --name openprocurement-agent \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e Kestrel__Endpoints__Http__Url=http://+:5050 \
  -e ConnectionStrings__SettingsConnection="Data Source=/app/data/Settings.db" \
  -e ConnectionStrings__ProcuringEntityConnection="Data Source=/app/data/ProcuringEntity.db" \
  -e ConnectionStrings__TenderHistoryConnection="Data Source=/app/data/TenderHistory.db" \
  -e DataProtection__KeyPath=/app/data/keys \
  -v openprocurement-agent_data:/app/data \
  -p 5050:5050 \
  ghcr.io/freehand-dev/openprocurement-agent:latest
```


```bash
docker build -f deploy/docker/Dockerfile -t openprocurement-agent
```