#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
SOLUTION_PATH="ScanFolderToFile.Modern.sln"
CONFIGURATION="Release"
FRAMEWORK="net10.0"
APP_PROJECT_DIR="src/ScanFolderToFile.App"
ASSEMBLY_NAME="ScanFolderToFile"
USER_DOTNET="$HOME/.dotnet/dotnet"
LOCAL_DOTNET="$ROOT_DIR/.dotnet/dotnet"
STEP_FORMAT="[1/5] Checking formatting..."
STEP_RESTORE="[2/5] Restoring solution..."
STEP_BUILD="[3/5] Building solution (Release)..."
STEP_TEST="[4/5] Running tests (Release)..."
STEP_LAUNCH="[5/5] Launching application..."
ERROR_DOTNET="A compatible dotnet executable was not found."
ERROR_SOLUTION="The modern solution file was not found."
ERROR_APP="No launchable app-host or DLL was produced."
FORMAT_DIFF="Formatting differences detected. Running auto-format..."
APP_HOST_FALLBACK="App-host launch failed. Falling back to the DLL..."

if [[ -x "$USER_DOTNET" ]]; then
  DOTNET_CMD="$USER_DOTNET"
elif [[ -x "$LOCAL_DOTNET" ]]; then
  DOTNET_CMD="$LOCAL_DOTNET"
elif command -v dotnet >/dev/null 2>&1; then
  DOTNET_CMD="dotnet"
else
  echo "$ERROR_DOTNET" >&2
  exit 1
fi

if [[ "$DOTNET_CMD" == "dotnet" ]]; then
  DOTNET_ROOT="$(cd "$(dirname "$(command -v dotnet)")" && pwd)"
else
  DOTNET_ROOT="$(cd "$(dirname "$DOTNET_CMD")" && pwd)"
fi
export DOTNET_ROOT

cd "$ROOT_DIR"

if [[ ! -f "$SOLUTION_PATH" ]]; then
  echo "$ERROR_SOLUTION" >&2
  exit 1
fi

echo "$STEP_FORMAT"
if ! "$DOTNET_CMD" format --verify-no-changes "$SOLUTION_PATH"; then
  echo "$FORMAT_DIFF"
  "$DOTNET_CMD" format "$SOLUTION_PATH"
fi

echo "$STEP_RESTORE"
"$DOTNET_CMD" restore "$SOLUTION_PATH" --locked-mode --nologo

echo "$STEP_BUILD"
"$DOTNET_CMD" build "$SOLUTION_PATH" -c "$CONFIGURATION" -p:UseAppHost=true --no-restore --nologo -v minimal

echo "$STEP_TEST"
"$DOTNET_CMD" test "$SOLUTION_PATH" -c "$CONFIGURATION" --no-build --nologo -v minimal

APP_DIR="$APP_PROJECT_DIR/bin/$CONFIGURATION/$FRAMEWORK"
APP_HOST="$APP_DIR/$ASSEMBLY_NAME"
APP_DLL="$APP_DIR/$ASSEMBLY_NAME.dll"

echo "$STEP_LAUNCH"
if [[ -x "$APP_HOST" ]]; then
  if "$APP_HOST" "$@"; then
    exit 0
  fi

  echo "$APP_HOST_FALLBACK"
fi

if [[ -f "$APP_DLL" ]]; then
  exec "$DOTNET_CMD" "$APP_DLL" "$@"
fi

echo "$ERROR_APP" >&2
exit 1
