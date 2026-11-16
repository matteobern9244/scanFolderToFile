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

## BKLG-010 — Dark theme by default, with a switch to the light theme
- **Status**: open
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: [spec.md](features/dark-theme/spec.md)
- **Summary**: The app must open dark and let the user switch to light. Today every window forces `RequestedThemeVariant="Light"` and `App.axaml` holds fixed light brushes: move the palette into theme dictionaries, default Dark, add a menu switch and persist the choice.

## BKLG-009 — macOS app menu shows "Avalonia Application" and "About Avalonia"
- **Status**: open — seen 2026-11-12 in the published bundle
- **Priority**: low
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: The application menu reads "Avalonia Application" / "About Avalonia" / "Hide Avalonia Application" (English) although `Info.plist` names the bundle "Scan Folder To File": `App.axaml` sets no application `Name`. Set it from `AppStrings` and check the About item.

## BKLG-008 — Clipped labels and squashed text in the macOS windows
- **Status**: in-progress — fixed in the working tree 2026-11-12 (button text wraps, main actions on a 3×3 grid, auto label/browse columns, `FilterWindow` sized to content); verified by screenshots of main, Filtri, Storico, Copia/Sposta, editor; Duplicati and Anteprima di stampa not opened (need a scan); not committed
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `MainWindow.axaml` packs the action buttons into 4 and 5 equal columns of a ~540 px card, so labels read "Storico Fi", "Altre oper", "Apri cartella fi"; the 132 px browse buttons cut "Seleziona cartella..."; the 124 px label column cuts "Cartella da scansionare" (same in `UtilitiesWindow`: "Cartella di destinazi"). `FilterWindow.axaml` is fixed at 560×420 and its status line is cut in half. Fix with content-sized layouts (wrapping rows, auto label columns, `SizeToContent`) and check every window by screenshot.

## BKLG-007 — App restore fails: vulnerable `Tmds.DBus.Protocol` 0.21.2 (NU1903)
- **Status**: in-progress — fixed in the working tree 2026-11-12 by Avalonia 12.1.6 (pulls `Tmds.DBus.Protocol` 0.94.1): `./start_scanfolder.command --publish` green (locked restore, Release build, 95/95 tests, bundle); not committed, CI not run
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: Avalonia 11.3.9 pulls `Tmds.DBus.Protocol` 0.21.2 (advisory GHSA-xrw6-gwf8-vvr9, high); with `TreatWarningsAsErrors` the restore of `ScanFolderToFile.App` and `App.Tests` fails, so the App cannot build here or in CI. Avalonia 11.3.23 depends on 0.21.3, Avalonia 12.1.6 on 0.94.1: bump Avalonia (patch now, or straight to 12 in [[BKLG-006]]), regenerate the `packages.lock.json` files, confirm the warning is gone.

