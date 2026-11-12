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
- `ScanFolderToFile.App` and `ScanFolderToFile.App.Tests` reference `Microsoft.macOS.dll` through a
  `HintPath` inside `./.dotnet/packs/`: the App build needs that local SDK with the macOS pack.
  If `dotnet` is missing, say so and stop instead of guessing a workaround.

## Validation

Run from the repository root, smallest scope first:

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
