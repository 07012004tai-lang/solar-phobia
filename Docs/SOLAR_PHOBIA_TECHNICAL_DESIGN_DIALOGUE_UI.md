# SOLAR PHOBIA: NẮNG GẮT
## TECHNICAL DESIGN: DIALOGUE UI SYSTEM
### Complete Implementation Guide for Programmers

**Version:** 1.0  
**Engine:** Godot 4 (or Phaser 3 for web)  
**Target Platform:** Windows, WebGL (itch.io)  
**Priority:** Core system for Act 1  

---

## TABLE OF CONTENTS

1. [System Overview](#system-overview)
2. [State Machine Architecture](#state-machine-architecture)
3. [Data Structures](#data-structures)
4. [UI Components & Layout](#ui-components--layout)
5. [Dialogue Flow & Branching Logic](#dialogue-flow--branching-logic)
6. [Integration with Game Loop](#integration-with-game-loop)
7. [Testing & Debug Checklist](#testing--debug-checklist)

---

## SYSTEM OVERVIEW

### Purpose
The Dialogue UI System manages:
- ✅ Ghost spawning and customer queue
- ✅ Order display and player selection
- ✅ Dialogue rendering (text, speaker name, character portrait)
- ✅ Branching dialogue based on player choices
- ✅ Kindness score tracking (for ending determination)
- ✅ Emotional state visualization (aura color, form stability)

### Key Features
- **Non-blocking dialogue** (player can continue gameplay while dialogue is displayed)
- **Branching paths** (player choices affect NPC emotional state and future dialogue)
- **Dynamic text rendering** (typewriter effect, voice-over lip-sync ready)
- **Portrait system** (character emotional states reflected in visual)
- **Queue management** (max 3 orders concurrent, respects turn order)

---

## STATE MACHINE ARCHITECTURE

### Global Game State

```
GameState {
  phase: "DAY" | "NIGHT"
  currentShrine: Shrine
  playerKindnessScore: int (0-100)
  gameTime: float (0-24 hours)
  timeUntilSunset: float (minutes)
}
```

### NPC/Ghost State Machine

```
NPCState = IDLE | APPROACHING | WAITING | BEING_SERVED | DIALOGUE | DEPARTING | RESOLVED

NPCStateMachine {
  currentState: NPCState
  npc: Ghost
  enterTime: float
  onStateChange(): void
  
  transitions: {
    IDLE → APPROACHING: OnSpawnTimer()
    APPROACHING → WAITING: OnReachStall()
    WAITING → BEING_SERVED: OnPlayerSelectOrder()
    BEING_SERVED → DIALOGUE: OnOrderComplete()
    DIALOGUE → DEPARTING: OnDialogueEnd()
    DEPARTING → RESOLVED: OnFadeComplete()
    WAITING → DIALOGUE: OnWaitTimeExpire() [Bad ending]
  }
}
```

### NPC Emotional State

```
EmotionalState {
  satisfaction: 0-100 (affects aura brightness)
  stability: 0-100 (affects flickering)
  kindnessReceived: int (player interaction count)
  dialogue_unlocked: DialogueNode[] (tree of unlocked lines)
  arc_progress: 0-3 (which "visit" is this: 1st, 2nd, 3rd)
}
```

### Dialogue Node Tree Structure

```
DialogueNode {
  id: string (e.g., "van_visit_1_start")
  npc: string (e.g., "van")
  character: string (speaker name, e.g., "Ông Văn")
  portrait: Texture (character image state)
  text: string (Vietnamese text, supports tags like <pause>, <speed=1.5>)
  
  type: "STATEMENT" | "QUESTION" | "ACTION"
  
  choices: Choice[] (if type is QUESTION)
  next: DialogueNode | null (if type is STATEMENT)
  
  effects: DialogueEffect[] (kindness gain, aura change, etc.)
  condition: bool (required game state for this node)
}

Choice {
  text: string (player choice text)
  target: DialogueNode (leads to this node)
  kindnessValue: int (+1, 0, or -1)
  effect: DialogueEffect (what happens if chosen)
}

DialogueEffect {
  type: "KINDNESS_GAIN" | "EMOTIONAL_SHIFT" | "AURA_COLOR" | "SPAWN_EFFECT" | "SOUND"
  value: float
}
```

---

## DATA STRUCTURES

### Ghost/NPC Definition

```gdscript
class_name Ghost

var name: String  # "van", "linh", "minh", "caong"
var display_name: String  # "Ông Văn", "Em Linh", etc.
var personality: String  # "gruff", "shy", "bitter", "desperate"
var dialogue_tree: Dictionary  # Keyed by dialogue_id

var current_state: String = "IDLE"
var emotional_state: EmotionalState
var visit_count: int = 0  # 1st, 2nd, or 3rd visit
var has_left: bool = false

var sprite: AnimatedSprite2D
var position: Vector2

# Methods
func _ready() -> void:
  emotional_state = EmotionalState.new()
  _load_dialogue_tree()

func _load_dialogue_tree() -> void:
  # Load from JSON or Dialogue file
  dialogue_tree = load("res://data/dialogues/%s.json" % name).data

func update_emotional_state(kindness_delta: int) -> void:
  emotional_state.kindnessReceived += kindness_delta
  emotional_state.satisfaction = clamp(emotional_state.satisfaction + kindness_delta * 5, 0, 100)
  
  # Unlock next dialogue node if threshold met
  if emotional_state.kindnessReceived >= 1:
    emotional_state.dialogue_unlocked.append("visit_2_start")
  if emotional_state.kindnessReceived >= 2:
    emotional_state.dialogue_unlocked.append("visit_3_start")

func change_aura_color(color: Color) -> void:
  # Animate aura glow to new color
  var tween = create_tween()
  tween.tween_property(aura_sprite, "self_modulate", color, 0.5)

func trigger_dialogue(dialogue_id: String) -> void:
  var node = dialogue_tree[dialogue_id]
  DialogueManager.display_dialogue(node)
```

### Dialogue Manager (Singleton)

```gdscript
class_name DialogueManager
extends Node

var current_dialogue: DialogueNode
var dialogue_history: Array[DialogueNode] = []
var is_dialogue_active: bool = false

signal dialogue_started(node: DialogueNode)
signal dialogue_ended(node: DialogueNode)
signal choice_made(choice: Choice, kindness_delta: int)

func display_dialogue(node: DialogueNode) -> void:
  current_dialogue = node
  is_dialogue_active = true
  dialogue_history.append(node)
  
  # Update game state
  GameManager.add_kindness(node.effects[0].kindness_gain if node.effects else 0)
  
  # Render dialogue
  _render_dialogue_ui(node)
  
  dialogue_started.emit(node)

func make_choice(choice: Choice) -> void:
  # Track choice
  choice_made.emit(choice, choice.kindnessValue)
  
  # Update NPC state
  var npc = _get_npc_by_id(current_dialogue.npc)
  npc.update_emotional_state(choice.kindnessValue)
  
  # Progress dialogue
  display_dialogue(choice.target)

func _render_dialogue_ui(node: DialogueNode) -> void:
  # Update portrait
  DialogueUI.portrait.texture = node.portrait
  
  # Display character name
  DialogueUI.character_label.text = node.character
  
  # Render text with typewriter effect
  DialogueUI.start_typewriter(node.text, 0.05)  # 20 chars per second
  
  # Show choices if question type
  if node.type == "QUESTION":
    DialogueUI.show_choices(node.choices)
  else:
    # Auto-advance on timer or space key
    await get_tree().create_timer(2.0).timeout
    if node.next:
      display_dialogue(node.next)
    else:
      end_dialogue()

func end_dialogue() -> void:
  is_dialogue_active = false
  dialogue_ended.emit(current_dialogue)
```

### Order/Customer Definition

```gdscript
class_name Order

enum OrderType { TEA, INCENSE, MONEY, RICE, WINE }

var id: String
var npc: Ghost
var items_needed: Dictionary  # { OrderType: quantity }
var time_limit: float = 30.0  # seconds
var created_at: float

var satisfaction: float = 100.0  # Decreases over time
var is_complete: bool = false

func _process(delta: float) -> void:
  # Decrease satisfaction over time
  var elapsed = Time.get_ticks_msec() / 1000.0 - created_at
  var progress = elapsed / time_limit
  satisfaction = max(0, 100 * (1 - progress))
  
  # Check if time expired
  if satisfaction <= 0:
    on_timeout()

func add_item(item_type: OrderType) -> void:
  if items_needed.has(item_type):
    items_needed[item_type] -= 1
    if items_needed[item_type] <= 0:
      items_needed.erase(item_type)
  
  # Check if order complete
  if items_needed.is_empty():
    complete_order()

func complete_order() -> void:
  is_complete = true
  var kindness_bonus = int(satisfaction / 10)  # 0-10 kindness points
  
  # Trigger dialogue
  npc.trigger_dialogue("%s_visit_%d_satisfied" % [npc.name, npc.visit_count])
  
  # Emit reward
  GameManager.add_spirit_essence(1 + (kindness_bonus / 5))

func on_timeout() -> void:
  # NPC becomes unstable
  npc.emotional_state.stability -= 20
  npc.change_aura_color(Color.RED)
  
  # Trigger "bad" dialogue
  npc.trigger_dialogue("%s_visit_%d_angry" % [npc.name, npc.visit_count])
```

---

## UI COMPONENTS & LAYOUT

### Dialogue Canvas (Rendered at bottom of screen)

```
┌─────────────────────────────────────────────┐
│ [Portrait: 80x120px]  [Character Name]      │
│                                              │
│ ┌──────────────────────────────────────────┐ │
│ │ Dialogue Text (typewriter effect)        │ │
│ │ "Trà đặc, đặc..."                       │ │
│ └──────────────────────────────────────────┘ │
│                                              │
│ [Choice A]  [Choice B]  [Choice C]  [Skip]  │
└─────────────────────────────────────────────┘
```

### Dialogue UI Components (Godot)

```gdscript
class_name DialogueUI
extends CanvasLayer

@onready var portrait: TextureRect = $VBoxContainer/HBoxContainer/Portrait
@onready var character_label: Label = $VBoxContainer/HBoxContainer/NameLabel
@onready var dialogue_text: Label = $VBoxContainer/DialogueBox/Text
@onready var choices_container: VBoxContainer = $VBoxContainer/ChoicesContainer
@onready var choice_button_scene = preload("res://ui/choice_button.tscn")

var current_typewriter_tween: Tween

func start_typewriter(text: String, speed: float = 0.05) -> void:
  dialogue_text.text = ""
  current_typewriter_tween = create_tween()
  
  for char in text:
    if char == '<':  # Parse tags
      # Handle pause/speed tags
      continue
    
    dialogue_text.text += char
    current_typewriter_tween.tween_callback(func(): AudioManager.play_sfx("dialogue_char"))
    current_typewriter_tween.tween_interval(speed)

func show_choices(choices: Array[Choice]) -> void:
  # Clear previous choices
  for child in choices_container.get_children():
    child.queue_free()
  
  # Create choice buttons
  for choice in choices:
    var button = choice_button_scene.instantiate()
    button.text = choice.text
    button.pressed.connect(func(): DialogueManager.make_choice(choice))
    choices_container.add_child(button)

func hide_dialogue() -> void:
  var tween = create_tween()
  tween.tween_property(self, "modulate:a", 0.0, 0.3)
  tween.tween_callback(func(): visible = false)

func show_dialogue() -> void:
  visible = true
  var tween = create_tween()
  tween.tween_property(self, "modulate:a", 1.0, 0.3)
```

### Portrait System (Emotional States)

```
Portraits per NPC:
- van_idle.png (neutral)
- van_happy.png (peaceful, after good service)
- van_sad.png (remembering)
- van_angry.png (impatient)
- van_glowing.png (resolved, ready to leave)

linh_shy.png
linh_bonding.png
linh_sad.png
linh_peaceful.png

minh_angry.png
minh_vulnerable.png
minh_grateful.png

caong_idle.png (boss)
caong_charging.png
caong_hurt.png
```

---

## DIALOGUE FLOW & BRANCHING LOGIC

### Dialogue Tree Example (van.json)

```json
{
  "van_visit_1_start": {
    "id": "van_visit_1_start",
    "npc": "van",
    "character": "Ông Văn",
    "portrait": "res://assets/characters/van_angry.png",
    "text": "Này bạn! Sạp hàng — nó đã sẵn sàng chưa? Tôi đã chờ từ khi mặt trời mọc.",
    "type": "STATEMENT",
    "next": "van_visit_1_order",
    "effects": [
      {"type": "KINDNESS_GAIN", "value": 0}
    ]
  },
  
  "van_visit_1_order": {
    "id": "van_visit_1_order",
    "npc": "van",
    "character": "Ông Văn",
    "portrait": "res://assets/characters/van_angry.png",
    "text": "Trà. Trà đặc. (Nước trà đặc.) Và một que nhang. Chỉ một. Tôi không tham lam.",
    "type": "STATEMENT",
    "next": null,
    "effects": [
      {"type": "SPAWN_ORDER", "value": ["TEA", "INCENSE"]}
    ]
  },

  "van_visit_2_start": {
    "id": "van_visit_2_start",
    "npc": "van",
    "character": "Ông Văn",
    "portrait": "res://assets/characters/van_sad.png",
    "text": "Con trai lớn của tôi, nó đã đi Mỹ. Mười lăm năm, không thư.",
    "type": "STATEMENT",
    "next": "van_visit_2_choice",
    "effects": []
  },

  "van_visit_2_choice": {
    "id": "van_visit_2_choice",
    "npc": "van",
    "character": "Ông Văn",
    "portrait": "res://assets/characters/van_sad.png",
    "text": "Bạn hiểu không? Biển đã lấy đi cha của cậu.",
    "type": "QUESTION",
    "choices": [
      {
        "text": "Tôi hiểu...",
        "target": "van_visit_2_sympathetic",
        "kindnessValue": 1,
        "effect": {"type": "AURA_COLOR", "value": "orange"}
      },
      {
        "text": "[Hãy tiếp tục...]",
        "target": "van_visit_2_neutral",
        "kindnessValue": 0,
        "effect": {"type": "AURA_COLOR", "value": "gray"}
      }
    ]
  },

  "van_visit_2_sympathetic": {
    "id": "van_visit_2_sympathetic",
    "npc": "van",
    "character": "Ông Văn",
    "portrait": "res://assets/characters/van_sad.png",
    "text": "Cảm ơn bạn. Ít nhất ai đó còn nghe...",
    "type": "STATEMENT",
    "next": null,
    "effects": [
      {"type": "KINDNESS_GAIN", "value": 1},
      {"type": "AURA_COLOR", "value": "warm_orange"}
    ]
  },

  "van_visit_3_final": {
    "id": "van_visit_3_final",
    "npc": "van",
    "character": "Ông Văn",
    "portrait": "res://assets/characters/van_glowing.png",
    "text": "Trà này... nó vị như nhà. Cảm ơn bạn, Tú. Tôi nghĩ... tôi có thể nghỉ ngơi bây giờ.",
    "type": "STATEMENT",
    "next": null,
    "effects": [
      {"type": "KINDNESS_GAIN", "value": 2},
      {"type": "AURA_COLOR", "value": "soft_gold"},
      {"type": "SPAWN_EFFECT", "value": "glow_fade_out"}
    ]
  }
}
```

---

## INTEGRATION WITH GAME LOOP

### Day Phase Flow

```
[Spawn Timer] → [Ghost Appears] → [Order Spawned] → [Player Serves] 
                                        ↓
                                  [Dialogue Triggered]
                                        ↓
                                  [Player Makes Choice]
                                        ↓
                                  [Kindness Score ±1]
                                        ↓
                                  [NPC Departs or Stays]
```

### Code Integration (Main Game Loop)

```gdscript
class_name StallPhaseManager
extends Node

@export var max_concurrent_orders: int = 3
@export var ghost_spawn_interval: float = 8.0  # seconds

var orders: Array[Order] = []
var ghosts_present: Array[Ghost] = []
var kindness_score: int = 0

var spawn_timer: float = 0.0

func _process(delta: float) -> void:
  # Update spawn timer
  spawn_timer += delta
  if spawn_timer >= ghost_spawn_interval:
    spawn_timer = 0.0
    _spawn_ghost()
  
  # Update orders
  for order in orders:
    order._process(delta)
    
    # Visualize satisfaction
    _update_order_ui(order)
    
    if order.is_complete:
      orders.erase(order)

func _spawn_ghost() -> void:
  if ghosts_present.size() >= max_concurrent_orders:
    return
  
  var ghost = _get_random_ghost()
  if ghost == null or ghost.has_left:
    return
  
  ghost.current_state = "APPROACHING"
  ghosts_present.append(ghost)
  
  # Animate ghost entering from off-screen
  var tween = create_tween()
  tween.tween_property(ghost, "position", _get_stall_position(), 1.0)
  tween.tween_callback(func(): ghost.current_state = "WAITING")

func handle_player_click_item(item_type: Order.OrderType) -> void:
  # Find active order
  if orders.is_empty():
    return
  
  var current_order = orders[0]
  current_order.add_item(item_type)
  
  # Visual feedback
  AudioManager.play_sfx("item_placed")

func _get_random_ghost() -> Ghost:
  # Weighted random: prioritize ghosts that haven't visited 3x
  var available = ghosts.filter(func(g): return g.visit_count < 3 and not g.has_left)
  if available.is_empty():
    return null
  return available[randi() % available.size()]

func _update_order_ui(order: Order) -> void:
  # Show satisfaction meter
  var meter = _get_order_ui(order)
  meter.value = order.satisfaction
  
  # Change color if low
  if order.satisfaction < 30:
    meter.self_modulate = Color.RED
  else:
    meter.self_modulate = Color.WHITE
```

---

## TESTING & DEBUG CHECKLIST

### Dialogue System Tests

```
□ Dialogue text renders correctly (Vietnamese characters, no mojibake)
□ Typewriter effect plays at correct speed
□ Character portrait changes with emotional state
□ Choices display and respond to clicks
□ Kindness score increments correctly
□ NPC emotional state updates after choice
□ Dialogue branches correctly based on player choice
□ All 3 visits for each NPC trigger correct dialogue
□ Bad ending dialogue triggers if player is dismissive
□ Dialogue ends gracefully and returns to gameplay
```

### UI/UX Tests

```
□ Text is readable (font size ≥ 24px)
□ Choices have clear hover state
□ Dialogue box doesn't overlap game world
□ Portrait animations are smooth
□ Aura color transitions are visible
□ Text doesn't overflow dialogue box
□ Vietnamese diacritics render correctly
□ UI scales correctly at 1920x1080 and 1280x720
```

### Order & Queue Tests

```
□ Orders spawn at correct intervals
□ Max 3 concurrent orders enforced
□ Satisfaction decreases over time
□ Items can be clicked in correct order
□ Order completion triggers dialogue
□ Order timeout triggers "angry" state
□ Spirit Essence awarded correctly
□ Queue UI shows remaining items clearly
```

### Branching & Score Tests

```
□ Kindness score starts at 0
□ +1 for sympathetic choices
□ 0 for neutral choices
□ -1 for dismissive choices (if implemented)
□ Kindness score determines ending (≥6 = good, 2-5 = neutral, ≤1 = bad)
□ All three endings trigger correctly
□ Chú Ba appears only on bad ending path
□ Player can achieve all three endings in different playthroughs
```

### Localization Tests (Vietnamese)

```
□ All character names render in Vietnamese
□ All dialogue text displays without encoding issues
□ Diacritical marks (ă, ê, ơ, ư, â, etc.) are correct
□ Tonal marks (grave, acute, tilde, etc.) are correct
□ No text truncation in Vietnamese
□ Font supports Vietnamese Unicode
```

---

## PERFORMANCE CONSIDERATIONS

### Optimization Tips

1. **Pool Dialogue Nodes:** Reuse nodes instead of creating new ones
2. **Cache Dialogue Trees:** Load JSON once at startup, not per NPC
3. **Typewriter Threading:** Use coroutines instead of frame-by-frame character rendering
4. **Portrait Textures:** Use texture atlases (combine all portraits into one texture with UV mapping)
5. **Audio Channels:** Allocate 2 channels (dialogue SFX + ambient music don't conflict)

### Memory Usage (Target)

- Dialogue Trees JSON: ~500 KB
- Textures (portraits): ~2 MB (if compressed PNG)
- Audio (SFX): ~1 MB
- **Total dialogue system:** ~3.5 MB (acceptable)

---

## DEBUGGING TOOLS

### Console Commands (for testing)

```gdscript
# Skip to next dialogue node
DM.skip_to_node("van_visit_3_final")

# Set kindness score
GameManager.kindness_score = 8

# Trigger specific NPC
GhostManager.spawn_ghost_by_name("minh")

# Show all unlocked dialogue
for node_id in DialogueManager.dialogue_history:
  print(node_id)
```

---

## EXAMPLE: FULL DIALOGUE FLOW (VĂN, VISIT 1)

```
1. Ông Văn spawns at stall
   - State: APPROACHING
   - Animation: Walk from left side of screen
   - Duration: 1 second

2. Ông Văn reaches stall
   - State: WAITING
   - Portrait appears on left side
   - Dialogue UI shows

3. Dialogue starts
   - Text: "Này bạn! Sạp hàng — nó đã sẵn sàng chưa? Tôi đã chờ từ khi mặt trời mọc."
   - Typewriter effect: 20 chars/sec
   - Auto-advance after 2 seconds

4. Next dialogue line
   - Text: "Trà. Trà đặc. Và một que nhang."
   - Spawns Order: [TEA, INCENSE]
   - Order displayed in queue UI
   - Timer starts: 30 seconds

5. Player clicks TEA item
   - Audio: "item_placed" SFX
   - Order updates: [INCENSE] remaining

6. Player clicks INCENSE
   - Audio: "item_placed" SFX
   - Order updates: [] (complete)
   - Order satisfaction: depends on time taken

7. Dialogue continues (based on satisfaction)
   - High satisfaction (>70%): 
     "Hnh. Tốt hơn tôi mong đợi."
     Kindness +1
     Ông Văn departs
   
   - Low satisfaction (<30%):
     "Quá chậm! Người khác có lẽ cần không gian."
     Kindness 0
     Ông Văn departs angry

8. Ông Văn departs
   - State: DEPARTING
   - Animation: Fade out over 1 second
   - Dialogue UI closes
   - Spirit Essence reward: +1 (or +2 if high satisfaction)

9. UI updates
   - Kindness score shown: +1
   - Spirit Essence counter incremented
   - Next ghost can spawn
```

---

**End of Technical Design: Dialogue UI System**

*This document is complete and ready for implementation. Share with programmers for code review before starting Week 1.*

---

## APPENDIX: FILE STRUCTURE

```
res://
├── scenes/
│   ├── stall_phase.tscn
│   └── dialogue_ui.tscn
├── scripts/
│   ├── dialogue_manager.gd
│   ├── ghost.gd
│   ├── order.gd
│   └── stall_phase_manager.gd
├── data/
│   └── dialogues/
│       ├── van.json
│       ├── linh.json
│       ├── minh.json
│       └── caong.json
├── assets/
│   ├── characters/
│   │   ├── van_angry.png
│   │   ├── van_sad.png
│   │   └── ... (all portraits)
│   └── ui/
│       ├── choice_button.tscn
│       └── order_display.tscn
└── audio/
    └── sfx/
        ├── dialogue_char.wav
        ├── item_placed.wav
        └── order_complete.wav
```

---

*Version 1.0 — Ready for Production*
