# HW1_CameraViews 🎥
A Unity project demonstrating camera behavior and screen adaptation in a 2D mobile game.

## 🎯 Project Goal
This project was created as part of **Homework 1 - Section B** in the Game Development course.  
It demonstrates:
- A moving player character in a 2D world.
- A live minimap in the top-right corner.
- A camera scaling solution that ensures object sizes appear consistent when rotating the screen (Portrait ↔ Landscape).

---

## 🧩 Features

### 1. 🎮 Player Movement
- Controlled using arrow keys.
- Smooth and responsive movement using `Input.GetAxisRaw`.

### 2. 🗺️ Mini-Map
- A second orthographic camera renders the world from above.
- The minimap follows the player using a `FollowPlayer` script.
- It is displayed as an overlay in the top-right corner using `Viewport Rect`.

### 3. 🔁 Orientation Handling
- When the screen rotates (e.g. in Unity Simulator via "Rotate"), Unity changes the screen aspect ratio.
- This causes the `orthographicSize` of the main camera to appear zoomed in/out, making objects look bigger or smaller.

---

## ❓ Explanation – Why do objects appear larger in Portrait mode?

Unity's orthographic camera uses the `orthographicSize` property to define **half the height** of the visible area in world units.  
When the device is rotated from Landscape to Portrait:
- The screen becomes taller and narrower.
- Unity preserves the height (`orthographicSize`), but squeezes the width.
- This causes fewer world units to be visible → resulting in a **zoomed-in** appearance.
- Hence, all objects look larger on screen.

---

## 🛠️ Solution – Camera Scaling Script

A custom script (`OrthographicCameraScaler.cs`) was written to:
- Monitor screen orientation and aspect ratio in real time.
- Dynamically update `Camera.orthographicSize` to maintain a **consistent visual scale**.
- Two modes are supported:
  - **Simple mode:** Manually set camera size for each orientation.
  - **Advanced mode:** Compute size based on base aspect ratio and current screen ratio.

---

## 📂 Project Structure
- `Scripts/PlayerMovement.cs` – Basic movement logic.
- `Scripts/FollowPlayer.cs` – Keeps minimap camera following the player.
- `Scripts/OrthographicCameraScaler.cs` – Adjusts camera zoom for consistent appearance.
- `Scenes/MainScene.unity` – The demo scene.
- `Prefabs/` – Includes player and visual objects.
- `UI/` – (Optional) for displaying debug info or coordinates.

---

## 📱 Tested on
- Unity Editor Simulator (portrait and landscape)
- Target device: iPhone 12 (19.5:9 aspect ratio)

---

## 📦 How to Run
1. Clone the repo:
   ```bash
   git clone https://github.com/<your-username>/HW1_CameraViews.git
