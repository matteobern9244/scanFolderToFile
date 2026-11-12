# BKLG-006 — Upgrade toolchain and packages to the latest available versions

## Versions (checked 2026-11-12 on the .NET release index and nuget.org)

| Component | Pinned now | Latest in current major | Latest overall |
|---|---|---|---|
| .NET SDK | 10.0.103 (`latestPatch` → 10.0.1xx) | 10.0.404 (LTS, support to 2028-11-14) | 11.0.100 (STS, GA 2026-11-10, support to 2028-11-14) |
| Microsoft.macOS runtime pack (net10.0) | 26.2.10197 | 26.2.10233 | 26.5.10318 (net10.0_26.5); net11 pack not checked |
| Avalonia (+ Desktop, Fluent, Headless) | 11.3.9 | 11.3.23 | 12.1.6 |
| QuestPDF | 2024.12.1 | — (calendar versions) | 2026.9.2 |
| xunit | 2.9.3 | 2.9.3 | xunit.v3 4.0.3 (Avalonia.Headless.XUnit 12.x requires xunit.v3) |
| xunit.runner.visualstudio | 3.1.4 | 3.1.x | 4.0.1 |
| Microsoft.NET.Test.Sdk | 17.14.1 | 17.x | 18.12.0 |
| FluentAssertions | 8.2.0 | 8.11.0 | 8.11.0 (v8 needs a paid licence for commercial use) |
| coverlet.collector | 6.0.4 | 6.x | 10.1.0 |

Re-check the numbers when a slice starts: they move.

## Rules

- Start only after [[BKLG-001]] P2 (green baseline on the current pins), so upgrade breakage is distinguishable
  from pre-existing failures.
- One bump per slice; each slice regenerates every `packages.lock.json` (restore runs `--locked-mode` in the
  launcher and in CI) and ends green on build, test, format and coverage.
- A TFM change also touches `start_scanfolder.command` (`FRAMEWORK`), both modern workflows (`-f net10.0`,
  publish paths), the macOS pack `HintPath`/reference and `CLAUDE.md`.

## Plan

| | state |
|---|---|
| **P1** decide the scope: current majors only, or .NET 11 + Avalonia 12 + xunit v3 | 📋 not started — Manual decision |
| **P2** SDK + `global.json` (+ TFM if .NET 11) | 📋 not started |
| **P3** test stack: Test.Sdk, runner, coverlet, FluentAssertions (xunit v3 if Avalonia 12) | 📋 not started |
| **P4** Avalonia | 📋 not started |
| **P5** QuestPDF (check the exporter output and the Community licence setting) | 📋 not started |
| **P6** macOS runtime pack | 📋 not started |
| **P7** `README.md`, `CHANGELOG.md`, `CLAUDE.md` | 📋 not started |
