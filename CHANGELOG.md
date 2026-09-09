# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/).

## [1.0.0] - 2026-09-09

### Added

- Compact always-on-top panel that shows the timestamp of the earliest
  Windows System/Application event log entry recorded today.
- Panel closes (and the app terminates) via its close button or Escape.
- Auto-update on startup: checks GitHub Releases, downloads silently in
  the background, then shows a one-line prompt to restart and apply it.
- GitHub Actions release workflow that builds, tests, and packages the
  app with Velopack, publishing a self-contained Windows installer.
