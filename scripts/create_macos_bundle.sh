#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
APP_HOST_NAME="ScanFolderToFile"
INFO_PLIST_SOURCE="$ROOT_DIR/src/ScanFolderToFile.App/Packaging/Info.plist"
ICON_SOURCE="$ROOT_DIR/scanFolderToFile/Resources/Deleket-Sleek-Xp-Basic-Files.ico"
ERROR_USAGE="Usage: scripts/create_macos_bundle.sh <publish-dir> <bundle-dir>"
ERROR_PUBLISH_DIR="The publish directory was not found."
ERROR_APP_HOST="The published app-host was not found."
ERROR_INFO_PLIST="The macOS Info.plist template was not found."
ERROR_ICON="The legacy app icon was not found."

if [[ $# -ne 2 ]]; then
  echo "$ERROR_USAGE" >&2
  exit 1
fi

if [[ "$1" = /* ]]; then
  PUBLISH_DIR="$1"
else
  PUBLISH_DIR="$ROOT_DIR/$1"
fi

if [[ "$2" = /* ]]; then
  BUNDLE_DIR="$2"
else
  BUNDLE_DIR="$ROOT_DIR/$2"
fi

CONTENTS_DIR="$BUNDLE_DIR/Contents"
MACOS_DIR="$CONTENTS_DIR/MacOS"
RESOURCES_DIR="$CONTENTS_DIR/Resources"

if [[ ! -d "$PUBLISH_DIR" ]]; then
  echo "$ERROR_PUBLISH_DIR" >&2
  exit 1
fi

if [[ ! -f "$PUBLISH_DIR/$APP_HOST_NAME" ]]; then
  echo "$ERROR_APP_HOST" >&2
  exit 1
fi

if [[ ! -f "$INFO_PLIST_SOURCE" ]]; then
  echo "$ERROR_INFO_PLIST" >&2
  exit 1
fi

if [[ ! -f "$ICON_SOURCE" ]]; then
  echo "$ERROR_ICON" >&2
  exit 1
fi

rm -rf "$BUNDLE_DIR"
mkdir -p "$MACOS_DIR" "$RESOURCES_DIR"

cp -R "$PUBLISH_DIR"/. "$MACOS_DIR/"
cp "$INFO_PLIST_SOURCE" "$CONTENTS_DIR/Info.plist"
cp "$ICON_SOURCE" "$RESOURCES_DIR/Deleket-Sleek-Xp-Basic-Files.ico"
chmod +x "$MACOS_DIR/$APP_HOST_NAME"

echo "Created macOS bundle at: $BUNDLE_DIR"
