# Mini Physics Puzzle Arena  
CMPE 485 – Unity Entry Project  

## 🎯 Project Overview

This project demonstrates the fundamental features of the Unity Engine using a 3D physics-based mini puzzle environment.

The objective of the game is to spawn different types of crates and deliver them to their correct target zones using physics interactions. Each lane has different surface properties (friction and bounce), and only the correct crate type should reach the corresponding target.

The project was implemented using the Built-in Render Pipeline.

---

## 🧱 Core Features Implemented

### 1️⃣ Object Creation
- Arena platform
- Three inclined lanes (Ice, Normal, Rubber)
- Target zones
- Player object
- Beacon objects
- Walls surrounding the arena

---

### 2️⃣ Rigidbody & Physics
- Dynamic Rigidbody on Player and Crates
- Kinematic Rigidbody on Arena
- Collision handling between crates, player, walls, and targets
- Use of Physic Materials with different friction and bounciness values

---

### 3️⃣ Physics Materials
Three Physic Materials were created:

- **Ice Lane**
  - Very low friction
  - High smoothness
- **Normal Lane**
  - Medium friction
- **Rubber Lane**
  - High friction
  - High bounciness

These materials produce visibly different physical behaviors on inclined surfaces.

---

### 4️⃣ Player Control (Input & AddForce)
The Player object:
- Uses `Rigidbody.AddForce`
- Controlled via WASD / Arrow Keys
- Speed is clamped to prevent unrealistic acceleration
- Demonstrates per-frame force application (assignment requirement 2.6)

---

### 5️⃣ Prefabs & Spawning System
- Crate is implemented as a Prefab
- Crates spawn from the Player’s current position
- Spawn controlled via:
  - 1 → Ice crate
  - 2 → Normal crate
  - 3 → Rubber crate

Each crate contains a type identifier used for validation.

---

### 6️⃣ Collision & Target Validation
- Target zones use Trigger Colliders
- When a correct crate enters its matching target:
  - Score increases
  - Crate is destroyed

---

### 7️⃣ UI & Game State
- Score displayed using TextMeshPro
- Win condition triggered after reaching target score
- Win panel displayed
- Player movement and spawning disabled after win
- Press R to restart the scene

---

### 8️⃣ Materials & Textures
Custom Materials were created using imported image textures.

Textures were assigned to:
- Albedo
- Metallic & Smoothness values modified to demonstrate visual differences
- Emission enabled on Beacon objects for glow effect

This demonstrates material property manipulation as required in the assignment.

---

### 9️⃣ Lighting (GPU-based)
The scene includes:
- Directional Light (main light source)
- Multiple Point Lights (Beacon lights)
- Soft shadows enabled

Beacon objects act as dynamic light sources.
Lighting calculations are GPU-based.

---

### 🔟 Animation
Beacon objects include:
- Scale-based pulse animation
- Optional animated light intensity
- Looping animation via Animator

---

### 1️⃣1️⃣ Sound
- Background music via AudioSource
- Toggle music using:
  - M → Pause/Resume

---

## 🎮 Controls

| Key | Action |
|-----|--------|
| WASD / Arrow Keys | Move Player |
| 1 | Spawn Ice Crate |
| 2 | Spawn Normal Crate |
| 3 | Spawn Rubber Crate |
| M | Toggle Music |
| R | Restart (after win) |

---

## 🏁 Win Condition

Deliver 3 correct crates to their corresponding target zones.

Upon completion:
- Win panel appears
- Player input is disabled
- Music and animations continue

---

## 🧠 Technical Notes

- Built using Unity Built-in Render Pipeline
- Physics interactions handled via Rigidbody and Physic Materials
- Lighting computed on GPU
- Game state managed through singleton-based ScoreManager

---

## 📹 Demonstration

A short demonstration video has been uploaded to Moodle showing:
- Physics differences between lanes
- Crate spawning
- Player interaction
- Score updates
- Win state
- Lighting and animation
- Sound toggle

---

## 👥 Collaborators

- at-ay
- SouthAscend

---

## 🛠 Unity Version

Unity 6000.x (Built-in Render Pipeline)