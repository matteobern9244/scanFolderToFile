# BKLG-001 — Reproducible toolchain: .NET SDK, macOS pack and modern CI

## Findings (audit 2026-11-12, read from the repository and this Mac)

- No `dotnet` anywhere: not on `PATH`, not in `~/.dotnet`, not in `./.dotnet`. Build, tests, format and
  coverage could not run (exit 127), so nothing in the modern track has been verified on this machine.
- Host: macOS 27.2 arm64, Xcode 27.2 (`/Applications/Xcode.app`), Homebrew present, Node 26 via nvm.
- `global.json` pins SDK `10.0.103` with `rollForward: latestPatch`: it resolves only inside the 10.0.1xx band
  (latest 10.0.115). SDK 10.0.404 (latest 10.0) and 11.0.100 (Homebrew cask `dotnet-sdk`) do NOT satisfy it.
- `src/ScanFolderToFile.App/ScanFolderToFile.App.csproj` and `tests/ScanFolderToFile.App.Tests/*.csproj` reference
  `Microsoft.macOS.dll` via `HintPath`
  `../../.dotnet/packs/Microsoft.macOS.Runtime.osx.net10.0_26.2/26.2.10197/...`: a git-ignored local folder at an
  exact version. A system-wide SDK does not provide it; a fresh clone and the CI runner cannot build the App.
- `.github/workflows/modern-ci.yml` installs only the SDK (no macOS pack) and runs only on push/PR to `main`;
  `gh run list` shows no run on `porting_mac_os`. The modern pipeline has never validated this code.
- The Homebrew cask installs system-wide and needs an administrator password: not usable unattended.

## Options for the macOS reference

| Option | Effect |
|---|---|
| A. Download the runtime pack as a NuGet package (`Microsoft.macOS.Runtime.osx.net10.0_26.x`) via a `PackageDownload` and point the reference at the restored path | Works in any clone and in CI without workloads; still a non-standard use of the bindings |
| B. Install the `macos` workload in `./.dotnet` locally and in CI, keep the reference via an MSBuild property for the pack version | Closest to today; CI must run `dotnet workload install macos` |
| C. Move the App to a `net10.0-macos` TFM | The supported way to use AppKit, but a bigger change to how Avalonia is hosted; decide after [[BKLG-002]] |
| D. Drop the native bridge | Removes the dependency entirely; only if [[BKLG-002]] shows it never runs |

The choice depends on [[BKLG-002]]; P1 and P2 do not.

## Plan

| | state |
|---|---|
| **P1** install the SDK locally in `./.dotnet` with `dotnet-install.sh` (no sudo), version per [[BKLG-006]] scope | 📋 not started — needs approval for the download |
| **P2** green baseline on the current pins: restore `--locked-mode`, build, test, format | 📋 not started |
| **P3** make the `Microsoft.macOS` reference resolvable in a fresh clone (option A or B) | 📋 not started |
| **P4** run `modern-ci.yml` on the porting branch (add the branch or `workflow_dispatch`) and add the macOS pack step | 📋 not started |
| **P5** update `CLAUDE.md` (.NET SDK section) and `README.md` setup | 📋 not started |

## Acceptance

- In a fresh clone: `./start_scanfolder.command` restores, builds, tests and launches the app.
- `modern-ci.yml` is green on the porting branch.
