# KitchenChaosProject (Duck Chef Kitchen Rush)

[中文](README.md) | [English](README.en.md)

## Game Overview

The story begins with an **overdue payment notice**. Kada, our little yellow duck, was once just a helper in the kitchen, but to keep this fast-food shop afloat, they decide to take on the busiest shift alone. Customers won’t wait, and ingredients can’t be wasted—every burger must be delivered on time: prepare buns and toppings, grill patties to perfection, then assemble and serve fast. In a tight kitchen space, you’ll need to plan efficient routes, minimize wasted movement, and chain cooking and assembly smoothly to survive peak hours.

This is a lighthearted single-player kitchen challenge game. You control Kada, a round and adorable duck chef, in a fast-paced kitchen with constantly arriving orders: pick up ingredients, chop prep, cook, assemble burgers, and deliver them to customers within the time limit. Orders keep refreshing, the pace ramps up, and your execution speed and task sequencing are put to the test.

At its core, the game is about **sequence and planning**: which order to handle first, whether to grill or chop first, and when to deliver—all directly affect your completed count and final rating. As you master the kitchen rhythm, Kada grows from a frantic beginner into a steady “Duck Head Chef,” winning applause and reputation one hot burger at a time.

## Project Structure (Main Folders)

> This section describes the locations and purposes of the main folders only (not every file).

- `Assets/`
  - Main Unity content: scenes, scripts, prefabs, art/audio, and other game assets.
- `Assets/Scripts/`
  - Project scripts folder: primary C# code (gameplay, interactions, UI, systems).
- `Packages/`
  - Unity Package Manager dependencies and package configuration.
- `ProjectSettings/`
  - Unity project settings (input, rendering, quality, tags/layers, etc.).
- `UserSettings/`

  - Editor/user-local settings (usually environment-specific, not core game logic).

- `Library/` (generated)
  - Unity-generated cache and imported artifacts; normally not edited manually.
- `Logs/`
  - Unity/editor logs.
- `build/`

  - Build outputs (platform artifacts, executables, etc.).

- `openspec/`
  - Project specs and collaboration/process documents (proposals, conventions, etc.).

## Development Notes

- Solution/project files (e.g., `*.sln`, `*.csproj`) are used for IDE integration (Visual Studio / Rider).
- Documentation in the repo root (e.g., `*.md`) records architecture and improvement notes.

## Reference & Credit

The gameplay/implementation approach is inspired by:

- https://www.youtube.com/watch?v=AmGSEH7QcDg
