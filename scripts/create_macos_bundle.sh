#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
ROOT_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
APP_HOST_NAME="ScanFolderToFile"
INFO_PLIST_SOURCE="$ROOT_DIR/src/ScanFolderToFile.App/Packaging/Info.plist"
ICON_SOURCE="$ROOT_DIR/scanFolderToFile/Resources/Deleket-Sleek-Xp-Basic-Files.ico"
ICON_NAME="Deleket-Sleek-Xp-Basic-Files"
ICON_SIZES=(16 32 128 256)
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
# macOS reads the bundle icon only from an .icns file: build one from the legacy .ico.
ICON_WORK_DIR="$(mktemp -d)"
trap 'rm -rf "$ICON_WORK_DIR"' EXIT
ICONSET_DIR="$ICON_WORK_DIR/$ICON_NAME.iconset"
mkdir -p "$ICONSET_DIR"
sips -s format png "$ICON_SOURCE" --out "$ICON_WORK_DIR/base.png" >/dev/null
for size in "${ICON_SIZES[@]}"; do
  sips -z "$size" "$size" "$ICON_WORK_DIR/base.png" --out "$ICONSET_DIR/icon_${size}x${size}.png" >/dev/null
  double_size=$((size * 2))
  if [[ "$double_size" -le 256 ]]; then
    sips -z "$double_size" "$double_size" "$ICON_WORK_DIR/base.png" --out "$ICONSET_DIR/icon_${size}x${size}@2x.png" >/dev/null
  fi
done
iconutil -c icns "$ICONSET_DIR" -o "$RESOURCES_DIR/$ICON_NAME.icns"
chmod +x "$MACOS_DIR/$APP_HOST_NAME"

echo "Created macOS bundle at: $BUNDLE_DIR"
