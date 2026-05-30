# AGENTS.md

This file provides context for AI agents (e.g. OpenAI Codex) working on the **mhwildsdb** project built with .NET 10 and PostgreSQL.

---

## Project Overview

`mhwildsdb` is a database With an RESTful Web API layer that serves Monster Hunter Wilds game data (skills, armour, armour sets, etc.), primarily aimed at supporting set builders. It is a clean rewrite of an older project, following better architectural standards.

The codebase uses a vertical slice-ish structure within a layered architecture: controllers → services → EF Core → PostgreSQL.

---

## Tech Stack

- **Runtime:** .NET 10, ASP.NET Core Web API (Controllers)
- **ORM:** Entity Framework Core 10 with Npgsql (PostgreSQL 17)
- **Validation:** FluentValidation 12, applied via a generic `ValidateFilter<T>` action filter
- **Error Handling:** Global exception handler (`GlobalExceptionHandler`) using `IExceptionHandler` + Problem Details
- **API Docs:** Scalar (OpenAPI), accessible at `/scalar/v1` in Development
- **Logging:** Serilog with Console + Seq sinks
- **Testing:** xUnit v3, FluentAssertions, NSubstitute, EF Core InMemory provider
- **Infrastructure:** Docker Compose (PostgreSQL + Seq containers)

---

## Repository Structure

```
mhwildsdb/
├── Controllers/          # HTTP entry points, thin — delegate to services
├── DTOs/                 # Request/response records (no domain logic)
├── Entities/             # EF Core domain entities — DO NOT MODIFY without approval
│   ├── Armours/
│   ├── Skills/
│   ├── Charms/
│   ├── EntityBase.cs     # Base class: Id (Guid), Created, LastModified
│   └── Resistances.cs    # Owned entity (value object)
├── Exceptions/           # Custom exception types extending AppException
│   └── Handlers/         # GlobalExceptionHandler
├── Filters/              # ValidateFilter<T> — generic FluentValidation action filter
├── Helpers/
│   ├── Extensions/
│   │   └── Mapping/      # Extension methods for entity ↔ DTO mapping
│   └── ValidationHelpers.cs
├── Migrations/           # EF Core auto-generated migrations — DO NOT hand-edit
├── Persistance/
│   ├── Configuration/    # IEntityTypeConfiguration<T> per entity
│   └── MhwildsDbContext.cs
├── Services/             # Business logic; one interface + implementation per aggregate
├── Validators/           # FluentValidation validators, mirroring DTO structure
│   ├── ArmourValidators/
│   └── SkillValidators/
└── Program.cs            # App entry point, DI registration, middleware pipeline

mhwildsdb.Tests/
├── Services/             # Integration-style service tests using EF InMemory
├── Helpers/              # Unit tests for helper methods used in validators 
└── └── ValidationHelpersTests.cs
```

---

## Commands

All commands should be run from the **solution root** unless otherwise noted.

### Build
```bash
dotnet build
```

### Run tests
```bash
dotnet test
```
> Always run tests after any implementation change. All tests must pass before considering a task complete.

### Add AND Apply a migration (run from inside mhwildsdb/ NOT from solution root)
```bash
cd mhwildsdb
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

### Start infrastructure (PostgreSQL + Seq)
```bash
docker compose up -d
```
> Requires a `.env` file at the solution root. See `.env.example` for required variables.

### Run the API locally
```bash
dotnet run --project mhwildsdb
```
> The connection string is stored in user secrets (`dotnet user-secrets`), not in `appsettings.json`.

---

## Architecture Conventions

### Entities
- Inherit from `EntityBase` (provides `Id`, `Created`, `LastModified`)
- All properties use `private set` - mutation is only through named methods (`Create`, `Update`)
- Use static factory methods (`Entity.Create(...)`) instead of public constructors
- EF Core requires a `private` parameterless constructor

### DTOs
- Use `sealed record` for all DTOs
- Suffixes: `CreateXDto`, `UpdateXDto`, `XDto` (response), `XSummaryDto` (lightweight response)
- DTOs live under `DTOs/<Aggregate>/` and are flat — no domain logic

### Mapping
- Done via static extension methods in `Helpers/Extensions/Mapping/`
- Pattern: `entity.ToDto()`, `dto.ToDomain()`
- No mapping libraries: AutoMapper, Mapster

### Services
- One `IXService` interface + `XService` implementation per aggregate
- Constructor injection via primary constructor syntax: `public XService(Dep _dep)`
- Throw typed exceptions (`NotFoundException`, `ConflictException`, `BadRequestException`) — never return nulls for missing resources
- Use `AsNoTracking()` for all read queries
- Eager-load navigation properties explicitly with `.Include()` / `.ThenInclude()`

### Validation
- All validators use FluentValidation and live in `Validators/<Aggregate>Validators/`
- Applied via `[ServiceFilter(typeof(ValidateFilter<TDto>))]` on controller actions
- `ValidationHelpers` contains shared predicate methods (e.g. `BeValidName`, `BeUnique`)
- Use `Cascade(CascadeMode.Stop)` to stop on first failure per rule chain

### Error Handling
- All custom exceptions extend `AppException(message, HttpStatusCode)`
- `GlobalExceptionHandler` maps exception types to HTTP status codes and returns RFC 9457 Problem Details
- Never return raw exception messages in production — only `AppException` messages are exposed

### EF Core
- Entity configuration lives in `Persistance/Configuration/` using `IEntityTypeConfiguration<T>`
- Default schema is `app`
- Many-to-many join tables are named explicitly (e.g. `ArmourSkillRanks`)
- `QuerySplittingBehavior.SplitQuery` is set globally for multi-include queries

---

## Workflow Rules

1. **Always create a feature branch** before making any changes. Never commit directly to `main`.
2. **Run `dotnet test` after every implementation.** All tests must pass.
3. **Never modify files in `Entities/`** without explicit instruction — entity changes require migrations.
4. **Never hand-edit files in `Migrations/`** — always use `dotnet ef migrations add`.
5. **Keep commits atomic** — one logical change per commit with a clear message.
6. **Write tests** for any new service method or validation helper methods added.

---

## Adding a New Resource (Typical Pattern)

When adding a new aggregate (e.g. `Decoration`, `Charm`):

1. Add entity to `Entities/<Aggregate>/` extending `EntityBase`
2. Add EF config to `Persistance/Configuration/<Aggregate>Configuration.cs`
3. Register `DbSet<T>` in `MhwildsDbContext`
4. Run `dotnet ef migrations add Added<Aggregate>`
5. Add DTOs to `DTOs/<Aggregate>/` (`CreateXDto`, `UpdateXDto`, `XDto`)
6. Add FluentValidation validators to `Validators/<Aggregate>Validators/`
7. Add mapping extensions to `Helpers/Extensions/Mapping/`
8. Add `IXService` interface + `XService` implementation to `Services/`
9. Register service in `Program.cs`
10. Add `XController` to `Controllers/`
11. Write service test in `mhwildsdb.Tests/Services/`
12. Run `dotnet test` — all tests must pass

---

## Do Not

- Do not add business logic to controllers — they should only delegate to services and return results
- Do not use AutoMapper or any other mapping library — use the existing extension method pattern
- Do not use public setters on entity properties
- Do not bypass `ValidateFilter<T>` by manually calling validators in services
- Do not expose internal exception details in production responses
- Do not add new NuGet packages without flagging it — keep dependencies minimal
