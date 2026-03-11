# Tiny Train Tales — Claude Session Notes

## Project
Unity 2D train game. Player travels between cities, buys/sells cargo, completes quests, and upgrades their train.

## Project Structure
- Assets/Scripts/ — all C# game scripts
- Assets/Prefabs/ — prefabs for train, cities, cargo, UI canvases
- Assets/Scenes/ — GameScene, StartScene, Test

## Key Scripts & Responsibilities

| Script | Purpose |
|---|---|
| GameManager.cs | Coins, gems, speed, distance, passengers, save/load, region unlocking |
| CityManager.cs | Tracks current/next/destination city, pathfinding calls, saving city state |
| PathFinding.cs | Dijkstra pathfinding between cities (h=0, uses Inspector-defined distances) |
| City.cs | Individual city logic, cargo demand setup, cargo creation, saving |
| CityMenu.cs | City popup menu, travel button, mouse-over detection |
| CityMarketMenu.cs | Buy/sell cargo UI, demand tracking on sell, commit/reset market |
| CargoManager.cs | Player inventory, cargo creation, saving/loading cargo |
| CargoDemand.cs | UI component showing demanded cargo per city (icon, count text, slider) |
| CargoItem.cs | Individual cargo item — name, count, price, profit calc, save/load |
| MenuAnimationY.cs | Slides UI panels in/out vertically. Static `mapMenuOpen` flag persists across scene reloads |
| MapDragUI.cs | Map drag/pan logic, checks "OpenMap" PlayerPrefs to unlock movement on reload |
| Region.cs | Region unlock logic, city activation |
| Train.cs | Train movement, acceleration, speed, stopping at station, animation |
| UpgradeManager.cs | Handles all upgrade purchases and costs, saves/loads via SaveSystem |
| SaveSystem.cs | Singleton. All JSON save/load logic. Called via SaveSystem.Instance |
| DebugShortcuts.cs | Keybind-based debug tool to add/remove coins, gems, speed, acceleration in-game |

## Workflow Agreement
- User handles scene changes, GameObject placement, component wiring in Unity Editor
- Claude handles C# script writing and fixes

---

## Save System Architecture

The game is migrating from PlayerPrefs to a JSON-based save system using `SaveSystem.cs`.
`SaveSystem` is a singleton (`SaveSystem.Instance`) that lives in the scene and persists via DontDestroyOnLoad.
All save files are stored in `Application.persistentDataPath` (same path for Editor and Windows builds).

### JSON Save Files

| File | Class | Contents |
|---|---|---|
| `cities.json` | `CitiesSaveFile` | Per-city cargo demand count, reset time, cargo items |
| `inventory.json` | `InventorySaveFile` | Player cargo inventory + currentCargoAmount |
| `train.json` | `TrainSaveFile` | trainSpeed, trainAcceleration |
| `currency.json` | `CurrencySaveFile` | coins, gems, networth |
| `upgrades.json` | `UpgradeSaveFile` | maxSpeed, maxPassangers, profitMultiplier, all upgrade costs and counts, amountOfCars |
| `passenger.json` | `PassangerSaveFile` | passangers (current count) |

### Save/Load Pattern
- `SaveToDisk()` — writes all files to disk. Called at end of `SaveAll()` in GameManager.
- `LoadFromDisk()` — called in `Awake()`. Reads all files from disk.
- Each domain has `Get___Data()` / `Set___Data()` accessors on SaveSystem.
- **Read-modify-set pattern** must be used when multiple scripts share a file (e.g. upgrades.json is written by both GameManager and UpgradeManager).
- `DeleteSaveFile()` — clears all in-memory data and deletes all JSON files.

### What Still Uses PlayerPrefs
- `"OpenMap"` — signals map should open on scene reload
- `"CurrentCity"`, `"NextCity"`, `"DestinationCity"` — city state
- `"Distance"`, `"RemainingDistance"` — journey progress (temporary, deleted on arrival)
- `"AutoCollect"`, `"AutoLeave"` — toggle settings
- `"UnlockedRegion" + index` — region unlock flags
- `"HasStartedGame"` — checked in GameManager.Awake() to decide whether to load StartScene

### Planned JSON Migrations (not yet done)
- `regions.json` — unlocked region indexes (currently PlayerPrefs)

