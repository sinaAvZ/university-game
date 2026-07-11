# GDD - Simple Collector

## Game Idea

Simple Collector is a small 2D top-down game. The player moves inside a closed area, collects all gems, and avoids moving enemies.

## Goal

The goal is to collect every gem in the level. If all gems are collected, the player wins. If the player touches an enemy, the game ends and the player can restart.

## Controls

- Move Up: W or Up Arrow
- Move Down: S or Down Arrow
- Move Left: A or Left Arrow
- Move Right: D or Right Arrow

## Main Objects

- Player: controlled by keyboard input
- Gems: collectible score items
- Enemies: moving obstacles
- Walls: keep the player inside the level
- UI: shows score and win/loss panel

## Gameplay Flow

1. The level starts with the player in the center.
2. The player moves around and collects gems.
3. Enemies move between fixed points.
4. Touching a gem increases the score.
5. Touching an enemy shows Game Over.
6. Collecting all gems shows You Win.
7. The Restart button reloads the level.

## Implementation

The project is implemented in Unity as a 2D game. The main scene is `Assets/Scenes/Main.unity`. The level is created by scripts when the scene starts.

Main scripts:

- `LevelSetup.cs`
- `PlayerController.cs`
- `GameManager.cs`
- `Collectible.cs`
- `SimpleEnemy.cs`
