# SOLAR PHOBIA: NẮNG GẮT
## AUDIO DESIGN DOCUMENT
### Complete Sound Design & Music Specification

**Version:** 1.0  
**Audio Director:** [Your team name]  
**Target:** Act 1 - Bãi Thuyền Shrine  
**Platforms:** Windows, WebGL  
**Audio Engine:** Godot Audio Bus + Spatial Audio (for 3D effect)  

---

## TABLE OF CONTENTS

1. [Audio Philosophy](#audio-philosophy)
2. [Music Design](#music-design)
3. [Sound Effects (SFX) Catalog](#sound-effects-sfx-catalog)
4. [Ambient Soundscapes](#ambient-soundscapes)
5. [Dialogue Audio Specifications](#dialogue-audio-specifications)
6. [Audio Implementation](#audio-implementation)
7. [Audio Testing Checklist](#audio-testing-checklist)

---

## AUDIO PHILOSOPHY

### Core Principle
**"The audio is half the story."**

Solar Phobia's audio should evoke **Vietnamese coastal folk horror** — mixing:
- Traditional Vietnamese instruments (preserved in musical themes)
- Natural ocean/beach ambience (grounding the supernatural)
- Subtle horror cues (builds dread without jump scares)
- Character-specific audio signatures (each NPC has a distinct "sound")

### Emotional Arc (Paralleled in Music)

| Time of Day | Mood | Music | SFX Profile |
|------------|------|-------|-------------|
| **Morning (Day Start)** | Confusion, wonder | Hopeful (minor key), light instrumentation | Seagulls, wind chimes, shrine bells |
| **Midday (High Sun)** | Pressure, anxiety | Dissonant strings, hollow sounds | Intense wind, omens (crows fleeing) |
| **Afternoon (Afternoon Decline)** | Compassion, connection | Warm, melancholic melodies | Waves, soft chimes, intimate moments |
| **Sunset (Transition)** | Dread, preparation | Rising tension, layered instruments | Wind howling, ancient drums (distant) |
| **Night (Boss Fight)** | Horror, survival | Intense, pulsing rhythm (like heartbeat) | Cá Ông whale calls, bone cracks, desperation |

---

## MUSIC DESIGN

### Track List

#### 1. **"Bãi Thuyền Sáng Sớm"** (Fishing Village Morning)
**Duration:** 3:00 (loop)  
**BPM:** 70  
**Key:** E minor (Vietnamese pentatonic scale)  
**Instrumentation:**
- **Primary:** Traditional Vietnamese đàn tranh (zither) — peaceful, contemplative
- **Secondary:** Soft wind flute (bamboo or ceramic)
- **Ambient:** Ocean waves (recorded on Central Vietnam coast), distant seagulls
- **Pulse:** Subtle unpitched percussion (temple blocks, soft triangle)

**Mood:** Melancholic beauty, slight unease (something is wrong with the sun)

**Dynamics:** 
- Intro (0:00-0:30): Solo zither, establishing theme
- Development (0:30-2:00): Add wind, waves become more prominent
- Peak (2:00-2:30): All instruments layer, questioning what's happening
- Outro (2:30-3:00): Fade back to sparse zither + waves

**Use Case:** Background during morning/noon management phase

**Production Notes:**
- Record live đàn tranh if possible (authentic > digital MIDI)
- Use actual Central Vietnam beach recordings for authenticity
- Key should reflect Vietnamese folk scales (pentatonic, emphasis on E-G-A-B-D)

---

#### 2. **"Mặt Trời Trống Đồng"** (The Hollow Sun)
**Duration:** 2:45 (loop)  
**BPM:** 85 (slightly faster = pressure increasing)  
**Key:** E minor (same as Track 1, but darker harmonization)  
**Instrumentation:**
- **Primary:** Electronic / synthesized hollow drum sound (mimics bronze drum)
- **Secondary:** Distorted strings (cello or erhu pulled/bowed unnaturally)
- **Ambient:** Wind intensifying, occasional dissonant tones
- **Percussion:** Low-frequency thuds (heartbeat-like, ~60 BPM)

**Mood:** Oppressive, unnatural, mounting dread

**Dynamics:**
- Intro (0:00-0:30): Single hollow drum sound, very sparse
- Build (0:30-1:30): Layer distorted strings, pulsing bass tone
- Peak (1:30-2:15): All elements at max, chaotic but rhythmic
- Outro (2:15-2:45): Fade to hollow drum alone again

**Use Case:** Background during high sun (midday), signals approaching pressure

**Production Notes:**
- Hollow drum sound: Use gong with reverb + ring-shift effect
- Distorted strings should sound **wrong** (uncanny valley)
- No melodic resolution — always feels unresolved/tense

---

#### 3. **"Linh Hồn Buồn"** (Spirits in Sorrow)
**Duration:** 3:30 (loop)  
**BPM:** 65 (slow, mournful)  
**Key:** A minor (relative minor to C major — introspective)  
**Instrumentation:**
- **Primary:** Erhu (Chinese bowed violin, very expressive — mimics wailing)
- **Secondary:** Soft cello (sustained notes, harmonic foundation)
- **Tertiary:** Bamboo flute (Em Linh's theme, airy and distant)
- **Ambient:** Faint temple bells, water droplets (Linh's signature)
- **Pulse:** None (or very subtle, sparse)

**Mood:** Tender, heartbreaking, compassionate

**Dynamics:**
- Intro (0:00-0:45): Solo erhu, very melancholic
- Build (0:45-2:00): Add cello underneath, bamboo flute enters
- Peak (2:00-2:30): All three instruments in harmony, most moving moment
- Outro (2:30-3:30): Gradual fade, back to sparse erhu + bells

**Use Case:** Background during Linh's dialogue, afternoon bonding moments

**Production Notes:**
- Erhu should sound human (wailing voice-like) — not mechanical
- Bamboo flute should sound lightweight, innocent
- No percussion — reinforces stillness, sadness

---

#### 4. **"Đợi Chờ"** (Waiting / Longing)
**Duration:** 2:50 (loop)  
**BPM:** 60 (very slow)  
**Key:** G minor (darker, more hopeless than A minor)  
**Instrumentation:**
- **Primary:** Traditional Vietnamese đàn bầu (monochord — single string instrument with very long sustain)
- **Secondary:** Cello (long, aching notes)
- **Ambient:** Distant rain, distant thunder, clock ticking (metaphorical)
- **Texture:** Granular, barely-there ambient pads

**Mood:** Longing, despair, romantic tragedy (Anh Minh's theme)

**Dynamics:**
- Intro (0:00-0:40): Solo đàn bầu, one long sustained note
- Build (0:40-1:50): Add cello, subtle shift in harmony (hope fade)
- Peak (1:50-2:15): Layered đàn bầu voices, atmospheric
- Outro (2:15-2:50): Everything fades except clock ticking

**Use Case:** Background during Minh's dialogue, moments of vulnerability

**Production Notes:**
- Đàn bầu is an extremely authentic Vietnamese instrument — use it (or sampling)
- Cello should sound anguished, not pretty
- Clock ticking = reminder of time passing, years of waiting

---

#### 5. **"Chiến Tranh"** (The Escape / Danger Theme)
**Duration:** 2:00 (loop, but plays during boss fight)  
**BPM:** 140 (double-time, urgent)  
**Key:** E minor (back to minor, but now with urgency)  
**Instrumentation:**
- **Primary:** Pulsing heartbeat (synthetic or recorded drum)
- **Secondary:** Layered strings (chaotic, not harmonious)
- **Tertiary:** Low brass or synth bass (Cá Ông's presence)
- **Ambient:** Wind howling, whale-like moans (Cá Ông's cry)
- **Percussion:** Urgent, irregular rhythm

**Mood:** Panic, danger, race against time, supernatural dread

**Dynamics:**
- Intro (0:00-0:15): Just heartbeat + one whale moan
- Build (0:15-0:50): Add chaotic strings, bass rumbles
- Peak (0:50-1:40): All instruments at full energy, driving forward
- Outro (1:40-2:00): Brief moment of calm (reaching shrine) before climax

**Use Case:** Expeditionary phase (beach run), boss fight against Cá Ông

**Production Notes:**
- Heartbeat should sync with boss attack rhythm (~1 beat per second)
- Whale moans should sound otherworldly (pitch-shift, reverb)
- Build should feel inexorable (no resolution until player reaches shrine)

---

#### 6. **"Hoàng Hôn Chuyển Tiếp"** (Sunset Transition / Good Ending Theme)
**Duration:** 3:15 (plays during Epilogue if good ending)  
**BPM:** 75 (gradual slow-down)  
**Key:** G major (major key = hope!)  
**Instrumentation:**
- **Primary:** Đàn tranh + erhu in harmony (Vietnamese + Chinese instruments together)
- **Secondary:** Soft woodwinds (flute, oboe)
- **Tertiary:** Cello (grounding)
- **Ambient:** Waves, shrine bells, distant chanting (monks)
- **Pulse:** Subtle tam tam (tam-tam gong with light mallet, spiritual effect)

**Mood:** Bittersweet peace, resolution, closure, spiritual completion

**Dynamics:**
- Intro (0:00-0:40): Solo đàn tranh, establishing hope
- Build (0:40-2:00): Add erhu, woodwinds, all in major harmony
- Peak (2:00-2:45): All instruments together in triumphant minor-to-major resolution
- Outro (2:45-3:15): Very slow fade, monks chanting "Aum" (OM) underneath

**Use Case:** Good ending epilogue, credits (if good ending achieved)

**Production Notes:**
- Major key is a stark contrast to the game's minor-key aesthetic
- Monk chanting adds spiritual authenticity
- Gradually slowing tempo reinforces "letting go" theme

---

#### 7. **"Chú Ba's Theme"** (Bad Ending / Corruption Theme)
**Duration:** 2:20 (plays if bad ending triggered)  
**BPM:** 90 (faster than normal, but not as frantic as boss theme)  
**Key:** E minor (same as normal, but heavily distorted)  
**Instrumentation:**
- **Primary:** Heavily distorted/glitched erhu (sounds like a ghost screaming)
- **Secondary:** Synthesized "wrong" tones (gamelan-like but algorithmically generated, not harmonic)
- **Tertiary:** Low-frequency rumble (subwoofer territory, ~20-40 Hz)
- **Ambient:** Reversed recordings, whispers, ominous silence
- **Percussion:** Irregular, unpredictable hits (no sense of rhythm)

**Mood:** Corruption, vengeance, horror, inevitable doom

**Dynamics:**
- Intro (0:00-0:25): Silence + one distorted scream
- Build (0:25-1:20): Layer glitched tones, approaching chaos
- Peak (1:20-2:00): All elements at maximum, truly unsettling
- Outro (2:00-2:20): Everything fades except low rumble

**Use Case:** Bad ending epilogue, Chú Ba boss fight variant

**Production Notes:**
- Distortion should be **extreme** — this is a corrupted spirit
- No traditional harmony — everything should sound "off"
- Subwoofer rumble is crucial (adds visceral dread even below hearing range)

---

## SOUND EFFECTS (SFX) CATALOG

### Environmental SFX

#### Ambient Loops (Layer beneath all scenes)

| SFX Name | Description | Duration | Volume | Notes |
|----------|-------------|----------|--------|-------|
| **ocean_waves_calm.wav** | Gentle waves lapping, ~1 min loop | 1:00 | -12 dB | Morning/afternoon |
| **ocean_waves_intense.wav** | Rough surf, approaching storm feeling | 1:15 | -8 dB | Midday/sunset |
| **wind_light.wav** | Gentle breeze through shrine | 0:45 | -18 dB | Background layer |
| **wind_strong.wav** | Howling wind, building pressure | 1:00 | -10 dB | High sun hours |
| **seagull_calls.wav** | Single gull cry, very occasional | 0:30 | -14 dB | Daytime only; stops at sunset |
| **temple_bells_distant.wav** | Faint bell toll, ~30 sec intervals | Variable | -20 dB | Always present, very subtle |
| **rain_light.wav** | Sparse raindrops (forecast of danger) | 1:20 | -15 dB | Late afternoon/sunset |
| **night_insects.wav** | Cicadas, frogs, after sunset | 1:45 | -12 dB | Night phase only |

---

### Interaction SFX

#### Player Actions

| SFX Name | Trigger | Duration | Volume | Priority |
|----------|---------|----------|--------|----------|
| **item_placed.wav** | Player clicks item for order | 0:35 | -6 dB | HIGH |
| **item_drag.wav** | Player drags item toward stall | 0:25 | -8 dB | MED |
| **order_complete.wav** | Order fully fulfilled | 0:60 | 0 dB | VERY HIGH |
| **npc_satisfied.wav** | NPC accepts order (happy) | 0:45 | -4 dB | HIGH |
| **npc_unsatisfied.wav** | NPC rejects order (angry) | 0:55 | -3 dB | VERY HIGH |
| **spirit_essence_gain.wav** | Reward currency picked up | 0:40 | -2 dB | HIGH |
| **button_hover.wav** | UI button hover (dialogue choice) | 0:15 | -12 dB | LOW |
| **button_press.wav** | UI button clicked | 0:20 | -10 dB | MED |

---

#### NPC/Ghost SFX

| SFX Name | Associated NPC | Description | Duration | Notes |
|----------|---|---|---|---|
| **van_footsteps.wav** | Ông Văn | Heavy, limping footsteps (older man) | 0:60 | Loop, plays during approach |
| **van_breath.wav** | Ông Văn | Slow, tired breathing | 0:30 | Plays when he sits |
| **linh_water_drip.wav** | Em Linh | Single water droplet, high-pitched | 0:12 | Loops continuously (~0.5 sec interval) |
| **linh_whisper.wav** | Em Linh | Distant, whispered "hello" | 0:30 | On first arrival |
| **minh_dress_rustle.wav** | Anh Minh | Cloth movement, wedding áo dài | 0:40 | Plays during walk |
| **minh_sigh.wav** | Anh Minh | Deep, anguished sigh | 0:25 | On specific dialogue lines |
| **caong_whale_call.wav** | Cá Ông | Deep, reverberant whale moan | 1:50 | Loop during boss fight |
| **caong_bone_crack.wav** | Cá Ông | Bone breaking/grinding sound | 0:35 | On boss attack |
| **caong_water_splash.wav** | Cá Ông | Heavy water displacement | 0:55 | Boss moves through water |

---

#### Emotional/Narrative SFX

| SFX Name | Trigger | Description | Duration | Volume |
|----------|---------|---|---|---|
| **aura_shift.wav** | NPC emotional change | Shimmering, ethereal tone | 0:40 | -8 dB |
| **dialogue_char.wav** | Each text character (typewriter) | Soft "ding" or tap | 0:10 | -18 dB |
| **ghost_materialize.wav** | NPC arrives | Swelling, appearing tone | 0:50 | -6 dB |
| **ghost_despair.wav** | NPC becomes unstable (bad service) | Dissonant, warped tone | 0:60 | -4 dB |
| **ghost_peace.wav** | NPC resolves (good service) | Harmonic chord, glowing | 0:75 | -2 dB |
| **ghost_depart.wav** | NPC leaves peacefully | Fading, echoing tone | 1:00 | -10 dB |
| **ritual_chime.wav** | Incense lit, ritual performed | Temple bell tone | 0:35 | 0 dB |
| **sun_omen.wav** | Hollow sun worsens | Disturbing, dissonant tone | 0:80 | -6 dB |

---

### Boss Fight SFX

| SFX Name | Event | Description | Duration | Volume |
|----------|-------|---|---|---|
| **caong_appear.wav** | Boss emerges from water | Explosive whale breach | 1:20 | +3 dB |
| **caong_roar.wav** | Boss attacks | Deep, layered roar | 0:90 | 0 dB |
| **caong_slam.wav** | Boss hits ground/player | Impact with reverb | 0:65 | -2 dB |
| **player_hit.wav** | Player takes damage | Pain grunt, impact | 0:30 | -4 dB |
| **player_run.wav** | Player sprinting | Footsteps, breathing | Variable | -6 dB |
| **shrine_barrier.wav** | Reaching safe shrine | Bright, protective tone | 1:00 | -1 dB |

---

## AMBIENT SOUNDSCAPES

### Soundscape Layering (Mixing Guide)

**Target Mix Levels** (during standard gameplay):

```
Layer 1: Ambient Music Track        | -8 dB (primary soundtrack)
Layer 2: Environmental Loop         | -12 dB (waves, wind, general ambience)
Layer 3: Ghost-Specific Ambient     | -14 dB (water drips for Linh, cloth for Minh, etc.)
Layer 4: UI/SFX Events             | -4 dB to 0 dB (comes to foreground when player acts)
Layer 5: Dialogue                   | +2 dB (loudest, clear voice)
```

### Phase-Specific Soundscapes

#### **Morning (6 AM - 10 AM): Hope & Discovery**
```
Background: "Bãi Thuyền Sáng Sớm" track
Ambient: Gentle waves, seagulls calling, temple bells distant
Mood: Peaceful but slightly eerie
Volume: Generally quiet (-10 to -15 dB ambient)
```

#### **High Sun (11 AM - 3 PM): Pressure & Dread**
```
Background: "Mặt Trời Trống Đồng" track
Ambient: Intense wind, occasional ominous sounds (crows fleeing is optional)
Mood: Oppressive, anxious
Volume: Louder (-6 to -10 dB ambient), feels suffocating
```

#### **Afternoon (3 PM - 5 PM): Compassion & Connection**
```
Background: "Linh Hồn Buồn" or "Đợi Chờ" track (depending on NPC)
Ambient: Softer waves, rain beginning, intimate
Mood: Vulnerable, touching
Volume: Softer (-12 to -18 dB ambient), introspective
```

#### **Sunset (5 PM - 6:30 PM): Transition & Danger**
```
Background: Hybrid of "Mặt Trời Trống Đồng" + "Chiến Tranh" (crossfade)
Ambient: Wind howling, distant thunder, barely-audible whale moans
Mood: Building dread, urgency
Volume: Rising (from -10 dB to -4 dB as sunset progresses)
```

#### **Night Phase (6:30 PM - after): Horror & Survival**
```
Background: "Chiến Tranh" track (intense, looped)
Ambient: Howling wind, ocean fury, Cá Ông whale calls, bone cracks
Mood: Horror, desperation, supernatural
Volume: VERY LOUD (0 dB to +3 dB), immersive
```

---

## DIALOGUE AUDIO SPECIFICATIONS

### Voice Acting Direction (For Future Production)

**Note:** For Game Jam, use **text-only dialogue** (no voice). Below is specification for **post-jam voice acting**.

#### Ông Văn (Fisherman)
**Voice Profile:** Male, 60s-70s, weathered, tired
**Accent:** Central Vietnam coast (Quảng Ngãi / Bình Định dialect)
**Delivery Style:**
- Slow, deliberate pacing
- Rough tone, but kind underneath
- Slight rasp in voice (age)
- Often pauses to remember things
- Fishing metaphors interspersed

**Example Delivery:**
```
[PAUSE 0.5 sec]
"Này... bạn. Sạp hàng... [PAUSE] nó đã sẵn sàng chưa?"
[Slower pacing throughout, tired voice]
```

**Recording Notes:**
- Record in quiet room (minimize AC/background noise)
- Use warm, slightly compressed microphone (mimics older voice)
- Add subtle reverb in post-production (shrine environment)

---

#### Em Linh (Drowned Girl)
**Voice Profile:** Female, 12-14, soft, shy, ethereal
**Accent:** Central Vietnam (but slightly distorted, as if underwater)
**Delivery Style:**
- Whispered, very quiet
- Hesitant, apologetic
- Sounds distant (like echoing from underwater)
- Often trails off mid-sentence
- Occasional sadness in voice

**Example Delivery:**
```
[WHISPERED]
"Xin lỗi... xin lỗi... cô có trà... không?"
[Voice should sound far away, like from another world]
```

**Recording Notes:**
- Record very quiet (add volume in post)
- Add heavy reverb + delay (underwater effect)
- Pitch-shift down slightly (-0.5 semitones) for ethereal quality
- Consider pitch variations (slight undulation, like water movement)

---

#### Anh Minh (Jilted Bride)
**Voice Profile:** Female, 30s, elegant, articulate, bitter
**Accent:** North Vietnam (Hanoi/Huế educated accent, refined)
**Delivery Style:**
- Clear, precise articulation
- Sharp, cutting tone (sarcasm)
- Dramatic pauses for effect
- Anguished when vulnerable
- Poetic, references classic Vietnamese literature

**Example Delivery:**
```
[SHARP]
"Tại sao bạn lại quan tâm?"
[PAUSE 1.0 sec, vulnerability creeping in]
"Hùng... anh ấy đã chạy trốn."
```

**Recording Notes:**
- Record with clear diction (educated woman)
- Add subtle tremolo/vibrato when emotional
- Slight reverb (shrine, but more present than Linh)
- No pitch-shifting (pure voice)

---

#### Cá Ông (Whale Lord)
**Voice Profile:** Non-human, deep, reverberant, otherworldly
**Description:** This is NOT a human voice — synthesized or heavily processed whale sounds + distorted human voice
**Delivery Style:**
- Very deep (subwoofer range: 20-60 Hz)
- Reverberant, echoing (like underwater caves)
- Monosyllabic, primal
- Mournful, not aggressive

**Example Delivery:**
```
[DEEP, REVERBERANT, WHALE-LIKE]
"WHYYYYYYY DO YOU LEAVEEEE?"
[Harmonically unstable, multiple voices layered]
```

**Recording Notes:**
- Use whale call recordings as base
- Layer distorted human voice underneath
- Heavy pitch-shifting (down 2-3 octaves)
- Add reverb + spatial effects (sounds like it's surrounding you)
- Test on subwoofer to ensure low frequencies are present

---

### Text-Only Dialogue (Game Jam Implementation)

For the jam, dialogue will be **text-only** to avoid AI voice issues and save time.

**Text Formatting for Emotional Cues:**
```
Regular dialogue: Normal font, white text
<emphasis>Important words</emphasis>: Larger font, different color
<whisper>Whispered lines</whisper>: Gray text, italicized
<anguished>Emotional peaks</anguished>: Red/orange tint, quivering animation
<peaceful>Resolved lines</peaceful>: Gold glow, gentle animation
```

---

## AUDIO IMPLEMENTATION

### Godot Audio Architecture

```
AudioServer (Godot Built-in)
├── Master Bus
│   ├── Music Bus (-8 dB)
│   │   ├── Track 1 (Bãi Thuyền Sáng Sớm)
│   │   ├── Track 2 (Mặt Trời Trống Đồng)
│   │   └── [other tracks, crossfading]
│   ├── Ambient Bus (-12 dB)
│   │   ├── Waves
│   │   ├── Wind
│   │   └── Seagulls
│   ├── SFX Bus (-4 dB)
│   │   ├── Item Sounds
│   │   ├── NPC Sounds
│   │   └── Event Sounds
│   └── Dialogue Bus (+2 dB)
│       └── Text-to-speech or voice lines (post-jam)
```

### Code Implementation (Godot GDScript)

```gdscript
class_name AudioManager
extends Node

# Audio bus assignments
@onready var master_bus = AudioServer.get_bus_index("Master")
@onready var music_bus = AudioServer.get_bus_index("Music")
@onready var ambient_bus = AudioServer.get_bus_index("Ambient")
@onready var sfx_bus = AudioServer.get_bus_index("SFX")

var current_music_track: AudioStreamPlayer
var ambient_players: Array[AudioStreamPlayer] = []

# Audio tracks (preloaded)
var music_tracks = {
  "morning": preload("res://audio/music/bai_thuyen_sang_som.ogg"),
  "hollow_sun": preload("res://audio/music/mat_troi_trong_dong.ogg"),
  "spirits_sorrow": preload("res://audio/music/linh_hon_buon.ogg"),
  "waiting": preload("res://audio/music/doi_cho.ogg"),
  "danger": preload("res://audio/music/chien_tranh.ogg"),
  "sunset": preload("res://audio/music/hoang_hon_chuyen_tiep.ogg"),
  "corruption": preload("res://audio/music/chu_ba_theme.ogg"),
}

# SFX library
var sfx_library = {
  "item_placed": preload("res://audio/sfx/item_placed.wav"),
  "order_complete": preload("res://audio/sfx/order_complete.wav"),
  "ghost_appear": preload("res://audio/sfx/ghost_materialize.wav"),
  "ghost_peace": preload("res://audio/sfx/ghost_peace.wav"),
  "caong_roar": preload("res://audio/sfx/caong_roar.wav"),
  # ... more SFX
}

func _ready() -> void:
  # Setup audio buses
  AudioServer.set_bus_mute(music_bus, false)
  AudioServer.set_bus_volume_db(music_bus, -8.0)
  AudioServer.set_bus_volume_db(ambient_bus, -12.0)
  AudioServer.set_bus_volume_db(sfx_bus, -4.0)

func play_music(track_name: String, fade_duration: float = 2.0) -> void:
  if current_music_track:
    # Fade out current track
    var tween = create_tween()
    tween.tween_property(current_music_track, "volume_db", -80.0, fade_duration)
    tween.tween_callback(func(): current_music_track.stop())
  
  # Play new track
  current_music_track = AudioStreamPlayer.new()
  current_music_track.stream = music_tracks[track_name]
  current_music_track.bus = AudioServer.get_bus_name(music_bus)
  current_music_track.volume_db = -80.0  # Start silent for fade-in
  add_child(current_music_track)
  current_music_track.play()
  
  # Fade in new track
  var tween = create_tween()
  tween.tween_property(current_music_track, "volume_db", -8.0, fade_duration)

func play_sfx(sfx_name: String, volume_db: float = 0.0) -> void:
  var sfx_player = AudioStreamPlayer.new()
  sfx_player.stream = sfx_library[sfx_name]
  sfx_player.bus = AudioServer.get_bus_name(sfx_bus)
  sfx_player.volume_db = volume_db
  add_child(sfx_player)
  sfx_player.play()
  
  # Auto-remove when finished
  await sfx_player.finished
  sfx_player.queue_free()

func update_phase(phase: String) -> void:
  # Change music based on time of day
  match phase:
    "MORNING":
      play_music("morning")
    "HIGH_SUN":
      play_music("hollow_sun")
    "AFTERNOON":
      play_music("spirits_sorrow")
    "SUNSET":
      play_music("danger")
    "NIGHT":
      play_music("danger")  # Stays same during boss

func trigger_dialogue_start(npc_name: String) -> void:
  # Play NPC-specific entrance SFX
  match npc_name:
    "van":
      play_sfx("van_footsteps")
    "linh":
      play_sfx("linh_water_drip")
    "minh":
      play_sfx("minh_dress_rustle")
```

---

## AUDIO TESTING CHECKLIST

### Mix Testing

```
□ Music audible but not overpowering
□ Ambient layers create immersive environment
□ SFX cut through (clear feedback to player)
□ Dialogue would be clearly audible (if added post-jam)
□ No frequency conflicts (e.g., bass rumble conflicts with whale calls)
□ Volume levels consistent across all tracks
□ Crossfades between music tracks are smooth
```

### Spatial Audio Testing

```
□ Stereo field is balanced (no hard panning)
□ Reverb creates sense of shrine space
□ NPC footsteps sound like they're moving (panning + volume fade)
□ Boss sounds like it's coming from water (spatial processing)
□ Player feels "surrounded" by environment during night phase
```

### Cultural Authenticity Testing

```
□ Vietnamese instruments are recognizable
□ No stereotypical "Asian gong" clichés
□ Pentatonic scales feel natural, not forced
□ Dialect/accent (post-jam) is respectful, not mocking
□ Folk elements are integrated, not exoticized
```

### Technical Testing

```
□ Audio plays on Windows / WebGL
□ No latency between action and SFX
□ No audio popping/clicking on bus transitions
□ Looping audio is seamless (no silence gaps)
□ Audio compresses well for web (< 50 MB total)
□ No crashes from audio buffer overflow
```

### Emotional Response Testing

```
□ Player feels uneasy during high sun (music works)
□ Player feels compassion during NPC dialogue (music works)
□ Player feels panic during boss fight (music works)
□ Player feels peace during good ending (music works)
□ Player feels dread during bad ending (music works)
```

---

## AUDIO ASSETS BUDGET

### File Sizes (Target)

```
Music Tracks (7 tracks × 3-3.5 MB)    | ~21 MB
Ambient Loops (8 loops × 1-2 MB)     | ~12 MB
SFX Library (40+ SFX × 0.2-1 MB)     | ~20 MB
Voice (if recorded)                   | ~5 MB
─────────────────────────────────────
TOTAL                                 | ~58 MB
```

### Compression Targets

```
Format: OGG Vorbis (better compression than WAV for web)
Music: 128 kbps VBR
Ambient: 96 kbps VBR
SFX: 64-128 kbps VBR
Voice: 96-128 kbps VBR
```

---

## SOUND DESIGNER NOTES

### Recommended Tools

```
DAW: Reaper (flexible, affordable)
Instruments:
  - Native Instruments Komplete (Vietnamese instruments samples)
  - Spitfire Audio (orchestral for erhu, cello, flute)
  - Kontakt Libraries (traditional instruments)
Field Recording:
  - Zoom H6 or equivalent (beach recordings in Vietnam)
  - Rode NT1 (quiet, professional)
Post-Production:
  - iZotope RX (vocal cleanup, if voice recording)
  - FabFilter Pro (EQ, reverb, mixing)
  - ffmpeg (batch audio conversion)
```

### Recording Locations (If Field Recording)

For maximum authenticity, record ambience in:
- **Bãi Biển Quảng Ngãi** (Central Vietnam coast where game is set)
- Sunrise/sunset for best natural ambience
- Include: waves, wind, seagulls, temple bells (if nearby shrine)

### Timeline for Audio Production

```
Week 1: Compose all 7 music tracks + arrange
Week 2: Record/synthesize SFX library
Week 3: Mix all tracks to final levels
Week 4: Polish, compress, integrate into game + testing
```

---

## APPENDIX: MUSIC COMPOSITION GUIDELINES

### Vietnamese Pentatonic Scale Primer

**Traditional Vietnamese scale (no leading tone):**
```
C D E  G A (C)
1 2 3  5 6 (8)

Notable: No F natural, no B natural
Flavor: Pentatonic minor feeling, versatile for melancholy
```

### Erhu (Chinese Bowed Violin) Techniques

```
Standard bow: Smooth, singing quality
Glissando: Sliding between notes (very expressive)
Tremolo: Fast repeated note (creates tension)
Pizzicato: Plucked (rare, for punctuation)
Overtones: High harmonics (ethereal, ghostly)
```

### Đàn Tranh (Vietnamese Zither) Techniques

```
Standard pluck: Clear, warm tone
Slide: Smooth transition between notes
Vibrato: Slight oscillation (adds emotion)
Harmonics: Gentle shimmer
Bent notes: Bending pitch up/down (folk flavor)
```

---

**End of Audio Design Document**

*Share with sound designer/composer for implementation.*

*Version 1.0 — Ready for Production*
