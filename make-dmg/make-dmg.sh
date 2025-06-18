#!/bin/bash

# ─── Configuration ─────────────────────────────────────────────────────────────

APP_NAME="Don't Wake Up"                  # Application name (without extension)
NEW_DMG_NAME="dont-wake-up-1.0.0-mac.dmg" # Name of the DMG that will be created
BACKGROUND_IMG="dmg-background.png"       # Background image for the new DMG

create-dmg \
  --volname "$APP_NAME Installer" \
  --background "$BACKGROUND_IMG" \
  --window-pos 200 120 \
  --window-size 600 420 \
  --icon-size 100 \
  --icon "$APP_NAME.app" 150 190 \
  --app-drop-link 450 190 \
  --format UDBZ \
  "$NEW_DMG_NAME" \
  "."