### SaveAll() Call Order (GameManager.cs)
1. SaveCurrency() → currency.json
2. train?.SaveTrain() → train.json
3. cam?.SavePos()
4. questManager?.SaveQuests()
5. carManager?.SaveCars()
6. cargoManager?.SaveCargo() → inventory.json + cities.json
7. cityManager?.SaveOnDeparture() or SaveCityOnQuit()
8. MenuAnimationY buttons SavePos()
9. Car[] SaveCar()
10. SaveUpgrades() → upgrades.json (GameManager fields: maxSpeed, maxPassangers, profitMultiplier)
11. upgradeManager?.SaveUpgradeData() → upgrades.json (UpgradeManager fields: costs, counts, amountOfCars)
12. SavePassangers() → passenger.json
13. SaveProgress() → PlayerPrefs Distance/RemainingDistance
14. PlayerPrefs.Save()
15. saveSystem?.SaveToDisk()

---

## Fixes & Changes Made

### 1. CargoDemand display & sell tracking (CityMarketMenu.cs, City.cs)
- `BuySell()` was passing `finalStock` instead of `sellAmount` to `cargoDemand.AddCount()`. Fixed.
- Added null guard and `Debug.LogWarning` to `City.SetUpDemandCargo()` for missing/unmatched cargo names.
- **Inspector checklist:** `cargoDemandName` must exactly match a name in CargoManager's `cargoItemsNames` array (case-sensitive). `cargoDemandAmount` must be > 0. CargoDemand GameObject must be active.

### 2. CargoDemand slider & cap (CargoDemand.cs)
- Added `[SerializeField] Slider slider` — assign in Inspector.
- `SetItemCount()` now clamps count and sets slider min/max/value.
- `AddCount()` clamps before exceeding max.

### 3. Pathfinding uses Inspector distances (PathFinding.cs)
- Replaced Euclidean `GetDistance()` with `GetDefinedDistance()` that reads `cityNeighborsDistances` from Inspector.
- Set `hCost = 0` (Dijkstra) since arbitrary graph weights have no admissible heuristic.
- Non-connected city fallback returns `int.MaxValue / 2`.

### 4. Map close button after scene reload (MenuAnimationY.cs)
- Root cause: `otherMenu.SetMenuPosition()` in `Start()` moved the close button canvas before its own `Start()` ran, causing it to capture the wrong `savedPos`.
- Fix: replaced with a one-frame coroutine `OpenOtherMenuNextFrame` so all `Start()` methods complete first.

### 5. Inventory not loading after scene reload (Station.cs)
- Root cause: `LeaveStation()` called `SaveCargo()` (in-memory only) then immediately reloaded the scene, destroying SaveManager before `SaveToDisk()` was called.
- Fix: added `CitySaveManager.Instance?.SaveToDisk()` before `SceneManager.LoadScene()`.

### 6. Train save migrated to JSON (Train.cs)
- `LoadTrain()` now reads from `SaveSystem.Instance.GetTrainData()` and assigns `speed` and `acceleration`.
- `SaveTrain()` now calls `SaveSystem.Instance.SetTrainData(...)`.
- Removed stray `PlayerPrefs.SetFloat` from `AddToAcceleration()`.

### 7. Currency migrated to JSON (GameManager.cs)
- `LoadCurrencyData()` reads from SaveSystem.
- `SaveCurrency()` writes to SaveSystem.
- Removed all PlayerPrefs reads/writes for coins, gems, networth.

### 8. Upgrades migrated to JSON (GameManager.cs, UpgradeManager.cs)
- `LoadUpgradesData()` in GameManager loads maxSpeed, maxPassangers, profitMultiplier from SaveSystem.
- `SaveUpgrades()` in GameManager saves those three fields using read-modify-set.
- `LoadSavedData()` in UpgradeManager loads all costs, counts, amountOfCars from SaveSystem.
- `SaveUpgradeData()` (public) in UpgradeManager saves all cost/count fields using read-modify-set.
- `upgradeManager?.SaveUpgradeData()` called from `SaveAll()` to ensure costs are always saved even if no upgrade was purchased.
- Removed all PlayerPrefs reads/writes for upgrade values.

### 9. Passengers migrated to JSON (GameManager.cs)
- `LoadPassangerData()` reads from SaveSystem.
- `SavePassangers()` writes to SaveSystem.
- Removed PlayerPrefs write for Passangers in `AddAndSubtractPassangers()`.

### 10. DebugShortcuts added (DebugShortcuts.cs)
- Keybind-driven debug tool. Hold a value key + press increase/decrease key.
- Supports: coins, gems, maxSpeed, acceleration.
- Configured entirely in Inspector (no hardcoded keys or values).

---

## Known Issues / TODO
- Discarded pathfinding call in `CityManager.GetNextCityInPath()` — result of `FindPath()` at line ~145 is never used. Should be removed.
- Regions not yet migrated to JSON (still using PlayerPrefs `"UnlockedRegion" + index`).
- Quests: one per city, some to unlock next area
- Timer for upgrades
- Sprites for everything (user handles)
- Encryption for save files — deferred until closer to release
