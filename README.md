# Gothic — R36S Port (PortMaster)

Port of **Gothic 1** and **Gothic 2: Night of the Raven** tailored for the **R36S** handheld retro gaming console (Rockchip RK3326 SoC, 4x Cortex-A35, 1 GB RAM).

The project combines the **Gothic Port Preparer** standalone Windows utility (.NET Framework 4.8), the **GothicN3DS** reimplementation engine (by ponuno), the ahead-of-time (AOT) recompiled AArch64 runtime via **pocket-native (v0.5.0)** (Zakuro / 3dsrecomp), and hardware-accelerated DRM/KMS Mali-G31 graphics on Rockchip RK3326.

---

## 🚀 Key Production Features

- **Verified OS Compatibility:** Launcher scripts and native AArch64 binaries verified across ArkOS, dArkOS, and dArkOSen firmware families.
- **Hardware-Accelerated 640×480 LCD Display:** Native DRM/KMS (`card0`) rendering with Mali-G31 GPU acceleration via pocket-native (`ZAKURO_FAST=1`, `ZAKURO_KMS=1`), delivering fluid performance at the R36S native 640×480 panel resolution (4:3 aspect ratio).
- **Audio Architecture & Dedicated R36S Fix:** DirectMusic soundtrack synthesized into clean WAV audio streams via `dmrender.exe`, combined with a patched AArch64 `pocket-native` audio backend (`dsp_voices.rs`) eliminating muted ALSA filters on the RK3326/RK817 sound codec.
- **Full Gamepad Controls:** Native handheld controls supporting both the modern **Console Scheme** (camera-relative analog movement, lock-on, quick-slots) and **Classic PC Scheme**, mapping all physical R36S buttons, dual analog sticks, D-Pad, and shoulder triggers.
- **Dual Title Support (Gothic 1 & 2):** Complete preparation support for both **Gothic 1 (Classic)** and **Gothic 2: Night of the Raven**, with automated world ZEN detection, DirectMusic staging, and asset packing.
- **Minimal Dependencies (.NET Framework 4.8):** Standalone desktop preparer (`GothicPortPreparer.exe`) runs out of the box on Windows 11 and 10 with zero external NuGet DLLs, SDK installations, or Linux/WSL/Docker requirements.
- **Automated Tool & Asset Pipeline:** Automatically fetches the latest tool releases (`g3ds-bake.exe`, `dmrender.exe`) from GitHub, validates game files, extracts VDFS archives, and bakes optimized `.pak` files with live streaming progress logs.
- **System Resource & Power Management:** Integrated CPU clock governor (`power.sh`) and EmulationStation suspension (`kill -STOP` / `kill -CONT`) to conserve memory and maximize battery life.

---

## 🛠️ Installation on R36S Console

### Required Game Files:
This port requires original game files from **Gothic 1** (v1.08k) or **Gothic 2: Night of the Raven** (v2.6).

