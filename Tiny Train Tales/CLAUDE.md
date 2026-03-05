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
| PathFinding.cs | A* / Dijkstra pathfinding between cities on the map |
| City.cs | Individual city logic, cargo demand setup, cargo creation, saving |
| CityMenu.cs | City popup menu, travel button, mouse-over detection |
| CityMarketMenu.cs | Buy/sell cargo UI, demand tracking on sell, commit/reset market |
| CargoManager.cs | Player inventory, cargo creation, saving/loading cargo |
| CargoDemand.cs | UI component showing demanded cargo per city (icon, count text, slider) |
| CargoItem.cs | Individual cargo item — name, count, price, profit calc, save/load |
| MenuAnimationY.cs | Slides UI panels in/out vertically. Static `mapMenuOpen` flag persists across scene reloads |
| MapDragUI.cs | Map drag/pan logic, checks "OpenMap" PlayerPrefs to unlock movement on reload |
| Region.cs | Region unlock logic, city activation |

## Workflow Agreement
- User handles scene changes, GameObject placement, component wiring in Unity Editor
- Claude handles C# script writing and fixes

---

## Fixes Made This Session

### 1. CargoDemand display & sell tracking (CityMarketMenu.cs, City.cs)
**Problem:** Demanded cargo not displaying, and selling demanded cargo not adding to the demand count.
- `CityMarketMenu.BuySell()` was passing `finalStock` (remaining inventory) to `cargoDemand.AddCount()` instead of `sellAmount` (amount actually sold). Fixed to use `sellAmount`.
- Added null guard and `Debug.LogWarning` to `City.SetUpDemandCargo()` so missing/unmatched `cargoDemandName` logs clearly instead of silently failing.
- **Inspector checklist:** Each City needs `cargoDemandName` set to exactly match a name in CargoManager's `cargoItemsNames` array (case-sensitive). `cargoDemandAmount` must be > 0. CargoDemand GameObject must be active in scene.

### 2. CargoDemand slider & cap (CargoDemand.cs)
**Problem:** Demand count could exceed the max and slider was never updated.
- Added `[SerializeField] Slider slider` field — assign in Inspector.
- `SetItemCount()` now clamps count and sets slider min/max/value.
- `AddCount()` now clamps before exceeding max and only passes the actually-added amount to `City.AddCargoCount()`.

### 3. Pathfinding uses set distances not scene positions (PathFinding.cs)
**Problem:** A* was using Euclidean world-space distance for both actual cost and heuristic. This ignored the `cityNeighborsDistances` values set in the Inspector.
- Replaced `GetDistance()` (Euclidean) with `GetDefinedDistance()` which looks up the distance from the city's `cityNeighborsDistances` array.
- Set `hCost = 0` everywhere — with arbitrary graph weights there is no admissible heuristic derivable from the graph alone, so Dijkstra's (h=0) is used. Guarantees optimal path. On a small city map the performance difference is zero.
- Fallback returns `int.MaxValue / 2` for non-connected cities (half to avoid overflow).

### 4. Discarded pathfinding call (CityManager.cs)
**Noted but not yet fixed:** `pathfinding.FindPath(currentCity, destinationCity, null)` at line 145 in `GetNextCityInPath()` — result is discarded. Wasteful call, should be removed.

### 5. Map close button not closing after scene reload (MenuAnimationY.cs)
**Problem:** After selecting a target city (which reloads the scene), the map auto-opens. Pressing close closed the map canvas but the close button canvas stayed visible.
**Root cause:** In `MenuAnimationY.Start()`, the map menu called `otherMenu.SetMenuPosition()` which physically moved the close button canvas to its open position. If the close button canvas's own `Start()` ran after this, `savedPos = transform.position` captured the open position instead of the closed one. When close was pressed, it animated "back" to the open position.
**Fix:** Replaced direct `otherMenu.StartAnimation()` / `otherMenu.SetMenuPosition()` calls in `Start()` with a one-frame coroutine (`OpenOtherMenuNextFrame`), so all `Start()` methods finish first before the otherMenu is moved.

---

## Known Issues / TODO (from TODO.cs)
- Quests: one per city, some to unlock next area
- Timer for upgrades
- Cargo system (ongoing)
- Sprites for everything (user handles)

## PlayerPrefs Keys of Note
- `"OpenMap"` — set before scene reload to signal map should open on load
- `"CurrentCity"`, `"NextCity"`, `"DestinationCity"` — saved city state
- `"CargoDemandCount" + cityName` — per-city demand progress
- `"CargoItemAmount" + cityName` — number of cargo items in a city
- `"NumberOfCargoItems"`, `"CurrentCargoAmount"` — player inventory
