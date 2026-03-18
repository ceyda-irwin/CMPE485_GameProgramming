# CMPE 485 — HW#1: Simple 3D Maze Escape Game (Unity URP)

## Overview
Third-person maze escape game. The player navigates a maze, finds/pushes a physics key, and wins by colliding the key with the door. Traps and guards can kill the player.

## Controls
- **WASD / Arrow Keys**: Move player
- **M**: Toggle background music
- **UI Buttons** (Win/Lose): **Yes** restart, **No** quit

## Game Rules / Mechanics
- **Maze**: Multiple paths; at least one path contains the **Key**, another reaches the **Door**.
- **Key**:
  - A physics object (Rigidbody + Collider).
  - The player can **push** it around.
- **Win condition**:
  - When the **Key** collides with the **Door**, the game shows the **Win UI**.
- **Lose condition**:
  - If the player collides with a **Trap** or **Guard**, the game shows the **Lose UI**.
- **Another round**:
  - After winning or dying, UI asks for another round:
    - **Yes**: reloads the current scene
    - **No**: exits the game

## Traps
- Spike traps periodically move up/down using **Coroutines**.
- When active, touching the trap causes **Lose**.

## Guards
- Guards patrol back-and-forth between two points using **Coroutines**.
- Colliding with a guard causes **Lose**.

## Audio
- Background music loops during gameplay.
- Can be toggled on/off with **M**.

## Assets / Render Pipeline
- **Render Pipeline**: Universal Render Pipeline (URP)
- **Unity Asset Store content**: Environment / props imported (URP variant when available).

## How to Run
1. Open the project in Unity (URP).
2. Open scene: `Assets/Scenes/SampleScene.unity`
3. Press **Play**.

