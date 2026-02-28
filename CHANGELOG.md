# Changelog

Tutte le modifiche rilevanti di questo repository devono essere documentate in questo file.

Questo changelog segue il formato di [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) e usa versionamento semantico come riferimento operativo.

## [Unreleased]

### Added

- `AGENTS.md` con regole operative del repository per agenti, workflow di modifica, test e policy pre-commit
- Test UI headless in `tests/ScanFolderToFile.App.Tests` per la finestra principale e le finestre dedicate del flusso macOS

### Changed

- Formalizzata la policy operativa del repository: prima di ogni `commit` e `push` vanno aggiornati `README.md` e `CHANGELOG.md`, ed eseguito `dotnet format`
- Il README e' stato riallineato allo stato reale del repository dopo il completamento del Piano 2
- La UI macOS non e' piu' descritta come shell minima: il flusso principale e' ora implementato in `src/ScanFolderToFile.App`

### Completed

- Chiuso il Piano 2 del porting macOS con:
  - `MainWindow` rifinita per il flusso principale
  - `FilterWindow` dedicata
  - `HistoryWindow` dedicata
  - menu desktop macOS
  - preview interna dei risultati
  - comandi di apertura file e cartelle coerenti con macOS

### Planned

- Porting progressivo di tutte le funzionalita' attualmente disponibili nella versione Windows
- Evoluzione del packaging macOS oltre gli artifact di CI

## [0.1.0] - 2026-02-28

### Added

- Nuova solution moderna `ScanFolderToFile.Modern.sln`
- Nuovo progetto `src/ScanFolderToFile.Core` basato su `.NET 10`
- Nuovo progetto `src/ScanFolderToFile.App` come shell Avalonia minima
- Nuovo progetto test `tests/ScanFolderToFile.Core.Tests`
- File `global.json` con pin della SDK `.NET 10.0.103`
- File `Directory.Build.props` con policy di build condivise
- File `Directory.Packages.props` con versioni NuGet centralizzate
- File `src/ScanFolderToFile.Core/Constants/AppStrings.cs` per la centralizzazione delle stringhe di produzione
- File `src/ScanFolderToFile.Core/Constants/AppAssets.cs` per il mapping degli asset legacy
- File `src/ScanFolderToFile.Core/Constants/AppBuild.cs` per costanti di build e quality gates
- Modelli di dominio per:
  - formato output
  - richiesta di scansione
  - filtri
  - risultato
  - storico
  - gruppi duplicati
- Contratti di servizio per path, scansione, export, storico, ZIP, duplicati e launcher esterno
- Implementazioni del core per:
  - validazione request
  - risoluzione path macOS
  - scansione ricorsiva
  - filtri per dimensione e data
  - rilevazione duplicati case-insensitive
  - persistenza JSON dello storico
  - export TXT
  - export Markdown
  - export PDF
  - creazione ZIP
  - orchestrazione end-to-end tramite `ScanService`
- Launcher macOS `start_scanfolder.command` con:
  - format check
  - auto-format locale
  - restore
  - build
  - test
  - avvio con fallback tra app-host e DLL
- Workflow GitHub Actions moderno automatico:
  - `.github/workflows/modern-ci.yml`
- Workflow GitHub Actions manuale:
  - `.github/workflows/modern-manual.yml`
- Suite di test moderna con:
  - unit test
  - integration test
  - controlli di policy

### Changed

- Il repository e' passato da sola base WinForms legacy a struttura dual-track:
  - legacy Windows
  - base moderna per il porting macOS
- La documentazione tecnica e operativa e' stata riallineata al nuovo assetto
- La gestione del versioning dei package e' stata spostata verso `PackageReference` con lock file

### Improved

- Quality gates introdotti per il codice moderno:
  - `dotnet format --verify-no-changes`
  - build pulita
  - test automatici
  - controllo coverage
- Reuse esplicito degli asset grafici esistenti del repository storico
- Base pronta per CI/CD moderna separata dal workflow legacy

### Fixed

- Separazione netta tra codice legacy WinForms e nuova base del porting
- Eliminazione di dipendenze WinForms dal nuovo codice moderno

## [Legacy]

### Notes

- La solution storica `scanFolderToFile.sln` e il progetto WinForms `scanFolderToFile/` restano presenti nel repository
- La pipeline legacy `.github/workflows/ci.yml` resta attiva per la build Windows
- Il codice legacy continua a fungere da baseline funzionale per il porting
