# ExchangeTracing

A portfolio management system that tracks buy/sell transactions and derives
portfolio performance (average cost, realized/unrealized P&L). Built as a
single-developer **Modular Monolith** with **Clean Architecture**.

> Early stage: the solution skeleton, PostgreSQL/EF Core infrastructure and
> architecture tests are in place; business features are next.
> Progress: [`PROGRESS.md`](PROGRESS.md).

## Tech stack

.NET 10 · ASP.NET Core Web API · Entity Framework Core · PostgreSQL ·
xUnit / NetArchTest · Docker. Frontend (planned): React + TypeScript.

## Architecture

- **Modular monolith** — one deployable; each business area (`Users`, `Assets`,
  `Transactions`, `Portfolio`) is an isolated module.
- **Clean Architecture per module** — `Domain` / `Application` /
  `Infrastructure` / `Presentation` as separate projects, so the dependency
  direction (`Presentation → Application → Domain`, `Infrastructure →
  Application/Domain`) is enforced at compile time. Modules never reference each
  other; `API` is the single composition root.
- **Transactions are the source of truth**; portfolio state is derived, not
  persisted.
- One PostgreSQL database, one schema per data-owning module.

More detail in [`docs/`](docs/).

## Project structure

```text
src/
├── API/                 # ASP.NET Core host + composition root (/health)
├── BuildingBlocks/      # shared technical primitives
└── Modules/{Users,Assets,Transactions,Portfolio}/
    └── {Domain,Application,Infrastructure,Presentation}/
tests/ArchitectureTests/ # NetArchTest boundary rules
docs/                    # architecture / domain / database / decisions
```

## Running the project

Requires .NET SDK 10.0.100+ and Docker.

```bash
# 1. create local secrets from templates (not committed)
cp .env.example .env
cp src/API/appsettings.Development.json.example src/API/appsettings.Development.json

# 2. start PostgreSQL
docker compose up -d

# 3. apply database migrations (needs the dotnet-ef tool: dotnet tool install --global dotnet-ef)
export ConnectionStrings__Postgres="Host=localhost;Port=5432;Database=exchangetracing;Username=exchangetracing;Password=exchangetracing"
dotnet ef database update \
  --project src/Modules/Users/Infrastructure --startup-project src/Modules/Users/Infrastructure

# 4. run the API
dotnet run --project src/API
```

Then:

- `http://localhost:5255/health` → `Healthy` (also verifies the DB connection)
- `http://localhost:5255/scalar/v1` → API reference UI (Scalar, dev only)
- `http://localhost:5255/openapi/v1.json` → OpenAPI document

In CI/production the connection string comes from the
`ConnectionStrings__Postgres` environment variable instead of the file.

## Tests

```bash
dotnet test
```

Runs the architecture tests that fail the build if a module/layer boundary is
violated. CI runs restore/build/test on every push and pull request.
