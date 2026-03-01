# ScanFolderToFile

ScanFolderToFile e' un repository in transizione:

- contiene l'applicazione storica WinForms per Windows basata su .NET Framework 4.8
- contiene la nuova base moderna per il porting macOS, basata su .NET 10 e Avalonia

L'obiettivo finale e' mantenere la parita' funzionale tra Windows e macOS. I cinque step del porting sono ora implementati: core cross-platform, UI macOS completa del flusso principale, utility operative, editor interno, stampa/anteprima, packaging `.app` self-contained, test rigorosi, quality gates e CI/CD separata dalla pipeline legacy.

## Stato Del Repository

### Legacy Windows

La codebase storica resta presente e non e' stata rimossa:

- solution: `scanFolderToFile.sln`
- progetto WinForms: `scanFolderToFile/`
- target: `.NET Framework 4.8`
- uso previsto: riferimento funzionale e manutenzione della versione Windows esistente

### Base Moderna Per Il Porting

La nuova base di lavoro e' gia presente:

- solution: `ScanFolderToFile.Modern.sln`
- `src/ScanFolderToFile.Core`: logica applicativa cross-platform
- `src/ScanFolderToFile.App`: applicazione desktop Avalonia per macOS
- `tests/ScanFolderToFile.Core.Tests`: unit test e integration test sul core
- `tests/ScanFolderToFile.App.Tests`: test UI headless sull'app macOS

## Cosa E' Gia Stato Implementato Nei Piani 1, 2, 3, 4 E 5

La nuova base moderna include:

- modelli di dominio per richieste, filtri, risultati e storico
- servizi per scansione file, filtri, duplicati, ZIP e persistenza dello storico
- servizio per utility operative:
  - copia file
  - spostamento file
  - riordino per estensione
- exporter per:
  - TXT
  - Markdown
  - PDF
- percorsi di default compatibili macOS:
  - `~/Documents/CONTENT`
  - `~/Documents/CONTENT/ZipFiles`
  - `~/Documents/CONTENT/History/HistoryFileCreated.json`
- centralizzazione delle stringhe di produzione in:
  - `src/ScanFolderToFile.Core/Constants/AppStrings.cs`
- manifest degli asset legacy riusabili in:
  - `src/ScanFolderToFile.Core/Constants/AppAssets.cs`
- costanti di build e quality gate in:
  - `src/ScanFolderToFile.Core/Constants/AppBuild.cs`
- applicazione Avalonia lanciabile con rendering software stabile su macOS
- rifinitura UI condivisa con stili centralizzati e layout allineati tra finestre, testi e pulsanti
- finestra principale macOS per il flusso core:
  - selezione cartella sorgente
  - selezione cartella output
  - scelta formato
  - toggle opzioni principali
  - preview interna dei risultati
  - apertura file e cartella
- finestra dedicata filtri
- finestra dedicata storico file creati
- finestra dedicata duplicati
- finestra utility file per:
  - copia
  - sposta
  - riordino per estensione
- editor interno per file testuali e preview modificabile
- finestra di anteprima di stampa con invio alla stampante di default su macOS
- menu desktop macOS con azioni principali e roadmap visibile
- unico script `.command` al root:
  - `start_scanfolder.command`
  - supporta sia avvio sia publish del bundle macOS
- bundle `.app` self-contained generabile localmente e in CI:
  - `dist/ScanFolderToFile.app`
- GitHub Actions moderna separata da quella legacy

## Struttura Del Repository

```text
.
|-- .github/
|   |-- workflows/
|   |   |-- ci.yml                  # pipeline legacy Windows
|   |   |-- modern-ci.yml           # CI moderna automatica
|   |   `-- modern-manual.yml       # workflow manuale
|-- AGENTS.md                       # regole operative per agenti e flusso pre-commit
|-- scanFolderToFile/               # applicazione WinForms legacy
|-- src/
|   |-- ScanFolderToFile.Core/      # core moderno
|   `-- ScanFolderToFile.App/       # app Avalonia macOS
|-- tests/
|   |-- ScanFolderToFile.Core.Tests/
|   `-- ScanFolderToFile.App.Tests/
|-- ScanFolderToFile.Modern.sln     # solution moderna
|-- scanFolderToFile.sln            # solution legacy
|-- global.json                     # SDK .NET 10 fissata
`-- start_scanfolder.command        # launcher e publish macOS
```

## Requisiti

Per lavorare sulla base moderna servono:

- macOS (target primario del porting attuale)
- .NET SDK 10.0.x
- accesso a una shell bash o zsh

Il repository include `global.json`, quindi la versione attesa della SDK e':

- `10.0.103`

Se non hai .NET 10 installato globalmente, il launcher puo' usare una copia locale in `.dotnet/dotnet` se presente.

## Avvio Rapido Su macOS

### Opzione 1: script unico intelligente

Per l'avvio standard il modo consigliato e':

```bash
./start_scanfolder.command
```

Lo script esegue in sequenza:

1. controllo formattazione (`dotnet format --verify-no-changes`)
2. auto-format se necessario
3. restore
4. build `Release` con `UseAppHost=true`
5. test
6. avvio dell'app moderna

Strategia di avvio:

- prova prima l'app-host nativo `ScanFolderToFile`
- se manca, usa il fallback `dotnet ScanFolderToFile.dll`

### Opzione 1B: packaging `.app` self-contained

Per generare un bundle macOS pronto da aprire con Finder:

```bash
./start_scanfolder.command --publish
```

Il comando esegue:

1. controllo formattazione (`dotnet format --verify-no-changes`)
2. auto-format se necessario
3. restore
4. build e test
5. `dotnet publish` self-contained per macOS
6. creazione del bundle `dist/ScanFolderToFile.app`

