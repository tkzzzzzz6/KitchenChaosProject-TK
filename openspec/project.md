# Project Context

## Purpose
Kitchen Chaos is a 3D cooking simulation game where players control a character that interacts with various kitchen counters to prepare and deliver food orders within a time limit. The game features:
- Time-based gameplay with countdown and game-over states
- Order management system with recipe matching
- Interactive kitchen counters (cutting, cooking, plating, delivery)
- Player movement and object interaction mechanics
- UI feedback and visual animations

## Tech Stack
- **Unity 6000.0.30f1** - Game engine
- **C#** - Primary programming language
- **Universal Render Pipeline (URP) 17.2.0** - Rendering pipeline
- **Unity Input System 1.14.2** - New Input System for player controls
- **Unity AI Navigation 2.0.9** - Pathfinding and navigation
- **Unity UI (uGUI) 2.0.0** - User interface system
- **Vscode** - Primary IDE 

## Project Conventions

### Code Style
- **Class naming**: PascalCase for all classes, interfaces, and methods
- **Private fields**: camelCase with no prefix (e.g., `moveSpeed`, `selectedCounter`)
- **SerializeField**: Used extensively for Unity Inspector exposure, placed on private fields
- **Events**: PascalCase with `On` prefix (e.g., `OnStateChanged`, `OnInteractAction`)
- **Singleton pattern**: Static `Instance` property with PascalCase naming
- **Comments**: Minimal inline comments; Unity auto-generated comments sometimes present
- **Regions**: Not used; code organized by method grouping instead
- **Header attributes**: Used for Inspector organization (e.g., `[Header("Movement")]`)

### Architecture Patterns
1. **Singleton Managers**: GameManager, OrderManager, SoundManager, MusicManager, GameInput all use singleton pattern with static `Instance` property
2. **Inheritance Hierarchy**:
   - `KitchenObjectHolder` base class → `Player` and `BaseCounter`
   - `BaseCounter` → specific counter types (ClearCounter, CuttingCounter, etc.)
3. **Event-Driven Communication**: Heavy use of C# events for decoupling
   - Managers fire events (e.g., `GameManager.OnStateChanged`)
   - UI components subscribe to manager events
   - Static events used for cross-scene communication
4. **ScriptableObject Data**: All game data (recipes, kitchen objects, audio clips) stored as SOs
5. **State Machine**: GameManager uses enum-based state machine (WaitingToStart → CountDownToStart → GamePlaying → GameOver)
6. **Component-Based Visuals**: Separate *Visual classes for animations (ContainerCounterVisual, CuttingCounterVisual, etc.)
7. **Physics-Based Movement**: Player movement uses Rigidbody when available, with capsule collision detection

### Testing Strategy
- Currently no automated test framework in use
- Manual testing through Unity Editor play mode
- Unity Test Framework package is available but not actively used

### Git Workflow
- **Branch**: Currently on `main` branch
- **Commit style**: Lowercase messages (e.g., "change projectsettings.asset", "init")
- **.gitignore**: Comprehensive Unity gitignore excluding Library/, Temp/, Obj/, Build/, IDE files, and CLAUDE.md/GEMINI.md
- **Files tracked**: Assets/, ProjectSettings/, Packages/manifest.json, .gitignore
- **Files ignored**: All generated Unity files, .csproj/.sln files, IDE settings

## Domain Context

### Game Loop
1. **WaitingToStart** (1s): Player disabled, UI shows waiting state
2. **CountDownToStart** (3s): Countdown displayed, player still disabled
3. **GamePlaying** (30s): Main gameplay, orders spawn every 2s (max 5 concurrent), player enabled
4. **GameOver**: Final score displayed, player disabled

### Kitchen Object System
- **Kitchen Objects**: Ingredients, plates, and food items that can be held
- **Holders**: Player and counters can hold exactly one kitchen object at a time
- **Transferring**: Objects move between holders via `TransferKitchenObject()`
- **Interactions**: Two interaction types:
  - `Interact()`: Pick up/place objects
  - `InteractOperate()`: Perform actions (cut, cook, etc.)

### Counter Types
- **ContainerCounter**: Spawns specific kitchen objects (e.g., tomato dispenser)
- **ClearCounter**: Simple storage surface
- **CuttingCounter**: Transforms objects via cutting (requires CuttingRecipeSO)
- **StoveCounter**: Cooks/fries objects (uses FryingRecipeSO)
- **PlatesCounter**: Spawns plates with timer
- **DeliveryCounter**: Validates and accepts completed recipes
- **TrashCounter**: Destroys held objects

### Recipe System
- **RecipeSO**: Defines list of required ingredients for a complete dish
- **RecipeListSO**: Pool of all possible recipes for random order generation
- **Order Matching**: Delivered plates validated against active orders (exact ingredient match required)
- **Scoring**: Tracks successful delivery count

## Important Constraints

### Unity-Specific
- **Scene-based architecture**: Scene transitions require static data cleanup to prevent memory leaks
- **Static event cleanup**: All static events must be cleared via `ClearStaticData.cs` when returning to menu
- **MonoBehaviour lifecycle**: Singleton instances set in `Awake()`, event subscriptions in `Start()`, cleanup in `OnDestroy()`
- **Time.timeScale**: Used for pause (0 = paused, 1 = running)
- **Layer masks**: Counter detection uses `counterLayerMask` for raycasting and collision

### Performance
- **Object pooling**: Not currently implemented; objects instantiated/destroyed dynamically
- **Physics**: Uses capsule collision for player, raycasting for counter detection
- **Update vs FixedUpdate**: Movement logic in `FixedUpdate()`, UI/interaction logic in `Update()`

### Input System
- **Rebinding persistence**: Key bindings saved to PlayerPrefs as JSON
- **Input filtering**: Optional joystick/XR input filtering for movement
- **Deadzone**: Configurable movement deadzone (default 0.2)
- **Binding indices**: Hardcoded indices for rebinding (Movement composite: Up=2, Down=4, Left=6, Right=8)

## External Dependencies

### Unity Packages (Packages/manifest.json)
- **com.unity.inputsystem@1.14.2**: New Input System with rebinding support
- **com.unity.render-pipelines.universal@17.2.0**: URP rendering
- **com.unity.ai.navigation@2.0.9**: NavMesh and navigation components
- **com.unity.ugui@2.0.0**: Unity UI system
- **com.unity.collab-proxy@2.10.2**: Unity Version Control integration
- **com.unity.multiplayer.center@1.0.0**: Multiplayer tools (not actively used)

### Input Actions Asset
- **GameControl.inputactions**: Defines all input bindings
  - Player.Move: WASD + Arrow keys + Gamepad
  - Player.Interact: E key
  - Player.Operate: F key
  - Player.Pause: Escape key

### No External Services
- No cloud services, APIs, or external databases
- All data stored locally in PlayerPrefs (input bindings only)
- Fully offline single-player experience