Buy the original game legally:
- **[Gothic 1 on GOG](https://www.gog.com/en/game/gothic)** or **[Steam](https://store.steampowered.com/app/65540/Gothic_1/)**
- **[Gothic 2: Gold Edition on GOG](https://www.gog.com/en/game/gothic_2_gold_edition)** or **[Steam](https://store.steampowered.com/app/39510/Gothic_II_Gold_Edition/)**

---

### 📋 Simple Installation Steps:

#### 1. Prepare Game Assets on PC:
1. Launch `dist\GothicPortPreparer\GothicPortPreparer.exe`.
2. Select your game: **Gothic 1 (Classic)** or **Gothic 2 (Night of the Raven)**.
3. Click **Auto-Detect** (automatically searches standard Steam and GOG library locations) or click **Browse...** to select your game folder.
4. Choose an output directory (default: `Documents\R36S_Ports` or select your SD card directly).
5. Click **PREPARE PORT FOR R36S**.
6. The preparer will convert music, bake world geometry/assets into PAK archives, package the runtime, and generate launcher scripts.
7. Upon completion, Windows Explorer automatically opens the output folder with:
   - Port directory: `Gothic R36S` (or `Gothic 2 R36S`)
   - Launcher script: `Gothic-R36S.sh` (or `Gothic-2-R36S.sh`)

#### 2. Copy Port to Console SD Card:
Copy the generated folder and root launcher script to the `ports/` directory on your console's MicroSD card (`roms/ports/` on primary SD or `roms2/ports/` on secondary SD):

- **Gothic 1:**
  ```text
  ports/
  ├── Gothic-R36S.sh
  └── Gothic R36S/
  ```

- **Gothic 2:**
  ```text
  ports/
  ├── Gothic-2-R36S.sh
  └── Gothic 2 R36S/
  ```

#### 3. Launch on R36S:
1. Insert the MicroSD card into your R36S console.
2. Power on the console and navigate to the **Ports** section in EmulationStation.
3. Select and launch **Gothic** (or **Gothic 2**)!

---

## 📂 Repository Structure

- **[`dist/`](dist/):** Standalone release distribution directory:
  - `GothicPortPreparer/` — Precompiled application, bundled runtime archive, and conversion tools (`g3ds-bake.exe`, `dmrender.exe`).
- **[`GothicPortPreparer/`](GothicPortPreparer/):** C# source code (.NET Framework 4.8, Windows Forms):
  - `Models/` — Data models for bake options, game types, GitHub releases, and progress states.
  - `Services/` — Game detection, tool dependency manager, DirectMusic rendering, asset baking, and port packager.
  - `UI/` — User interface forms and layout (`MainForm.cs`).
- **[`RuntimeTemplate/`](RuntimeTemplate/):** R36S target runtime template files:
  - `pocket-native/` — AArch64 **pocket-native (v0.5.0)** executable with RK3326 audio fix, `run.sh` launcher, and `power.sh` CPU script.
  - `games/000400000FA3D500/` — Precompiled AArch64 `title.so` library, container `game.3ds`, and replacement system font.
- **[`tools/`](tools/):** Cached Windows binaries (`g3ds-bake.exe`, `dmrender.exe`) used during asset preparation.
- **[`build.bat`](build.bat):** Automated MSBuild build script for compiling `GothicPortPreparer.exe` from source.
- **[`RuntimeTemplate.zip`](RuntimeTemplate.zip):** Compressed template package used by the preparer for clean port staging.

---

## 💻 Building from Source

To compile the desktop preparer from source on Windows:

```cmd
build.bat
```

The script locates MSBuild, compiles the Release configuration (`net48`), and packages the resulting executable along with required tools into `dist\GothicPortPreparer\`.

---

## 🎮 Controls (R36S Gamepad)

The port features full gamepad support utilizing the GothicN3DS engine and pocket-native input layer. Players can select between the **Console Scheme** (recommended for handhelds, modeled after the modern console release) and the **Classic PC Scheme** via the in-game pause menu (**Pause → Controls → Scheme**).

### 🕹️ Movement & Camera (Console Scheme)
| R36S Button | In-Game Action | Description |
|---|---|---|
| **Left Analog Stick** | Character Movement | Camera-relative analog movement (run, walk, steer in 360°) |
| **Right Analog Stick** | Camera Control | Rotate camera and look around |
| **D-Pad** | Menu Navigation / Quick Slots | Navigate menus, inventory rows, and select active quick slots |

### ⚔️ Combat & Character Actions
| R36S Button | In-Game Action | Description |
|---|---|---|
| **A** | Interact / Confirm / Attack | Talk to NPC, search chest/body, pick up item, confirm menu choice, strike in combat |
| **B** | Jump / Cancel / Back | Jump forward, climb ledges, dismiss dialog/menu |
| **X** | Draw / Sheathe Weapon | Ready equipped melee/ranged weapon or holster it |
| **Y** | Quick Action / Inventory | Access quick item menu or toggle inventory view |
| **L1** / **L2** *(ZL)* | Quick Slots | **Tap:** Cycle to next weapon, bow, or spell.<br>**Hold + D-Pad:** Select specific quick slot item; release to use. |
| **R1** / **R2** *(ZR)* | Target Lock-On / Modifier | Lock camera onto nearest enemy or activate combat direction modifier |

### 🧭 Menus & System Shortcuts
| R36S Button | In-Game Action | Description |
|---|---|---|
| **Start** | Pause Menu | Open game menu, save/load game, adjust options and controls |
| **Select** | Journal / Map / Stats | Open quest log, area map, and character attribute sheet |
| **FN + Select** | Quit Session | Clean exit back to EmulationStation |

---

## ☕ Support

If you enjoy this port, you can support its development by buying me a coffee.

<a href="https://buymeacoffee.com/kadeshar"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" width="174" height="41"></a>

---

## 📜 Licenses and Credits

### Original Game

Thank you to **Piranha Bytes** and **THQ Nordic** for creating the legendary Gothic series (*Gothic 1* and *Gothic 2: Night of the Raven*).

### GothicN3DS

Thank you to **ponuno** and the contributors of [GothicN3DS](https://github.com/ponuno/GothicN3DS) for reverse-engineering the ZenGin engine and creating the handheld reimplementation.

### Runtime and Porting Components

Thank you to the maintainers and contributors of the components that make this handheld port possible:

- **pocket-native (v0.5.0)** by **whovian64** and contributors — AOT 3DS recompiler runtime (MIT License)
- **Zakuro** and **3dsrecomp** by **fearkov** — 3DS emulator core and ahead-of-time recompiler (MIT License)
- **PortMaster** community — open handheld gaming runtime ecosystem
- **Azahar / citra_system_archives** by **B3n30** — open-source shared font replacement (Montserrat / GNU Unifont)
- **DirectMusic Renderer (`dmrender`)** — DirectMusic synthesis engine for Gothic soundtracks
