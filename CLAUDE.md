@AGENTS.md

# CLAUDE.md

Claude Code notes for this repository. `AGENTS.md` (imported above) is the source of truth for
goals, scope, editing rules, testing and the pre-commit workflow; this file only adds what is
specific to Claude Code.

## Tooling Differences

- `AGENTS.md` mentions `apply_patch`: in Claude Code use the `Edit` / `Write` tools instead.
- Search with `rg` / `rg --files` as stated in `AGENTS.md`.

## .NET SDK

- `global.json` pins SDK `10.0.103` (`rollForward: latestPatch`).
- `start_scanfolder.command` looks for `dotnet` in this order: `~/.dotnet/dotnet`,
  `./.dotnet/dotnet` (git-ignored local SDK), then `PATH`.
- `ScanFolderToFile.App` and `ScanFolderToFile.App.Tests` set `UseMacOSBindings=true`; `Directory.Build.props`
  then downloads the `Microsoft.macOS.Runtime.osx.net10.0_*` NuGet package at restore and references its
  `Microsoft.macOS.dll` (no `macos` workload needed). Change its version only there.
- `ScanFolderToFile.App.Tests` runs on xunit v3; `ScanFolderToFile.Core.Tests` is still on xunit 2.
- If `dotnet` is missing, say so and stop instead of guessing a workaround.

## Validation

Run from the repository root, smallest scope first. When `dotnet` is not on `PATH`, use the local SDK:
`export DOTNET_ROOT="$PWD/.dotnet"` and call `./.dotnet/dotnet` in place of `dotnet`.

- `dotnet build ScanFolderToFile.Modern.sln`
- `dotnet test ScanFolderToFile.Modern.sln`
- `dotnet format --verify-no-changes ScanFolderToFile.Modern.sln` (check), `dotnet format ScanFolderToFile.Modern.sln` (fix)
- `./start_scanfolder.command` for the end-to-end launch on macOS (`--publish` builds `dist/ScanFolderToFile.app`)

CI for the modern track is `.github/workflows/modern-ci.yml` (macOS runner, locked restore,
format check, Release build, tests with coverage, self-contained publish and bundle).
`.github/workflows/ci.yml` belongs to the legacy Windows track.

## Backlog

Tracked work lives in `docs/implementations/BACKLOG.md` (closed work in `BACKLOG-HISTORY.md`), managed with the
`backlog` skill. Read the register before starting work; anything that needs a plan, spec or report gets an entry
before its doc folder is created.

## Commits

Commit and push only when the user asks. Before a commit, follow the `AGENTS.md` pre-commit
workflow (`README.md`, `CHANGELOG.md`, `dotnet format`, then commit, then push).
