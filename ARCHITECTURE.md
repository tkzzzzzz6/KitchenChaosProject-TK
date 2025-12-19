# Project Architecture: KitchenChaosProject

This document outlines the architectural design, core systems, and code organization of the **KitchenChaosProject**.

## 1. High-Level Overview
The project follows a standard Unity **Component-Based Architecture**, heavily utilizing **ScriptableObjects** for data management and **Events** for decoupling input from logic.

### Key Design Patterns
- **Singleton:** Used for major managers (`Player`, `OrderManager`) to allow easy global access.
- **Command/Event-Driven:** `GameInput` raises events that the `Player` and other systems listen to, decoupling the input method from the game actions.
- **Inheritance & Composition:** 
  - A robust inheritance hierarchy for Counters (`BaseCounter` -> Specific Counters).
  - A `KitchenObjectHolder` base class allows both the `Player` and `Counters` to share logic for holding items.
- **Data-Driven Design:** All game items and recipes are defined as `ScriptableObject` assets, making it easy to balance the game without changing code.

---

## 2. Directory Structure & Organization

### `Assets/Scripts`
The core logic is located here, organized by functionality:

- **Root (`/`):** Core managers and base classes (`GameInput.cs`, `Player.cs`, `OrderManager.cs`, `KitchenObjectHolder.cs`).
- **`Counter/`:** Logic for all interactive kitchen counters.
  - `BaseCounter.cs`: Abstract base class defining the interaction interface.
  - `ContainerCounter.cs`, `CuttingCounter.cs`, etc.: Concrete implementations.
- **`ScriptsObjects/`:** C# definitions for ScriptableObjects (`RecipeSO`, `KitchenObjectSO`).
- **`UI/`:** UI-specific logic (`ProgressBarUI`, `KitchenObjectIconUI`).

### `Assets/ScriptObjects`
Contains the actual data assets (instances of the classes in `ScriptsObjects/`), such as:
- **Recipes:** `CuttingRecipeList`, `FryingRecipeList`.
- **Items:** Specific `KitchenObjectSO` assets (Tomato, Cheese, etc.).

---

## 3. Core Systems

### 3.1 Input System
- **`GameInput.cs`**: Wraps Unity's generated `GameControl` class (New Input System).
- **Responsibility**: Detects input and fires C# events (e.g., `OnInteractAction`, `OnOperateAction`).
- **Usage**: `Player.cs` subscribes to these events to trigger gameplay actions.

### 3.2 Player & Interaction
- **`Player.cs`**: 
  - **Singleton**.
  - Handles movement and collision.
  - Acts as the central "Actor". When input is received, it performs a raycast to detect a `BaseCounter` and calls its `Interact()` or `InteractOperate()` methods.
  - Inherits from `KitchenObjectHolder` to carry items.

### 3.3 Counter System
- **`KitchenObjectHolder.cs`**: Base class handling the logic of holding, setting, and clearing a `KitchenObject`.
- **`BaseCounter.cs`**: Inherits from `KitchenObjectHolder`. Defines the abstract methods `Interact(Player)` and `InteractOperate(Player)`.
- **Polymorphism**: The Player interacts with `BaseCounter`, unaware of the specific type (e.g., Stove, Cutting Board), allowing for easy extensibility.

### 3.4 Kitchen Objects & Recipes
- **`KitchenObject.cs`**: The physical MonoBehaviour in the scene representing an item (Tomato, Plate).
- **`KitchenObjectSO`**: Data definition (Prefab, Sprite, Name).
- **`RecipeSO`**: Defines crafting logic (Input Item -> Output Item + Progress).
- **`OrderManager.cs`**: 
  - **Singleton**.
  - Manages the game loop of generating orders.
  - Validates deliveries by comparing the `PlateKitchenObject` contents against the active `orderRecipeSOList`.

### 3.5 UI
- **Component-Based**: UI elements like `ProgressBarUI` are likely attached directly to their relevant in-game objects (e.g., a cutting counter) and update based on local events or state changes.

---

## 4. Data Flow Example: "Cutting a Tomato"

1.  **Input**: User presses "Interact".
2.  **GameInput**: Fires `OnInteractAction`.
3.  **Player**: Receives event, detects `CuttingCounter` via raycast.
4.  **Polymorphism**: Calls `cuttingCounter.Interact(this)`.
5.  **Logic**: `CuttingCounter` checks if Player is holding a Tomato.
6.  **Data**: Checks `CuttingRecipeSO` to see if Tomato is cuttable.
7.  **State Change**: If valid, the Tomato is placed on the counter.
8.  **Input**: User presses "Operate" (Cut).
9.  **Logic**: `CuttingCounter` advances progress.
10. **Visuals**: `ProgressBarUI` updates to show cutting progress.
11. **Result**: When progress completes, the Tomato is destroyed and replaced by a Tomato Slices object (defined in the SO).
