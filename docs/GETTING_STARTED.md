# Getting Started

Kurz: diese Schritte starten die benötigten Infrastruktur-Services, die API und die Angular-Entwicklungsinstanz.

## Voraussetzungen
- Docker & Docker Compose
- .NET SDK (8/9+)
- Node.js & npm (für das Frontend)

## Environment
Lege sensible Werte in einer `.env` im Projekt-Root ab (beispielsweise `SA_PASSWORD`, `POSTGRES_PASSWORD`, `KEYCLOAK_ADMIN`, `KEYCLOAK_ADMIN_PASSWORD`).

## 1) Infrastruktur starten (SQL Server, Postgres, Keycloak)
Im Projekt-Root ausführen:

```bash
docker compose up -d
```

## 2) Datenbank-Migrationen anwenden
Die Migrations befinden sich im `EmployeeHub.Infrastructure`-Projekt. Beispiel:

```bash
dotnet ef database update --project backend/EmployeeHub.Infrastructure --startup-project backend/EmployeeHub.Api
```

Wenn `dotnet-ef` fehlt, installieren mit:

```bash
dotnet tool install --global dotnet-ef
```

## 3) Backend starten
Im `backend/EmployeeHub.Api`-Ordner:

```bash
cd backend/EmployeeHub.Api
dotnet run
```

Die API lauscht standardmäßig entsprechend der `launchSettings` bzw. Umgebungsvariablen. In Development ist Swagger verfügbar.

## 4) Frontend starten
Im `frontend`-Ordner:

```bash
cd frontend
npm install
npm start
```

Standardmäßig läuft das Angular-Devserver auf `http://localhost:4200`.

## 5) Tests ausführen
Im Solution-Root:

```bash
dotnet test backend/EmployeeHub.Tests
```

---

Bei Bedarf kann ich die `README.md` weiter ausbauen (z.B. Beispiel-`.env`, Keycloak-Import-Dump, oder CI-Schritte). Sag mir kurz, welche Ergänzungen du möchtest.
