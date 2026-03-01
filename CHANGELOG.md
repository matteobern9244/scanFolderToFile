# Changelog

Tutte le modifiche rilevanti di questo repository devono essere documentate in questo file.

Questo changelog segue il formato di [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) e usa versionamento semantico come riferimento operativo.

## [Unreleased]

### Added

- `AGENTS.md` con regole operative del repository per agenti, workflow di modifica, test e policy pre-commit
- Coverage pass dedicata con nuovi test mirati file-per-file su `Core` e `App` per aumentare la copertura reale del codice moderno
- Test UI headless in `tests/ScanFolderToFile.App.Tests` per la finestra principale e le finestre dedicate del flusso macOS
- Servizio `FileOperationsService` nel core per:
  - copia file
  - spostamento file
  - riordino per estensione
- Nuova finestra `UtilitiesWindow` per eseguire le utility operative da macOS
- Nuova finestra `DuplicatesWindow` per consultare e aprire i duplicati trovati
- Nuovi test del core per le utility operative
- Nuova finestra `EditorWindow` per l'editing interno dei file testuali
- Nuova finestra `PrintPreviewWindow` per l'anteprima di stampa
- Servizio `MacPrintService` per l'invio alla stampante di default su macOS
- Nuovi test UI headless per editor interno e anteprima di stampa
- Script `scripts/create_macos_bundle.sh` per assemblare il bundle macOS
- Template `Info.plist` per il bundle `ScanFolderToFile.app`
- Nuovi test di parita' per:
  - semantica ZIP-only del `ScanService`
  - auto-apertura dell'editor TXT
  - fallback di `Apri file generato` verso lo ZIP
- Nuovi servizi di workflow:
  - `IRichTextEditorService`
  - `IPrintWorkflowService`
  - `IEditorWindowLauncher`

### Changed

- Formalizzata la policy operativa del repository: prima di ogni `commit` e `push` vanno aggiornati `README.md` e `CHANGELOG.md`, ed eseguito `dotnet format`
- Formalizzato in `AGENTS.md` il target di copertura desiderato: `100%` line e `100%` branch per il codice moderno
- `MacExternalLauncher` e `MacPrintService` sono ora testabili con un comando iniettabile per consentire test affidabili sui wrapper macOS
- Il README e' stato riallineato allo stato reale del repository dopo il completamento del Piano 2
- La UI macOS non e' piu' descritta come shell minima: il flusso principale e' ora implementato in `src/ScanFolderToFile.App`
- Il README e' stato riallineato anche al completamento del Piano 3 e alle nuove utility operative
- Il README e' stato riallineato al completamento del Piano 4
- La UI Avalonia e' stata ripulita con stili condivisi e allineamento coerente di pulsanti, card, campi e testi tra tutte le finestre
- Le pipeline moderne GitHub Actions ora pubblicano anche il bundle macOS self-contained come artifact
- Il README e' stato riallineato al completamento del Piano 5 e al nuovo flusso di packaging
- Il repository usa ora un solo file `.command`: `start_scanfolder.command` gestisce sia il launch sia il publish con `--publish`
- I workflow GitHub Actions moderni verificano ora la coverage sul report Cobertura aggregato invece che sul primo file trovato
- I workflow GitHub Actions moderni generano ora sia il report coverage completo sia il report code-only, e usano il report code-only per i gate CI
- Le soglie CI della coverage sono state riallineate al floor oggi verificato dalla suite moderna (`88%` line, `71%` branch sul report code-only)
- `ScanService` ora riallinea la semantica ZIP a Windows:
  - con `Creare Zip Cartella Scelta` attivo genera solo lo ZIP
  - non esegue export TXT/PDF/Markdown
  - non esegue duplicate check
  - scrive una sola entry di storico
- `MainWindow` ora riallinea il caso TXT a Windows:
  - dopo `GENERA FILE`, se il formato e' TXT, apre automaticamente l'editor
  - `Apri file generato` usa il fallback allo ZIP quando la run e' ZIP-only
- L'app macOS usa ora un bridge nativo AppKit come superficie primaria per editor e stampa, mantenendo le finestre Avalonia come fallback tecnico
- Il launcher e i workflow moderni non forzano piu' l'installazione del workload `macos`, evitando errori di permessi sulle macchine dove il progetto e' gia' compilabile
- `.gitignore` esclude ora esplicitamente i tool locali `.tools/` e i `TestResults` annidati generati dalle run di coverage

### Completed

- Chiuso il Piano 2 del porting macOS con:
  - `MainWindow` rifinita per il flusso principale
  - `FilterWindow` dedicata
  - `HistoryWindow` dedicata
  - menu desktop macOS
  - preview interna dei risultati
  - comandi di apertura file e cartelle coerenti con macOS
- Chiuso il Piano 3 del porting macOS con:
  - `copy/move` integrati nella UI
  - riordino per estensione
  - finestra duplicati dedicata
  - menu utility attivati per le funzioni del piano
- Chiuso il Piano 4 del porting macOS con:
  - editor interno operativo
  - anteprima di stampa interna
  - stampa verso la stampante di default
  - voci `Editor interno` e `Stampa` attivate nel menu
- Chiuso il Piano 5 del porting macOS con:
  - packaging `.app` self-contained
  - publish integrato nello script `.command` principale
  - bundle macOS generato anche in CI
  - hardening finale della UI con layout e allineamenti uniformi

### Planned

- Signing e notarization Apple come evoluzione successiva facoltativa
- Eventuale conversione dell'icona legacy in `.icns`

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
