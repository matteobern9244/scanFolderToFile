#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "$0")" && pwd)"
SOLUTION_PATH="ScanFolderToFile.Modern.sln"
APP_PROJECT="src/ScanFolderToFile.App/ScanFolderToFile.App.csproj"
APP_PROJECT_DIR="src/ScanFolderToFile.App"
BUNDLE_SCRIPT="scripts/create_macos_bundle.sh"
ASSEMBLY_NAME="ScanFolderToFile"
CONFIGURATION="Release"
FRAMEWORK="net10.0"
RUNTIME="osx-arm64"
BUNDLE_DIR="dist/ScanFolderToFile.app"
PUBLISH_MODE="false"
USER_DOTNET="$HOME/.dotnet/dotnet"
LOCAL_DOTNET="$ROOT_DIR/.dotnet/dotnet"
STEP_FORMAT="[1/5] Checking formatting..."
STEP_RESTORE="[2/5] Restoring solution..."
STEP_BUILD="[3/5] Building solution..."
STEP_TEST="[4/5] Running tests..."
STEP_LAUNCH="[5/5] Launching application..."
STEP_PUBLISH="[5/5] Publishing macOS app bundle..."
FORMAT_DIFF="Formatting differences detected. Running auto-format..."
APP_HOST_FALLBACK="App-host launch failed. Falling back to the DLL..."
READY_MESSAGE="Self-contained macOS bundle ready:"
ERROR_DOTNET="A compatible dotnet executable was not found."
ERROR_SOLUTION="The modern solution file was not found."
ERROR_APP="No launchable app-host or DLL was produced."
ERROR_BUNDLE_SCRIPT="The macOS bundle helper script was not found."

APP_ARGS=()

while [[ $# -gt 0 ]]; do
  case "$1" in
    --publish)
      PUBLISH_MODE="true"
      ;;
    --arm64)
      RUNTIME="osx-arm64"
      ;;
    --x64)
      RUNTIME="osx-x64"
      ;;
    --debug)
      CONFIGURATION="Debug"
      ;;
    --release)
      CONFIGURATION="Release"
      ;;
    *)
      APP_ARGS+=("$1")
      ;;
  esac
  shift
done

PUBLISH_DIR="artifacts/publish/$RUNTIME"

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

if [[ "$PUBLISH_MODE" == "true" ]]; then
  if [[ ! -f "$BUNDLE_SCRIPT" ]]; then
    echo "$ERROR_BUNDLE_SCRIPT" >&2
    exit 1
  fi

  echo "$STEP_PUBLISH"
  "$DOTNET_CMD" publish "$APP_PROJECT" -c "$CONFIGURATION" -r "$RUNTIME" --self-contained true -p:UseAppHost=true --no-restore --nologo -v minimal -o "$PUBLISH_DIR"
  bash "$BUNDLE_SCRIPT" "$PUBLISH_DIR" "$BUNDLE_DIR"
  echo "$READY_MESSAGE $ROOT_DIR/$BUNDLE_DIR"
  exit 0
fi

APP_DIR="$APP_PROJECT_DIR/bin/$CONFIGURATION/$FRAMEWORK"
APP_HOST="$APP_DIR/$ASSEMBLY_NAME"
APP_DLL="$APP_DIR/$ASSEMBLY_NAME.dll"

echo "$STEP_LAUNCH"
if [[ -x "$APP_HOST" ]]; then
  if [[ ${#APP_ARGS[@]} -gt 0 ]]; then
    if "$APP_HOST" "${APP_ARGS[@]}"; then
      exit 0
    fi
  else
    if "$APP_HOST"; then
      exit 0
    fi
  fi

  echo "$APP_HOST_FALLBACK"
fi

if [[ -f "$APP_DLL" ]]; then
  if [[ ${#APP_ARGS[@]} -gt 0 ]]; then
    exec "$DOTNET_CMD" "$APP_DLL" "${APP_ARGS[@]}"
  fi

  exec "$DOTNET_CMD" "$APP_DLL"
fi

echo "$ERROR_APP" >&2
exit 1
