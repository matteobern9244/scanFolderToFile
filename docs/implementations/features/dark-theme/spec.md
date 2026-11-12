# BKLG-010 — Dark theme by default, with a switch to the light theme

## Request (2026-11-12)

The app opens in a dark theme by default; the user can switch to the light theme from the app.

## Current state

- All 7 windows set `RequestedThemeVariant="Light"` in their `.axaml`.
- `App.axaml` defines the palette as fixed light `SolidColorBrush` resources (`WindowBackgroundBrush` `#F4F1EA`,
  `CardBackgroundBrush`, `HeroBackgroundBrush`, ...): they do not follow the theme variant.
- No user setting is persisted today besides the history file.

## Target

- `App.axaml` palette moved into `ResourceDictionary.ThemeDictionaries` with a `Light` (today's colours) and a
  `Dark` entry; every brush keeps its key, so the windows keep using `{DynamicResource ...}` unchanged.
- `Application.RequestedThemeVariant` = `Dark` by default; the per-window `RequestedThemeVariant="Light"` removed.
- A theme switch in the native menu (`Altro` › light/dark, a checkable item) that changes
  `Application.RequestedThemeVariant` at runtime for every open window.
- The choice persisted across launches (a small JSON settings file next to the history, through `IAppPaths`),
  read at startup; a missing or unreadable file means Dark.
- Labels and file names in `AppStrings`; the theme logic outside the windows' code-behind.

## Acceptance

- First launch: every window is dark, text readable (contrast checked on the screenshots).
- Switching to light changes all open windows at once and survives a restart; switching back works.
- Headless tests: default variant is Dark, the switch updates the variant and the stored setting, a corrupt
  settings file falls back to Dark.
- Screenshots of all windows in both themes.

## Plan

| | state |
|---|---|
| **P1** dark palette in `ThemeDictionaries`, default Dark, drop per-window Light | 📋 not started |
| **P2** theme switch in the native menu | 📋 not started |
| **P3** persisted preference (settings file, `IAppPaths`) | 📋 not started |
| **P4** tests, screenshots of every window in both themes, `README.md` / `CHANGELOG.md` | 📋 not started |
