# ExchangeTracing

A portfolio management system that tracks completed buy/sell transactions and
derives portfolio performance (average cost, realized and unrealized P&L) from
them. Built as a single-developer **Modular Monolith** with **Clean
Architecture**.

> Status: early foundation. The solution skeleton, PostgreSQL/EF Core
> infrastructure and architecture tests are in place. No business endpoints
> exist yet — the first feature slice (Users) is next. See
> [Project status](#project-status) and [`PROGRESS.md`](PROGRESS.md).

## Tech stack

- **Backend:** C# / .NET 10 (SDK `10.0.100`), ASP.NET Core Web API,
  Entity Framework Core, PostgreSQL
- **Testing:** xUnit, NetArchTest.Rules
- **Tooling:** Docker / Docker Compose
- **Frontend (planned):** React, TypeScript, Vite, TanStack Query, Tailwind CSS

## Architecture

- **Modular Monolith** — one deployable, but each business area is an isolated
  module with explicit boundaries.
- **Clean Architecture per module** — every module has `Domain`, `Application`,
  `Infrastructure` and `Presentation` layers as separate projects, so the
  dependency direction is enforced **at compile time**:

  ```text
  Presentation → Application → Domain
  Infrastructure → Application, Domain
  ```

- **Module isolation** — modules do not reference each other. The `API` project
  is the single composition root that wires the modules together.
- **Source of truth** — `Transactions` are the source of truth. Portfolio state
  (holdings, average cost, P&L) is **derived** from transactions + market
  prices, not persisted as an independent table.
- **Single database, schema per module** — one PostgreSQL database; each
  data-owning module (`Users`, `Assets`, `Transactions`) maps to its own schema.
  `Portfolio` owns no tables (read-only/derived).

Deeper docs live in [`docs/`](docs/): `architecture.md`, `domain.md`,
`database.md`, `decisions.md`, `frontend-architecture.md`, `roadmap.md`.

## Project structure

```text
ExchangeTracing.sln
Directory.Build.props        # shared build settings (net10.0, nullable, warnings-as-errors)
docker-compose.yml           # PostgreSQL 17
src/
├── API/                     # ASP.NET Core host + composition root (/health)
├── BuildingBlocks/          # shared technical primitives
└── Modules/
    ├── Users/               # each module: Domain / Application / Infrastructure / Presentation
    ├── Assets/
    ├── Transactions/
    └── Portfolio/           # no Infrastructure DbContext (derived, read-only)
tests/
└── ArchitectureTests/       # NetArchTest rules that enforce the boundaries above
docs/                        # architecture / domain / database / decisions
PROGRESS.md                  # done / next-step log
```

## Getting started

### Prerequisites

- [.NET SDK 10.0.100+](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for PostgreSQL)

### 1. Configure secrets (not committed)

Secrets are never committed. Copy the templates and fill in real values:

```bash
cp .env.example .env
cp src/API/appsettings.Development.json.example src/API/appsettings.Development.json
```

- `.env` — credentials for the PostgreSQL container (used by `docker-compose`).
- `src/API/appsettings.Development.json` — the API's `ConnectionStrings:Postgres`.

### 2. Start PostgreSQL

```bash
docker compose up -d
```

### 3. Run the API

```bash
dotnet run --project src/API
```

Verify it is up and connected to the database:

```bash
curl http://localhost:5255/health   # -> Healthy
```

> The port comes from `src/API/Properties/launchSettings.json` (default `5255`).
> `/health` also checks the database connection, so a `Healthy` response means
> the app reached PostgreSQL.

## Running tests

```bash
dotnet test
```

Currently this runs the **architecture tests**, which fail the build if a
boundary rule is broken (e.g. `Domain` depending on `Infrastructure`, or one
module depending on another).

## Configuration reference

| Setting | Local dev | CI / production |
| --- | --- | --- |
| PostgreSQL credentials | `.env` (gitignored) | environment |
| API connection string | `appsettings.Development.json` (gitignored) | `ConnectionStrings__Postgres` env var |

EF Core design-time commands (`dotnet ef ...`) also read
`ConnectionStrings__Postgres` from the environment.

## Continuous integration

GitHub Actions (`.github/workflows/ci.yml`) runs on every push and pull request:
`dotnet restore` → `dotnet build` → `dotnet test`.

## Project status

Implemented:

- [x] Modular monolith solution skeleton (18 projects, compile-time boundaries)
- [x] PostgreSQL + Docker Compose + EF Core infrastructure (per-module DbContext,
      schema isolation, DB health check) — no entities/migrations yet
- [x] Architecture tests (NetArchTest) enforcing module/layer boundaries

Next:

- [ ] Users module — first end-to-end vertical slice
- [ ] Assets, Transactions, Portfolio, Market prices
- [ ] React frontend

The living checklist is in [`PROGRESS.md`](PROGRESS.md); the phased plan is in
[`docs/roadmap.md`](docs/roadmap.md).
```

This README is updated as features land.
