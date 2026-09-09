# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

## [1.1.3] - 2026-09-09

### Fixed

- v1.1.2 only renamed the invisible OS window title (used for Alt-Tab),
  not the visible in-panel header, which still read "EARLIEST EVENT
  TODAY". The panel header now reads "ESTIMATED START TIME".

## [1.1.2] - 2026-09-09

### Changed

- Window title changed to "Estimated Start Time".

## [1.1.1] - 2026-09-09

### Fixed

- "Click to restart now" prompt did nothing: clicking it also started
  the window's drag-to-move behavior, which captured the mouse and
  swallowed the click before it reached the restart handler.

## [1.1.0] - 2026-09-09

### Added

- Custom clock app icon, used for the taskbar/Alt-Tab presentation and
  the application window.

## [1.0.0] - 2026-09-09

### Added

- Compact always-on-top panel that shows the timestamp of the earliest
  Windows System/Application event log entry recorded today.
- Panel closes (and the app terminates) via its close button or Escape.
- Auto-update on startup: checks GitHub Releases, downloads silently in
  the background, then shows a one-line prompt to restart and apply it.
- GitHub Actions release workflow that builds, tests, and packages the
  app with Velopack, publishing a self-contained Windows installer.
