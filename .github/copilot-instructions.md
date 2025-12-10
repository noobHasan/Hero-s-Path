# Hero's Path - Copilot Instructions

## Project Overview

**Hero's Path** is a Unity 6.0 (6000.2.6f2) 3D adventure game with physics-based player movement, animation systems, and world interaction. The project uses the Universal Render Pipeline (URP) for graphics and the new Input System for player controls.

### Key Technologies
- **Engine**: Unity 6.0.2.6f2
- **Target Platform**: Windows Standalone (64-bit)
- **Graphics**: Universal Render Pipeline (URP 17.2.0)
- **Input**: New Input System (1.14.2) via `InputSystem_Actions.inputactions`
- **Animation**: Animator-driven character animation with Rigidbody physics
- **Additional**: Cinemachine 3.1.5 (camera system), AI Navigation 2.0.9

## Architecture & Core Components

### Game Scene Structure
- **Single main scene**: `Assets/Scenes/SampleScene.unity`
- **Layers**: Default, TransparentFX, Water, UI, Cars, Player, Ground
- **Key tags**: "Ground" (for collision detection)

### Player Controller Pattern (`Assets/Scripts/PlayerController.cs`)
The player controller demonstrates the engine's architecture:

```
Update Loop (Input) → FixedUpdate Loop (Physics) → Collision Callbacks
```

**Key pattern**: Input is captured in `Update()`, but movement is applied in `FixedUpdate()` using `Rigidbody.MovePosition()` for smooth, physics-aware movement.

**Critical implementation details**:
- Movement input stored in private field `_moveDir` between Update and FixedUpdate cycles
- Jump triggered via `Rigidbody.AddForce()` with `ForceMode.Impulse`
- Animation state synchronized via `Animator.SetBool()` (e.g., "isRunning", "isJumping")
- Ground state tracked through `OnCollisionEnter/Exit()` callbacks checking for "Ground" tag
- Rotation applied immediately in Update for responsive camera control feel

### GameObject Component Structure
```
PlayerCharacter (GameObject)
├── Rigidbody (body="Dynamic", constraints="Freeze Rotation")
├── Collider (Capsule)
├── Animator (references humanoid controller)
├── PlayerController (MonoBehaviour script)
└── Models And Animation/[NPC assets]
```

## Developer Workflows

### Building & Running
- **Build target**: Standalone Windows 64-bit (preset in EditorBuildSettings)
- **Play in Editor**: Use Unity Editor play mode; physics behavior matches build
- **Scene Management**: Only `SampleScene.unity` is gameplay; others are for prototyping

### Physics Debugging
- Rigidbody constraints: Rotation frozen to prevent toppling during movement
- Use Gizmos to visualize collision bounds and Rigidbody axes
- Physics Update Rate: Check `Time.fixedDeltaTime` in TimeManager.asset (default 0.02s)

### Animation Integration
- Animator state machine defines transitions between idle/running/jumping
- Parameters are **boolean** flags (not enums):
  - `"isRunning"` = character moving
  - `"isJumping"` = airborne
- Animation clips loaded from `Assets/Models And Animation/` folder
- **Never** set `Animator.enabled = false`; instead, freeze animation via `speed = 0`

## Code Patterns & Conventions

### Input Handling
- **Old Input System used**: `Input.GetAxisRaw()` and `Input.GetButtonDown()` (legacy, not new InputSystem package)
- Normalize movement vector: `new Vector3(x, 0, z).normalized`
- Input captured in `Update()`, applied physics in `FixedUpdate()`

### Physics Movement
```csharp
// Preferred pattern (smooth, frame-rate independent)
Vector3 targetPos = rb.position + moveDir * speed * Time.fixedDeltaTime;
rb.MovePosition(targetPos);

// Avoid: Direct velocity assignment (can break physics constraints)
// rb.velocity = ...
```

### Collision Detection
- Use `OnCollisionEnter(Collision collision)` for discrete physics events
- Always check **tag** or **layer** to identify collision type: `collision.gameObject.CompareTag("Ground")`
- Avoid `Physics.OverlapSphere()` in per-frame code; cache results

### Animator Parameter Updates
```csharp
// Set state flags in Update()
animator.SetBool("isRunning", moveDir.magnitude > 0);
animator.SetBool("isJumping", !isGrounded);

// For transitions: single bool flags, not int/float enums
```

## Project Structure

```
Assets/
├── Scripts/
│   └── PlayerController.cs          # Core player logic
├── Scenes/
│   └── SampleScene.unity             # Main gameplay scene
├── Models And Animation/             # Character models, rigs, animation clips
├── Material/                         # Game materials and textures
├── Settings/                         # URP/Quality settings
├── Plugins/                          # Third-party DLLs
├── ithappy/                          # Cartoon City free assets
├── Fantasy Skybox FREE/              # Skybox textures
├── Stylized NPC - Peasant Nolant/   # NPC character prefab
├── InputSystem_Actions.inputactions # Input action map (currently unused in code)
└── TutorialInfo/                    # Readme UI system
```

## Critical Integration Points

### Input System Gap
- **Defined**: `Assets/InputSystem_Actions.inputactions` (Move, Look, Attack actions)
- **Used**: Neither the new InputSystem package nor legacy Input Manager integrated into PlayerController
- **Pattern for extending**: If adding new actions, reference actions via `PlayerInput` component or manual `InputActionMap.performed` callbacks

### Rendering Pipeline
- URP configured in `ProjectSettings/URPProjectSettings.asset`
- Custom materials should use URP-compatible shaders (avoid Standard shader)
- Postprocessing enabled (v3.5.0); Add effects in URP Renderer asset

### Cross-Scene Concerns
- No DontDestroyOnLoad patterns currently; all objects destroyed between scenes
- UI rendered via `Canvas` component; check `Canvas/EventSystem` for input blocking

## Debugging & Troubleshooting

- **Character not moving**: Check `Rigidbody.constraints` (should allow XZ movement), `isGrounded` state
- **Animation not playing**: Verify Animator is not disabled and state parameters match animator's state machine
- **Jump feels floaty**: Adjust `jumpForce` and `Time.fixedDeltaTime` ratio
- **Collisions not detected**: Ensure objects have Rigidbody+Collider and one is not kinematic; check tags match "Ground"

## Guidelines for AI Agents

1. **Physics-first mentality**: Always use `Rigidbody.MovePosition()` for movement, not Transform.position
2. **Animation is non-negotiable**: Sync Animator parameters whenever state changes
3. **Ground detection matters**: Jumping/landing transitions depend on accurate ground state tracking
4. **Normalize input vectors**: Player input is raw directional input, must normalize before use
5. **Don't fight the URP**: Use URP shaders and materials; avoid legacy Standard/built-in shaders
6. **Test in FixedUpdate context**: Physics behavior in build differs subtly from Editor; always test physics-related changes in standalone build
