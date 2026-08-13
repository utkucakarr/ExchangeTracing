# Progress

Yapılan adımların günlüğü. Detay/plan için `docs/roadmap.md`, kalıcı kayıt için `git log`.

## Local setup (yeni klon)

Secret'lar repoya commit'lenmez. Klonladıktan sonra:

```bash
cp .env.example .env                                            # docker-compose creds
cp src/API/appsettings.Development.json.example src/API/appsettings.Development.json  # API connection string
# ikisini de gerçek değerlerle düzenle
docker compose up -d
dotnet run --project src/API
```

CI/prod ortamında connection string `ConnectionStrings__Postgres` environment variable'ı ile verilir. EF Core design-time komutları (`dotnet ef ...`) da bu env var'ı bekler (yoksa factory net bir hata verir):

```bash
export ConnectionStrings__Postgres="Host=localhost;Port=5432;Database=exchangetracing;Username=exchangetracing;Password=..."
```

## ✅ Yapıldı

### Phase 1 — Foundation
- [x] **Backend iskeleti** — Modüler monolit + Clean Architecture çözümü.
  - `ExchangeTracing.sln` + 18 proje: `API`, `BuildingBlocks`, 4 modül (`Users`, `Assets`, `Transactions`, `Portfolio`) × 4 katman (`Domain/Application/Infrastructure/Presentation`).
  - Bağımlılık yönü proje referanslarıyla derleyici seviyesinde zorlanıyor (`Presentation → Application → Domain`, `Infrastructure → Application/Domain`; modüller birbirini referanslamıyor; `API` tek composition root).
  - `Directory.Build.props` ile ortak ayarlar (`net10.0`, nullable, implicit usings, warnings-as-errors).
  - Her modülde `AddXyzModule()` DI giriş noktası (boş gövde).
  - `API/Program.cs`: modülleri kaydeder + `/health` endpoint.
  - Doğrulandı: `dotnet build` (0 uyarı/hata), `curl /health` → `{"status":"ok"}`, `dotnet test` (exit 0).

- [x] **PostgreSQL + Docker Compose + EF Core** — veri katmanı altyapısı.
  - `docker-compose.yml`: PostgreSQL 17-alpine, named volume, `pg_isready` healthcheck, port 5432. Creds gitignored `.env`'den (`.env.example` şablonu commit'li).
  - Connection string gitignored `appsettings.Development.json`'da (`.example` şablonu commit'li); CI/prod'da `ConnectionStrings__Postgres` env var. Commit'lenen `appsettings.json`'da secret yok.
  - Veri sahibi modüller (Users, Assets, Transactions) Infrastructure'da EF Core + Npgsql; her biri kendi PostgreSQL şemasında (`users`/`assets`/`transactions`) `DbContext` + design-time factory. Şema-başına `__EFMigrationsHistory`.
  - **Portfolio'da DbContext yok** (türetilmiş/okuma-odaklı, tablo sahiplenmiyor).
  - Her modül kendi DB health check'ini kaydeder; `/health` = `MapHealthChecks`. Henüz entity/tablo/migration yok.
  - Doğrulandı: `docker compose up` (healthy), `dotnet build` (0 uyarı/hata), `curl /health` → `Healthy` (loglar üç DbContext için `SELECT 1` → gerçek DB bağlantısı), `dotnet test` (exit 0).

## ➡️ Sıradaki adım

- [ ] **Mimari testler (NetArchTest)** — sınır/bağımlılık kurallarını CI'da otomatik doğrula.

## ⏭️ Sonraki adımlar (planlanan sıra)

- [ ] İlk dikey dilim: **Users** modülü (entity → create/get use-case → controller → uçtan uca).
- [ ] Assets modülü
- [ ] Transactions modülü (buy/sell + validation + history)
- [ ] Portfolio hesaplama (average cost, realized/unrealized P&L)
- [ ] Market prices (mock provider)
- [ ] Frontend (React) iskeleti