Varianti supportate:

- `./start_scanfolder.command --publish --arm64`
- `./start_scanfolder.command --publish --x64`
- `./start_scanfolder.command --publish --debug`

### Opzione 2: comandi manuali

```bash
dotnet restore ScanFolderToFile.Modern.sln --locked-mode
dotnet build ScanFolderToFile.Modern.sln -c Release -p:UseAppHost=true --no-restore
dotnet test ScanFolderToFile.Modern.sln -c Release --no-build
dotnet run --project src/ScanFolderToFile.App/ScanFolderToFile.App.csproj
```

## Quality Gates

La base moderna e' protetta da quality gate obbligatori:

- `dotnet format --verify-no-changes`
- `dotnet build`
- `dotnet test`
- line coverage minima CI (report code-only): `88%`
- branch coverage minima CI (report code-only): `71%`

I workflow GitHub Actions producono:

- un report coverage completo per artifact e consultazione
- un report coverage code-only (esclude i file `.axaml`) usato per i gate CI

Queste soglie sono allineate ai workflow GitHub Actions del progetto e riflettono il floor oggi verificato dalla suite.

Target operativo richiesto per il codice moderno:

- line coverage: `100%`
- branch coverage: `100%`

Il repository oggi mantiene ancora una soglia minima CI piu' bassa mentre la suite viene portata gradualmente verso la copertura completa.

## Workflow Prima Di Commit E Push

Per mantenere il repository coerente, prima di ogni `commit` e `push` bisogna sempre:

1. aggiornare `README.md` se lo stato del progetto o la documentazione operativa sono cambiati
2. aggiornare `CHANGELOG.md` con le modifiche rilevanti
3. eseguire `dotnet format` sulla solution moderna

La policy e' formalizzata anche in:

- `AGENTS.md`

## Test

La suite moderna contiene:

- unit test su validazione, path, storico, filtri, duplicati, exporter e orchestrazione
- unit test sulle utility operative (`copy/move/riordino`)
- integration test mirati su filesystem temporaneo reale
- test UI headless su:
  - finestra principale
  - filtri
  - storico
  - duplicati
  - utility file
  - editor interno
  - anteprima di stampa
- controlli di policy per:
  - costanti centralizzate
  - asset dichiarati
  - presenza del launcher
  - copertura aggiuntiva dei branch di UI e servizi con test dedicati

Esecuzione consigliata:

```bash
dotnet test ScanFolderToFile.Modern.sln -c Release
```

Per raccogliere coverage:

```bash
dotnet test ScanFolderToFile.Modern.sln \
  -c Release \
  --collect:"XPlat Code Coverage;Format=cobertura"
```

## CI/CD

Il repository mantiene due pipeline distinte.

### Pipeline legacy

File:

- `.github/workflows/ci.yml`

Scopo:

- build della solution WinForms legacy su runner Windows

### Pipeline moderna

File:

- `.github/workflows/modern-ci.yml`
- `.github/workflows/modern-manual.yml`

Scopi:

- validazione automatica su `push` e `pull_request` verso `main`
- workflow manuale con `workflow_dispatch`
- generazione artifact scaricabili
- publish self-contained per `osx-arm64`
- creazione e upload del bundle `ScanFolderToFile.app`
- verifica automatica delle soglie di coverage

Le pipeline moderne girano su:

- `macos-14`

## Asset Grafici

Il porting riusa gli asset gia presenti nel repository storico quando possibile.

Asset principali gia mappati:

- `scanFolderToFile/Resources/open.png`
- `scanFolderToFile/Resources/open-file-icon.png`
- `scanFolderToFile/Resources/open-file-icon1.png`
- `scanFolderToFile/Resources/print.png`
- `scanFolderToFile/Resources/file.png`
- `scanFolderToFile/Resources/Deleket-Sleek-Xp-Basic-Files.ico`

La mappatura canonica vive in:

- `src/ScanFolderToFile.Core/Constants/AppAssets.cs`

## Stato Della UI macOS

La UI moderna copre gia' il flusso principale del porting:

- finestra principale operativa
- filtri dedicati
- storico dedicato
- duplicati dedicati
- utility operative integrate
- editor interno integrato
- stampa e anteprima integrate
- preview interna dei risultati
- menu macOS con azioni principali
- bundle macOS self-contained generabile e archiviato in CI
- layout UI ripulito e riallineato tra finestre, pulsanti e testi

Il porting pianificato in cinque piani e' quindi chiuso a livello funzionale. Restano solo eventuali evoluzioni successive non ancora incluse nel perimetro iniziale, come signing, notarization e affinamenti di distribuzione.

## Roadmap Di Alto Livello

Il perimetro del porting iniziale e' completato. Le evoluzioni naturali successive, fuori dai cinque piani originali, sono:

- signing del bundle macOS
- notarization Apple
- eventuale conversione dell'icona legacy in formato `.icns`
- distribuzione pubblica oltre gli artifact GitHub Actions

## Convenzioni Del Progetto

- il codice nuovo non deve dipendere da `System.Windows.Forms`
- le stringhe di produzione C# devono stare in file di costanti
- gli asset vanno riusati dal repository storico quando possibile
- le modifiche moderne devono passare tutti i quality gate prima del merge

## Licenza E Note Operative

Questo README descrive lo stato corrente del repository dopo il completamento dei Piani 1, 2, 3, 4 e 5. La codebase legacy resta presente per confronto, verifica funzionale e continuita' operativa mentre la base macOS moderna resta ora il target principale di manutenzione e rifinitura.
