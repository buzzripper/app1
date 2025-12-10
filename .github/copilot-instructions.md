# Copilot Instructions

## Terminology
- "module": Refers to **Auth**, **App**, or **Portal** modules. Each module contains `Shared`, `Api`, and `Server` projects.

## Development Standards
- Follow `.editorconfig` for formatting and naming.
- API controllers and endpoint definitions go in `Api` projects.
- Hosting/runtime configuration goes in `Server` projects.
- Cross-cutting types and DTOs go in `Shared` projects.
- `System.Apis` contains common Aspire service defaults (OpenTelemetry, health checks, service discovery).
- `System.Shared` contains shared utilities across all modules.

## Code Style
- Target: .NET 10, C# 14
- Use file-scoped namespaces.
- Use primary constructors where appropriate.
- Prefer collection expressions (`[]`) over `new List<T>()`.
