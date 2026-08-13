# Progress

Yapılan adımların günlüğü. Detay/plan için `docs/roadmap.md`, kalıcı kayıt için `git log`.

## ✅ Yapıldı

### Phase 1 — Foundation
- [x] **Backend iskeleti** — Modüler monolit + Clean Architecture çözümü.
  - `ExchangeTracing.sln` + 18 proje: `API`, `BuildingBlocks`, 4 modül (`Users`, `Assets`, `Transactions`, `Portfolio`) × 4 katman (`Domain/Application/Infrastructure/Presentation`).
  - Bağımlılık yönü proje referanslarıyla derleyici seviyesinde zorlanıyor (`Presentation → Application → Domain`, `Infrastructure → Application/Domain`; modüller birbirini referanslamıyor; `API` tek composition root).
  - `Directory.Build.props` ile ortak ayarlar (`net10.0`, nullable, implicit usings, warnings-as-errors).
  - Her modülde `AddXyzModule()` DI giriş noktası (boş gövde).
  - `API/Program.cs`: modülleri kaydeder + `/health` endpoint.
  - Doğrulandı: `dotnet build` (0 uyarı/hata), `curl /health` → `{"status":"ok"}`, `dotnet test` (exit 0).

## ➡️ Sıradaki adım

- [ ] **PostgreSQL + Docker Compose + EF Core** — `docker-compose.yml`, connection string, modül başına DbContext (tek DB).

## ⏭️ Sonraki adımlar (planlanan sıra)

- [ ] Mimari testler (NetArchTest) — sınır kurallarını CI'da doğrula.
- [ ] İlk dikey dilim: **Users** modülü (entity → create/get use-case → controller → uçtan uca).
- [ ] Assets modülü
- [ ] Transactions modülü (buy/sell + validation + history)
- [ ] Portfolio hesaplama (average cost, realized/unrealized P&L)
- [ ] Market prices (mock provider)
- [ ] Frontend (React) iskeleti
