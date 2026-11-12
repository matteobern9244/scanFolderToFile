# Backlog

Open work for the modern macOS track (`src/`, `tests/`, `ScanFolderToFile.Modern.sln`) and, when
explicitly stated, the legacy WinForms track (`scanFolderToFile/`). Closed entries move to
`BACKLOG-HISTORY.md`. Managed with the `backlog` skill; ids are `BKLG-NNN`.

- **Verification**: `dotnet build` / `dotnet test` / `dotnet format --verify-no-changes` on
  `ScanFolderToFile.Modern.sln`, then `./start_scanfolder.command` when the change is visible in the app
  (see `AGENTS.md` and `CLAUDE.md`). Work that cannot be verified here goes in the pending ledger below.
- **Shipping**: the `AGENTS.md` pre-commit workflow (`README.md`, `CHANGELOG.md`, `dotnet format`, commit, push);
  commit and push only on request.
- **GitHub mirror**: off.

## Pending verification
Committed but NOT yet verified. Remove a line the moment its check passes.

Nothing pending.

## Open

## BKLG-006 — Upgrade toolchain and packages to the latest available versions
- **Status**: blocked — on [[BKLG-001]] (green baseline on the current pins) and on the scope decision (latest within current majors, or majors too)
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: yes (choose the scope: .NET 10 LTS patch vs .NET 11, Avalonia 11.3 vs 12; FluentAssertions 8 licence for commercial use)
- **Doc**: [plan.md](features/latest-versions-upgrade/plan.md)
- **Summary**: The repo pins SDK 10.0.103 and packages from late 2025; move to the latest versions, one bump per slice, each with regenerated `packages.lock.json` and a green build/test.

## BKLG-005 — "Reorder by extension" renames files that are already sorted
- **Status**: open — waiting for a decision: legacy parity (leave the file in place) or current behaviour
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: yes (decide the expected behaviour)
- **Doc**: —
- **Summary**: `FileOperationsService.ReorderByExtensionAsync` calls `EnsureUniquePath` before `PathsMatch`, so `src/txt/a.txt` becomes `src/txt/a(1).txt` on every run and the `SkippedPaths` branch is dead; legacy `Utils.ReorderFilesInFolderByType` leaves it in place. The test `CoveragePassTests.FileOperationsService_ReorderByExtensionAsync_WhenFileAlreadyInTargetFolder_RenamesIt` asserts the current behaviour. Fix (if parity): compare the un-suffixed target with the source first, skip on match, flip the test.

## BKLG-004 — ZIP creation when the output folder is inside the source folder
- **Status**: open — not reproduced yet; needs a failing test first
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `ZipArchiveService.CreateZipAsync` writes `<output>/ZipFiles/<zip>` while `ZipFile.CreateFromDirectory` walks the source; with the default output `~/Documents/CONTENT` and a source of `~/Documents`, the archive likely tries to include itself (IOException or a corrupt entry). Fix: write to a temp file outside the source and move it, or reject/skip the output folder; add a Core test.

## BKLG-003 — macOS-safe file enumeration (inaccessible folders, `.DS_Store`)
- **Status**: open
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `FileCollector` and `FileOperationsService.EnumerateSourceFiles` use `Directory.EnumerateFiles(..., SearchOption.AllDirectories)`, so one folder protected by macOS privacy (TCC) aborts the whole scan, and `.DS_Store` is collected/moved while only `desktop.ini` is excluded. Fix: shared `EnumerationOptions { RecurseSubdirectories = true, IgnoreInaccessible = true }`, exclude `.DS_Store` via `AppStrings.Filters`, tests in `Core.Tests`.

## BKLG-002 — Verify the native AppKit bridge actually runs
- **Status**: blocked — on [[BKLG-001]]: needs a working build and a launch of the app
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `AppKitWorkflowServices.cs` uses `AppKit`/`Foundation` from a bare `Microsoft.macOS.dll` in a plain `net10.0` Avalonia app, with no native Xamarin runtime initialisation; every call is wrapped in a bare `catch` that silently falls back (and also swallows cancellation). Likely the native editor/print never run. Check in the launched app; then either move to a supported setup or drop the bridge, and narrow the `catch` with logging.

## BKLG-001 — Reproducible toolchain: .NET SDK, macOS pack and modern CI
- **Status**: open — audit done 2026-11-12: no `dotnet` on this Mac, build/test/format not run
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: yes (approve the SDK download; push the branch so CI can run)
- **Doc**: [plan.md](bugs/toolchain-macos-reference/plan.md)
- **Summary**: The App and App.Tests reference `Microsoft.macOS.dll` through a `HintPath` into the git-ignored `./.dotnet/packs/...26.2.10197`, which no fresh clone or CI runner has, and `modern-ci.yml` runs only on `main` (never on this branch). Install the SDK locally, make the reference resolvable everywhere, run the modern CI on the porting branch.
