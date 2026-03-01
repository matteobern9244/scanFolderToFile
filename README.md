# ScanFolderToFile

ScanFolderToFile e' un repository in transizione:

- contiene l'applicazione storica WinForms per Windows basata su .NET Framework 4.8
- contiene la nuova base moderna per il porting macOS, basata su .NET 10 e Avalonia

L'obiettivo finale e' mantenere la parita' funzionale tra Windows e macOS, ma il repository oggi e' organizzato per lavorare per step. I primi quattro step sono gia' implementati: il nuovo core cross-platform, la UI macOS del flusso principale, le utility operative principali, l'editor interno, la stampa/anteprima, test rigorosi, quality gates e CI/CD separata dalla pipeline legacy.

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

## Cosa E' Gia Stato Implementato Nei Piani 1 E 2

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
- launcher macOS intelligente:
  - `start_scanfolder.command`
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
`-- start_scanfolder.command        # launcher macOS
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

### Opzione 1: launcher intelligente

Il modo consigliato per avviare la base moderna e':

```bash
./start_scanfolder.command
```

Il launcher esegue in sequenza:

1. controllo formattazione (`dotnet format --verify-no-changes`)
2. auto-format se necessario
3. restore
4. build `Release` con `UseAppHost=true`
5. test
6. avvio dell'app moderna

Strategia di avvio:

- prova prima l'app-host nativo `ScanFolderToFile`
- se manca, usa il fallback `dotnet ScanFolderToFile.dll`

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
- line coverage minima: `90%`
- branch coverage minima: `85%`

Queste soglie sono allineate ai workflow GitHub Actions del progetto.

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

Esecuzione consigliata:

```bash
dotnet test ScanFolderToFile.Modern.sln -c Release
```

Per raccogliere coverage:

```bash
dotnet test tests/ScanFolderToFile.Core.Tests/ScanFolderToFile.Core.Tests.csproj \
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

Resta solo il packaging finale e l'hardening conclusivo.

## Roadmap Di Alto Livello

I prossimi step del porting completeranno:

- packaging desktop piu' avanzato
- hardening finale
- validazione conclusiva del porting

## Convenzioni Del Progetto

- il codice nuovo non deve dipendere da `System.Windows.Forms`
- le stringhe di produzione C# devono stare in file di costanti
- gli asset vanno riusati dal repository storico quando possibile
- le modifiche moderne devono passare tutti i quality gate prima del merge

## Licenza E Note Operative

Questo README descrive lo stato corrente del repository dopo i Piani 1, 2, 3 e 4. La codebase legacy resta presente per confronto, verifica funzionale e continuita' operativa mentre il porting verso macOS procede.
