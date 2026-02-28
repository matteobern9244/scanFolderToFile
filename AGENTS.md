# AGENTS.md

These instructions are for coding agents working in this repository.

## Goals

- Prioritize correctness, maintainability, and functional parity between Windows and macOS.
- Keep changes minimal and scoped to the user request.
- Preserve existing architecture and naming conventions unless the task explicitly requires a refactor.
- Prefer completing one validated step at a time instead of mixing multiple unfinished changes.

## Repository Scope

- This repository is dual-track:
  - legacy Windows WinForms code lives in `scanFolderToFile/`
  - the modern macOS port lives in `src/`, `tests/`, and `ScanFolderToFile.Modern.sln`
- Treat the modern codebase as the default target for changes unless the user explicitly asks to modify the legacy Windows project.
- Do not break the legacy solution or delete legacy files unless explicitly requested.

## Working Style

- Explain planned changes briefly before editing.
- Prefer concrete fixes over speculative refactors.
- Do not change unrelated files.
- When implementing large plans, work in small verifiable slices and validate each slice before moving on.
- Keep user-facing summaries concise and include touched file paths.

## Editing Rules

- Prefer `rg` for search and `rg --files` for file discovery.
- Use ASCII unless the target file already uses Unicode or Unicode is required.
- Add comments only when logic is not obvious.
- Use `apply_patch` for manual edits.
- Never use destructive git commands unless explicitly requested.
- Do not revert user changes that are unrelated to the current task.

## Codebase Rules

- Keep production C# string literals centralized in `src/ScanFolderToFile.Core/Constants/AppStrings.cs` whenever practical.
- Reuse existing repository assets when possible before introducing new images, icons, or logos.
- Prefer extending the existing modern architecture (`Core`, `App`, tests) instead of creating parallel ad-hoc utilities.
- Avoid introducing UI-only behavior into the `Core` project.
- Keep the software-rendering fallback in the Avalonia app unless a rendering change is explicitly requested and validated.

## UI / Avalonia

- Keep the macOS UI consistent across windows and dialogs.
- Reuse shared styles, shared labels, and helper classes when possible.
- Avoid duplicating event-handling patterns that can be centralized in helper types.
- Favor pragmatic code-behind and small helper classes over unnecessary framework additions.
- Preserve a clear separation between:
  - main workflow UI
  - dedicated dialogs/windows
  - cross-platform core services

## Testing

- Run the smallest relevant tests first.
- For changes in the modern codebase, use the modern solution:
  - `dotnet build ScanFolderToFile.Modern.sln`
  - `dotnet test ScanFolderToFile.Modern.sln`
- For formatting validation, use:
  - `dotnet format ScanFolderToFile.Modern.sln`
- For launcher and end-to-end local validation on macOS, use:
  - `./start_scanfolder.command`
- If only documentation changes are made, state clearly that code tests were not needed.
- Report any tests not run and why.

## Pre-Commit Workflow

For every change that is going to be committed and pushed, always execute this sequence:

1. update `README.md` if repository state, workflow, setup, or operational documentation changed
2. update `CHANGELOG.md` with relevant changes
3. run `dotnet format` on the modern solution
4. only then proceed with `git commit`
5. only after the commit succeeds proceed with `git push`

This is a required repository policy for all work on the macOS port and the modern codebase.

## Communication

- Be concise and direct.
- Include touched file paths in summaries.
- If blocked, state the blocker and the next best option.
- When a plan is partially complete, state clearly what is done and what remains.