## BKLG-006 — Upgrade toolchain and packages to the latest available versions
- **Status**: in-progress — Avalonia 12.1.6, App.Tests on xunit v3 and macOS pack 26.5 done locally (2026-11-12); next: Core.Tests to xunit v3, test packages, QuestPDF, then .NET 11
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: yes (confirm the FluentAssertions 8 licence fits the project's use, or pick a replacement)
- **Doc**: [plan.md](features/latest-versions-upgrade/plan.md)
- **Summary**: The repo pins SDK 10.0.103 and packages from late 2025; move to the latest versions, one bump per slice, each with regenerated `packages.lock.json` and a green build/test.

## BKLG-005 — "Reorder by extension" renames files that are already sorted
- **Status**: open — decided 2026-11-12: legacy parity, an already-sorted file stays in place and is reported as skipped
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `FileOperationsService.ReorderByExtensionAsync` calls `EnsureUniquePath` before `PathsMatch`, so `src/txt/a.txt` becomes `src/txt/a(1).txt` on every run and the `SkippedPaths` branch is dead; legacy `Utils.ReorderFilesInFolderByType` leaves it in place. The test `CoveragePassTests.FileOperationsService_ReorderByExtensionAsync_WhenFileAlreadyInTargetFolder_RenamesIt` asserts the current behaviour. Fix: compare the un-suffixed target with the source first, skip on match, flip the test.

## BKLG-004 — ZIP creation when the output folder is inside the source folder
- **Status**: open — not reproduced yet; needs a failing test first
- **Priority**: medium
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `ZipArchiveService.CreateZipAsync` writes `<output>/ZipFiles/<zip>` while `ZipFile.CreateFromDirectory` walks the source; with the default output `~/Documents/CONTENT` and a source of `~/Documents`, the archive likely tries to include itself (IOException or a corrupt entry). Fix: write to a temp file outside the source and move it, or reject/skip the output folder; add a Core test.

## BKLG-003 — macOS-safe file enumeration (inaccessible folders, `.DS_Store`)
- **Status**: in-progress — fixed in the working tree 2026-11-16 (shared EnumerationOptions with IgnoreInaccessible, `.DS_Store` excluded, Core tests green); committed, not tried on a real TCC-protected folder in the app
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `FileCollector` and `FileOperationsService.EnumerateSourceFiles` use `Directory.EnumerateFiles(..., SearchOption.AllDirectories)`, so one folder protected by macOS privacy (TCC) aborts the whole scan, and `.DS_Store` is collected/moved while only `desktop.ini` is excluded. Fix: shared `EnumerationOptions { RecurseSubdirectories = true, IgnoreInaccessible = true }`, exclude `.DS_Store` via `AppStrings.Filters`, tests in `Core.Tests`.

## BKLG-002 — Verify the native AppKit bridge actually runs
- **Status**: open — confirmed 2026-11-12 in the published bundle: menu Altro › Editor interno opens the Avalonia `EditorWindow` (accessibility root `WindowChrome`), not the native AppKit editor, so the native path fails and the bare `catch` hides why; next: surface the exception, then choose `net10.0-macos` or drop the bridge
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: no
- **Doc**: —
- **Summary**: `AppKitWorkflowServices.cs` uses `AppKit`/`Foundation` from a bare `Microsoft.macOS.dll` in a plain `net10.0` Avalonia app, with no native Xamarin runtime initialisation; every call is wrapped in a bare `catch` that silently falls back (and also swallows cancellation). Likely the native editor/print never run. Check in the launched app; then either move to a supported setup or drop the bridge, and narrow the `catch` with logging.

## BKLG-001 — Reproducible toolchain: .NET SDK, macOS pack and modern CI
- **Status**: in-progress — P1–P3 done locally: SDK in `./.dotnet`, locked restore re-downloads the macOS pack from an empty cache, `./start_scanfolder.command --publish` green; P4 parked: CI switched off by the user on 2026-11-12 (legacy workflow disabled on GitHub, `modern-ci.yml` manual only), so the remote check waits for a manual run
- **Priority**: high
- **Added**: 2026-11-12
- **Manual**: yes (approve the SDK download; push the branch so CI can run)
- **Doc**: [plan.md](bugs/toolchain-macos-reference/plan.md)
- **Summary**: The App and App.Tests reference `Microsoft.macOS.dll` through a `HintPath` into the git-ignored `./.dotnet/packs/...26.2.10197`, which no fresh clone or CI runner has, and `modern-ci.yml` runs only on `main` (never on this branch). Install the SDK locally, make the reference resolvable everywhere, run the modern CI on the porting branch.

## BKLG-011 — Ricontrollare e rivalidare la CI/CD su GitHub
- **Status**: open — CI switched off on 2026-11-12 by the user's choice; kept as the last entry on request
- **Priority**: low
- **Added**: 2026-11-12
- **Manual**: yes (decide when the CI goes back on; re-enable `NetFramework.CI` in GitHub › Actions, or approve `gh workflow enable`)
- **Doc**: —
- **Summary**: Review and revalidate the GitHub CI/CD before turning it back on: legacy `ci.yml` (disabled, last run on `main` failed on 2026-02-28), `modern-ci.yml` and `modern-manual.yml` (manual only) with the Avalonia 12 / NuGet macOS pack setup, the coverage gate (88% line / 71% branch, never computed on the new code), publish and bundle artifacts; then restore the automatic triggers and close [[BKLG-001]] P4.
