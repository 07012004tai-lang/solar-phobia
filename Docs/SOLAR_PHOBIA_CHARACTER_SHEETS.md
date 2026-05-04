# SOLAR PHOBIA: NẮNG GẮT
## CHARACTER SHEETS & VISUAL REFERENCE
### Complete Character Design Document

**Version:** 1.0  
**Last Updated:** Game Jam Week 1  
**Target Audience:** 2D Artists, Game Designers, Programmers  
**Format:** Markdown + Design Reference  

---

## TABLE OF CONTENTS

1. [Character Overview](#character-overview)
2. [Ông Văn - The Fisherman](#ông-văn---the-fisherman)
3. [Em Linh - The Drowned Girl](#em-linh---the-drowned-girl)
4. [Anh Minh - The Jilted Bride](#anh-minh---the-jilted-bride)
5. [Cá Ông Bộ Xương - Whale Lord (Boss)](#cá-ông-bộ-xương---whale-lord-boss)
6. [Visual Style Guide](#visual-style-guide)
7. [Animation Requirements](#animation-requirements)
8. [Color Specifications](#color-specifications)

---

## CHARACTER OVERVIEW

| Character | Role | Age | Personality | Primary Color | Key Trait |
|-----------|------|-----|-------------|--------------|-----------|
| **Ông Văn** | Recurring NPC | 60s-70s | Gruff, Nostalgic | Brown #8B7355 | Weathered Fisherman |
| **Em Linh** | Emotional Anchor | 12-14 | Shy, Apologetic | Blue-Gray #4A7BA7 | Water Trails |
| **Anh Minh** | Complex NPC | 30s | Bitter, Intelligent | Gold #D4AF37 | Flickering Form |
| **Cá Ông** | Boss Encounter | Mythical | Desperate, Territorial | Bone White #F5F5F5 | Bioluminescence |

---

## ÔNG VĂN - THE FISHERMAN

### Character Identity

**Full Name:** Ông Văn (Uncle Văn)  
**Role:** First customer, recurring NPC, story guide  
**Age (in death):** 60s-70s  
**Death date:** ~5 years before game start  
**Status:** Peaceful but melancholic  

### Visual Design Specifications

#### Face & Appearance
- **Weathered face** — deep lines, sun-damaged skin
- **Burn marks** on forearms (fishing accident scars)
- **Missing two fingers** on left hand (old fishing injury — visual shorthand)
- **Eyes:** Tired but kind, often squinted (from decades at sea)
- **Hair:** Gray-white, slightly unkempt, shoulder-length

#### Clothing
- **Primary:** Faded blue fishing shirt (áo dài style, weathered)
- **Secondary:** Worn gray pants, bare feet (ghostly form)
- **Accessories:** Small fishing net draped over shoulder (optional, for silhouette clarity)

#### Ghostly Manifestation
- **Translucency:** ~70% opaque (more solid than other ghosts, grounded presence)
- **Aura:** Warm orange glow around edges when peaceful; dims when distressed
- **Movement:** Slow, deliberate, limping (old war injury + fishing injuries)
- **Special effect:** Faint mist trails footsteps (water-like, representing sea)

### Color Palette

| Purpose | Color | Hex | Notes |
|---------|-------|-----|-------|
| Primary (Shirt/Silhouette) | Weathered Brown | #8B7355 | Warm, earthy, aged |
| Secondary (Pants) | Faded Gray-Brown | #A0957A | Desaturated, weathered |
| Accent (Skin/Face) | Tan | #D4A574 | Sun-worn, age-appropriate |
| Aura (Peaceful) | Warm Gold | #E8A040 | Memory, comfort, human warmth |
| Aura (Distressed) | Dim Orange | #B8744F | Muted, troubled state |

### Personality & Voice

**Speech Pattern:**
- Regional dialect (Central Vietnam coast — Quảng Ngãi region)
- Rough, colloquial Vietnamese with fishing terminology
- Uses metaphors about the sea, nets, currents
- Speaks slowly, with pauses (contemplative)

**Key Phrases:**
- "Mạnh mẽ, mạnh mẽ!" (Strong, strong!) — about tea
- "Biển không tàn nhẫn. Nó chỉ quên cách lắng nghe." (The sea isn't cruel. It just forgot how to listen.)
- "Con trai tớ... anh ấy đã chạy sang bên kia biển." (My son... he ran to the opposite shore.)

**Emotional Range:**
- Gruff → Nostalgic → Vulnerable → Peaceful

### Story Arc & Dialogue

#### Visit 1: "The Impatient Customer" (Daytime)
**Context:** First interaction with player. Establishes stall mechanic and NPC purpose.

**Arrival:**
```
[Ông Văn emerges from the beach mist, limping]
"You there! The stall—is it ready? I have been waiting since the sun rose. 
My throat is sand."
```

**Order:**
```
"Tea. Strong tea. (Nước trà đặc.) And one incense stick. Just one. 
I am not greedy."
```

**If served quickly:**
```
[Sits, sips tea]
"Hnh. Better than I expected. 
My wife used to make it this way... before everything changed."
[Nods slowly, +1 Spirit Essence]
```

**If served too slowly:**
```
[Irritable]
"How long does it take to pour tea? A child could do this faster!"
[Leaves unsatisfied, -Spirit Essence]
```

#### Visit 2: "The Story Emerges" (Mid-Act)
**Context:** Ông Văn returns to the stall. Player can choose to listen or remain professional.

**Arrival:**
```
[Sits quietly, stares at the tea]
"You know... my oldest son. He went to America. Fifteen years. 
No letters. No phone calls."
```

**If player shows sympathy:**
```
"The sea took his father. The war took his home. The boy... 
he ran to the opposite shore. Smart. Cowardly, but smart."

[Long pause]

"If you ever meet him in the next life, tell him—
the sea doesn't hate him. He just forgot how to listen."
```

**Player Choice Impact:**
- ✅ Show compassion → Unlock Visit 3
- ❌ Stay professional → Visit 3 still happens, but dialogue is shorter

**Outcome:** +2 Spirit Essence, deeper connection unlocked

#### Visit 3: "The Letting Go" (Sunset/End of Act)
**Context:** Final visit. Ông Văn is ready to find peace.

**Arrival:**
```
[Approaches slowly, holds tea cup with both hands]
[Long silence]

"This tea... it tastes like home. Before the war. Before the storms. 
Before everything was taken."
```

**Core Moment:**
```
[Eyes closed, voice calm]
"I think... I can rest now. 
You listened to us, Tú. That's more than most people ever did."

[His form begins to glow softly, becoming more translucent]
[Quiet, peaceful smile]

"Thank you."
```

**Outcome:** +3 Spirit Essence, Ông Văn's arc complete. He becomes a memory (no longer appears as customer, but is referenced by other NPCs).

### Animation Requirements

**Idle State:**
- 2 frames: Standing, slight sway (bobbing in place, breathing)
- Loop: 1.5 seconds per cycle

**Walk/Approach:**
- 4 frames: Limping gait (left leg slightly shorter stride)
- Loop: 1 second per cycle
- Direction: Left-to-right approach to stall

**Sit Animation:**
- 2 frames: Settling into seat, then relaxed sitting
- Loop: Static once seated

**Drink Animation:**
- 3 frames: Lifting cup, sipping, setting down
- Duration: 2 seconds total

**Emotional Shifts:**
- **Peaceful:** Brightness +20%, aura color shifts to warm gold
- **Distressed:** Translucency decreases (flickers), aura dims to orange-brown
- **Leaving:** Fade-out animation (dissolve over 1.5 seconds)

### Visual Reference Notes for Artists

**Silhouette Priority:** Ông Văn should be **instantly recognizable by outline alone**
- Hunched posture (spine curves forward)
- Slightly asymmetrical stance (left leg shorter)
- Missing fingers visible when hand is extended

**Lighting Context:**
- **Day phase:** Desaturated, warm tones; brown becomes muted tan
- **Transition:** Colors become richer as sun sets
- **Aura effect:** Warm glow increases as he finds peace

**Comparison References:**
- **Age/build:** Similar to weathered fishermen in films like *The Quiet Family* or *Everything Everywhere All at Once*
- **Color palette:** Earthy tones from Vietnamese coastal cinematography (muted browns, tans, weathered blues)
- **Movement style:** Slow, deliberate, with the weight of decades

---

## EM LINH - THE DROWNED GIRL

### Character Identity

**Full Name:** Em Linh (Little Linh)  
**Role:** Emotional anchor, secondary NPC, melancholy thread  
**Age (appearance):** 12-14 years old  
**Death date:** ~2 years before game start  
**Status:** Lost, lonely, seeking connection  

### Visual Design Specifications

#### Face & Appearance
- **Young face:** Smooth skin, gentle features, age-appropriate
- **Expression:** Serene sadness, often downcast eyes
- **Hair:** Long, black, perpetually wet and dripping
- **Eyes:** Large, gentle, with a distant quality (as if looking through you to the ocean)
- **Skin tone:** Pale, blue-gray pallor (drowned appearance)

#### Clothing
- **Primary:** Áo dài (traditional Vietnamese dress) in pale white/cream
- **Condition:** Torn at hem, edges ragged as if eaten by tide
- **Sleeve:** One sleeve partially transparent (water damage)
- **Feet:** Bare, pale

#### Ghostly Manifestation
- **Translucency:** 60-70% opaque (more ethereal than Ông Văn)
- **Water effect:** Continuous water droplets trail from hair and dress; droplets evaporate in ~1 second
- **Aura:** Soft blue shimmer (like underwater light filtering from surface)
- **Movement:** Gliding, smooth (no footsteps, just gentle motion)
- **Special effect:** Occasionally, her form becomes slightly more transparent (sadness indicator)

### Color Palette

| Purpose | Color | Hex | Notes |
|---------|-------|-----|-------|
| Primary (Dress/Silhouette) | Pale White | #E8E8E8 | Pure, innocent, deteriorating |
| Skin | Blue-Gray | #9DB8C8 | Drowned, cold, ethereal |
| Accent (Trim) | Pale Blue | #C8E8F0 | Faded dye, water-logged |
| Aura (Peaceful) | Water Shimmer | #6FB3D5 | Cool blue, flowing |
| Aura (Sad) | Dim Cyan | #4A9FB5 | Muted, withdrawn |

### Personality & Voice

**Speech Pattern:**
- Soft, whispered voice (as if underwater)
- Hesitant, apologetic tone
- Uses simple, childlike phrasing
- Often trails off mid-sentence
- References water, shells, small creatures

**Key Phrases:**
- "Xin lỗi... xin lỗi..." (Sorry, sorry...)
- "Mẹ tôi... trước đây mẹ tôi..." (My mother... before...)
- "Có phải vỏ ốc nhớ những sinh vật mà nó được tạo thành không?" (Do shells remember the creatures that made them?)

**Emotional Range:**
- Withdrawn → Bonding → Vulnerable → Hopeful

### Story Arc & Dialogue

#### Visit 1: "The Shy Arrival" (Morning)
**Context:** Em Linh appears during mid-morning rush. Player notices she's different from Ông Văn.

**Arrival:**
```
[Sound: Water droplets instead of footsteps]
[Em Linh materializes slowly, hesitantly]

"Excuse me... can I... do you have tea?"
[Whispered, uncertain]
```

**Self-Doubt:**
```
"No, wait. I'm sorry. You're busy. I'll come back. 
I always come back too early."
[Looks down, ready to leave]
```

**Request:**
```
"Um. If you have it... incense? 
The kind that smells like flowers? 
My mother used to burn it on my birthday."
[Voice cracks slightly on "mother"]
```

**If served:**
```
[Inhales deeply, tears form silently]
"Yes. Yes. This is it. 
Thank you."
[Sits quietly, +1 Spirit Essence]
```

#### Visit 2: "The Bonding Moment" (Midday)
**Context:** Em Linh returns. She's more comfortable now. Player can engage deeper.

**Arrival:**
```
[Appears less hesitantly this time, though still soft]
[Approaches the stall and points at the beach]
```

**The Shell Observation:**
```
"Look—there. A shell. It's so beautiful. 
Do you think something lived in it?"

[Pause, waiting for response]

"Do you think it misses the creature that made it home?"
```

**Monologue (if player listens):**
```
"I think about things like that. Since I can't ask my mother anymore. 
What they're thinking. What they're feeling. 
Whether they miss the people they lost."

[Looks directly at player]

"I miss her. But I can't remember her face clearly. 
I can only remember... warmth. That feeling."
```

**Outcome:** +2 Spirit Essence, unlocked deeper dialogue path

#### Visit 3: "The Vulnerable Plea" (Sunset)
**Context:** Final visit before player departs for next shrine. Em Linh makes a personal request.

**Arrival:**
```
[Appears at dusk, her blue aura slightly brighter in the dimming light]
[Sits without ordering]
```

**The Core Moment:**
```
"I don't remember my mother's face clearly anymore. 
But I remember she loved me. 
That's enough, isn't it? That should be enough."

[Looks up at player directly for the first time]

"Will you remember me? Even after you leave? 
I don't want to be completely alone."
```

**Player's Internal Response:**
```
[Thought: Yes. I will.]
```

**Her Reaction:**
```
[Her form glows with soft blue light]

"Thank you, Tú. 
I'll be at peace now."

[She begins to dissolve gently, like water returning to the sea]

"Tell the living... tell them we're not monsters. 
We're just... lonely."
```

**Outcome:** +3 Spirit Essence, Em Linh's arc complete. She becomes the "good ending" indicator.

### Animation Requirements

**Idle State:**
- 2 frames: Floating/standing, gentle sway
- FX: Water droplets fall every 0.5 seconds from hair and dress
- Loop: 2 seconds per cycle

**Walk/Approach:**
- 4 frames: Gliding motion (no footsteps, just smooth movement)
- FX: Water trail follows behind (fades quickly)
- Loop: 1.2 seconds per cycle

**Sit Animation:**
- 2 frames: Settling down, then seated (but not resting on ground — hovering slightly)
- FX: Droplets form a small puddle that evaporates

**Sadness Trigger:**
- Frame flickers between solid and translucent (opacity: 70% → 50% → 70%)
- Aura shifts from bright blue to dim cyan
- Duration: 2 seconds

**Peace/Resolution Animation:**
- Glow brightens over 3 seconds
- Form becomes more transparent (80% → 95%)
- Gentle upward float
- Dissolve (fade to transparent) over 1.5 seconds

### Visual Reference Notes for Artists

**Silhouette Priority:** Em Linh should read as **young, innocent, tragic**
- Small frame, slight build
- Long hair extends below shoulders
- Torn áo dài hem visible
- Head slightly tilted down (shy posture)

**Lighting Context:**
- **Day phase:** Pale blue-gray tones; skin becomes slightly more desaturated
- **Transition:** Blue aura becomes more vibrant as sun sets (contrast increases)
- **At rest:** Gentle shimmer around edges (underwater effect)

**Comparison References:**
- **Age/delicacy:** Similar to child spirits in *Spirited Away* but more grounded, more human
- **Color palette:** Cool blues and pale whites, reminiscent of deep-water photography
- **Movement style:** Smooth, weightless, like slow motion

**Emotional Cue Design:**
- **Happy:** Brighter blue aura, more solid form, slight smile
- **Sad:** Dimmer aura, translucency increases, gaze downward
- **Peaceful:** Golden-blue blend in aura (hope + sadness merging)

---

## ANH MINH - THE JILTED BRIDE

### Character Identity

**Full Name:** Anh Minh (Elder Minh)  
**Role:** Complex NPC, moral gray area, redemption arc  
**Age (in death):** 30s  
**Death date:** ~45 years before game start (1975, during Fall of Saigon)  
**Status:** Angry, lonely, secretly hopeful  

### Visual Design Specifications

#### Face & Appearance
- **Striking features:** Sharp cheekbones, defined jawline, beautiful but severe
- **Expression:** Resting frown, eyes filled with old rage and disappointment
- **Hair:** Long, black, styled partially up (traditional bridal style, half-undone)
- **Eyes:** Intense, piercing, rarely softening
- **Makeup:** Remnants of wedding makeup (faded lipstick, kohl around eyes)

#### Clothing
- **Primary:** Áo dài in white and gold (traditional wedding dress)
- **Condition:** Torn, muddy, stained with what appears to be rust (blood?)
- **Trim:** Gold brocade, intricate patterns, now faded and weathered
- **Undergarment:** Pale silk slip visible through tears
- **Feet:** Bare, pale

#### Ghostly Manifestation
- **Translucency:** 65% opaque (between Ông Văn and Em Linh)
- **Form instability:** **Flickers when emotional** — edges pixelate, form destabilizes
- **Aura:** Golden shimmer with dark undertones (wavering between warmth and coldness)
- **Movement:** Sharp, deliberate, purposeful strides
- **Special effect:** Aura flickers between gold and gray depending on emotional state

### Color Palette

| Purpose | Color | Hex | Notes |
|---------|-------|-----|-------|
| Primary (Dress/Silhouette) | Off-White | #F5E6D3 | Aged, yellowed wedding dress |
| Accent (Trim/Gold) | Faded Gold | #D4AF37 | Precious but tarnished |
| Accent (Accents) | Deep Mauve | #8B6F7A | Stains, age, sorrow |
| Aura (Angry) | Harsh Gold | #FFD700 | Sharp, cutting light |
| Aura (Vulnerable) | Soft Gold | #E8C74C | Warm, hopeful |
| Aura (Flickering) | Gray-Gold | #A89D5E | Unstable, transitioning |

### Personality & Voice

**Speech Pattern:**
- Articulate, educated (upper-class Vietnamese from Hanoi/Huế)
- Sarcastic, cutting humor
- Uses classical Vietnamese poetry references
- Sharp tone, but with underlying vulnerability
- Speaks in measured cadences (formal, controlled)

**Key Phrases:**
- "Tại sao bạn lại quan tâm?" (Why do you even care?)
- "Chờ đợi không có kết thúc." (Waiting has no end.)
- "Hùng đã chạy trốn. Tôi chỉ chết." (Hùng ran away. I just died.)
- "Biết ơn bạn. Ít nhất ai đó còn nhớ rằng tôi tồn tại." (Thank you. At least someone remembers that I existed.)

**Emotional Range:**
- Hostile → Sarcastic → Vulnerable → Grateful

### Story Arc & Dialogue

#### Visit 1: "The Cutting Critique" (Early Morning)
**Context:** Anh Minh's first appearance. She's immediately antagonistic, testing the player.

**Arrival:**
```
[She walks in with sharp, purposeful steps]
[Her golden aura flickers with tension]
```

**The Critique:**
```
"Is this the best service you can offer? Pathetic."

[She examines the stall with visible disdain]

"I don't have time for slow service. The sun is burning away the day. 
And I... I don't have many of those left."
```

**Order (Demanding):**
```
"Wine. Strong wine. And mock money—a large pile. 
I don't have time to explain why."
```

**If served correctly:**
```
[She sips the wine slowly]
[Her expression softens slightly, though still guarded]

"You did well. For now. 
Perhaps you're not completely useless."
[+1 Spirit Essence, but distance remains]
```

**If served incorrectly:**
```
[She stands abruptly]

"This is not what I asked for. 
You're not listening. No one ever listens."

[She leaves, and her aura dims to gray]
[No reward]
```

#### Visit 2: "The Emotional Slip" (Midday)
**Context:** Anh Minh returns during the high-sun pressure period. Player notices a crack in her armor.

**Arrival:**
```
[She sits without speaking immediately]
[Her hands tremble slightly as she holds the wine glass]
```

**The Slip:**
```
[She stares at the mock money for a long time]

"This money used to mean something. Before he left."

[Long pause]

"Before Hùng left."

[Her aura flickers—gold fades to gray—as she catches herself]
```

**Defensive Recovery:**
```
"Never mind. Serve me. The sun is burning away the day. 
Just... don't ask."
```

**Player Impact:**
- ✅ If player shows concern → She stays longer, +2 Spirit Essence
- ❌ If player remains silent → She leaves quickly, +1 Spirit Essence

**Hidden Unlock:** This visit unlocks Visit 3 with full depth. Without it, Visit 3 is shorter.

#### Visit 3: "The Confession" (Sunset)
**Context:** Final visit. If player has been patient, Anh Minh opens completely.

**Arrival:**
```
[She arrives at dusk]
[Her aura is no longer flickering—it's stable, warm gold]
[She sits without being asked]
```

**The Story:**
```
"His name was Hùng. We were going to marry in the spring of 1975. 
A spring wedding. The flowers were already blooming."

[She stares at nothing, lost in memory]
```

**The War:**
```
"But the war came. Always the war. He was conscripted last-minute. 
During the chaos... during the evacuation... 
his family fled to the South."

[Her voice becomes quiet]

"He never came back."
```

**The Waiting:**
```
"I waited three years. Three years, Tú. 
Thinking he would return. Thinking the chaos would settle and he would come home."

[Voice breaks]

"When I finally accepted he wasn't coming back... 
I walked into the sea."
```

**The Gratitude:**
```
[She reaches out and gently takes your hand]
[Her touch is cold, but her grip is firm and human]

"You serve us like we matter. Like we're not just... forgotten. 
That's rare, Tú. Very rare."

[She looks directly into your eyes]

"Now go. Complete your work. The sun grows worse each day. 
And Hùng... wherever he is... I hope someone is kind to him too."
```

**Outcome:** +3 Spirit Essence, Anh Minh's arc complete. She becomes a symbol of bittersweet acceptance.

### Animation Requirements

**Idle State:**
- 2 frames: Standing proudly, slight turn of head
- FX: Aura pulses gently (gold → slight gray → gold) to show inner tension
- Loop: 2 seconds per cycle

**Walk/Approach:**
- 4 frames: Sharp, deliberate strides
- FX: Slight golden shimmer trails footsteps
- Loop: 0.8 seconds per cycle (faster than others — purposeful)

**Sit Animation:**
- 2 frames: Settling into seat with posture (back straight, dignified)
- FX: Golden aura dims slightly (vulnerable state)

**Emotional Flicker (Anger):**
- Form pixelates at edges (opacity flickers: 100% → 85% → 100%)
- Aura shifts to harsh, bright gold
- Duration: 1.5 seconds

**Emotional Stability (Vulnerability):**
- Flicker stops
- Aura shifts to softer, warmer gold with slight blue undertone
- Form becomes more solid
- Duration: 3 seconds

**Emotional Release (Resolution):**
- Aura glows warmly (stable gold with violet hints)
- Form becomes translucent and serene
- Gentle outward shimmer (like light through stained glass)
- Slow fade over 2 seconds

### Visual Reference Notes for Artists

**Silhouette Priority:** Anh Minh should read as **beautiful but broken, tragic yet defiant**
- Tall, upright posture (pride despite pain)
- Long hair (half-up bridal style suggests time frozen)
- Torn wedding dress (contrast between beauty and decay)
- Sharp facial features (elegant, not soft)

**Lighting Context:**
- **Day phase:** Gold tones desaturate, dress becomes more cream-colored
- **Transition:** Gold trim re-emerges as sun sets, warmth returns
- **Emotional shifts:** Aura color is the primary emotional indicator (gold = hope, gray = despair)

**Comparison References:**
- **Beauty/tragedy:** Similar to Vietnamese cinema portraits (like *A Tale of Love* or *Cyclo*)
- **Color palette:** Gold and cream against muted backgrounds, reminiscent of faded photographs
- **Movement style:** Precise, controlled, elegant even in grief

**Flickering Effect Design:**
- Flickering occurs **only during emotional distress** (anger, pain, frustration)
- Not a glitch — it's a visual metaphor for her fractured psyche
- Stops when she finds peace/acceptance
- Artists should think of it as "her form destabilizes when her emotions do"

---

## CÁ ÔNG BỘ XƯƠNG - WHALE LORD (BOSS)

### Character Identity

**Full Name:** Cá Ông Bộ Xương (Whale Lord Skeleton)  
**Role:** Boss encounter, story catalyst, tragic antagonist  
**Nature:** Mythological spirit (not a traditional ghost)  
**Age:** Unknown (possibly centuries old)  
**Status:** Desperate, territorial, corrupted by longing  

### Visual Design Specifications

#### Appearance
- **Body Shape:** Whale-like (massive, curved spine)
- **Skeletal Structure:** Half-skeleton, half-rotting flesh
- **Bones:** Pale, bleached white with barnacle encrustations
- **Flesh (if visible):** Deep green-black, decomposing, covered in seaweed
- **Size:** Massive — takes up ~40-50% of screen width during fight
- **Details:** Barnacles cluster on bones, seaweed trails from joints, bones are scarred and broken

#### Ghostly Manifestation
- **Translucency:** Mostly opaque (70-80%) — it's a physical presence, not ethereal
- **Bioluminescence:** Deep green glow emanates from eye sockets and gaps in skeleton
- **Aura:** Murky green-black fog surrounds it (like deep-sea pressure)
- **Movement:** Jerky, unnatural, breaking physics (teleports short distances, moves in stutters)
- **Sound:** Deep whale calls mixed with grinding bone sounds

#### Combat Visual Feedback
- **Attack charge:** Bones glow brighter green, fog darkens
- **Hit taken:** Brief flicker of red light at impact point
- **Weakened state:** Green glow dims, movements become more erratic

### Color Palette

| Purpose | Color | Hex | Notes |
|---------|-------|-----|-------|
| Primary (Bones) | Bone White | #F5F5F5 | Ancient, bleached |
| Secondary (Skeletal Details) | Pale Gray | #D3D3D3 | Shadow on bones |
| Accent (Rotting Flesh) | Deep Green-Black | #1A3A3A | Decay, abyssal |
| Accent (Barnacles/Decay) | Rust-Brown | #6B4423 | Oxidation, age |
| Glow (Primary) | Deep Sea Green | #2ECC71 | Bioluminescence |
| Glow (Secondary) | Cyan-Green | #5DEBB3 | Electric, unnatural |
| Aura (Fog) | Murky Green-Black | #0F2F1F | Pressure, danger |

### Personality & Voice

**Speech Pattern:**
- **Deep, reverberating voice** (sound design: whale call mixed with grinding)
- **Speaks in monosyllabic, emphatic phrases** (not articulate like human ghosts)
- **Repeats themes:** "SHORE," "HOME," "FORGOTTEN," "TAKEN"
- **Language:** Mix of whale-like sounds and distorted Vietnamese

**Key Phrases:**
```
"WHY DO YOU LEAVE? THE SHORE IS OUR HOME."
"THE DEAD MUST NOT ABANDON THE SHORE."
"FORGOTTEN... ALWAYS FORGOTTEN..."
"YOU DO NOT BELONG HERE."
```

**Emotional Capacity:**
- Desperate → Territorial → Mournful

### Story Context (Why Cá Ông?)

**Mythological Basis:**
- In Vietnamese folklore, Cá Ông (Whale King) is a protective spirit of fishermen and the sea
- Often honored with shrines and rituals
- Represents the boundary between human realm and marine realm

**Game Narrative Role:**
- Cá Ông is **not the real villain** — it's a victim of the hollow sun's corruption
- It's bound to the beach, unable to return to deep sea (where it's safe)
- It attacks the player not out of malice, but **desperation and territorial instinct**
- Defeating Cá Ông is not a "victory" — it's a tragic survival

**Connection to Act 1 Themes:**
- If player has been kind to ghosts, they understand Cá Ông's pain
- Cá Ông represents the cost of indifference: spirits become corrupted when neglected
- Sets up Acts 2-3 question: "What happens when the dead stop being people and become monsters?"

### Combat Design

**Boss Phase: Sunset → Night (Expedition Phase)**

**Arena:**
- Beach environ (sand, tide pools, rocks)
- Player can move left-right, has limited jumping
- Cá Ông occupies center-right of screen

**Attack Patterns:**

| Attack | Behavior | Tells | Counter |
|--------|----------|-------|---------|
| **Charge** | Rushes forward with shoulder, slow but heavy | Glowing intensifies, ground rumbles | Jump or dodge left |
| **Tail Sweep** | Sweeps from right to left | Tail glows, lifts up briefly | Jump over |
| **Roar** | Emits shockwave, pushes player back | Opens mouth, green glow pulses | Block or move back |
| **Bone Throw** | Detaches barnacle clusters, throws them | Limb glows, pulls back | Dodge right/left |

**Difficulty Scaling:**
- **Early game (less player upgrades):** Slower attack speed, longer tells
- **Late game (more player upgrades):** Faster, more erratic
- **Never unbeatable** — the goal is to reach the next shrine, not kill Cá Ông

**Phase 2 (If player is winning):**
- Cá Ông becomes more desperate, erratic
- Green glow flickers violently
- Attacks become less coordinated (signaling its pain/desperation)
- If cornered, it retreats to the sea (player wins encounter)

**Dialogue During Fight:**
```
[Opening]
"WHY DO YOU LEAVE? THE SHORE IS OUR HOME."

[Mid-combat]
"THE DEAD MUST NOT ABANDON THE SHORE."

[If player is winning]
"FORGOTTEN... ALWAYS FORGOTTEN..."

[Retreat]
"YOU DO NOT BELONG HERE. 
BUT NEITHER DO I ANYMORE."
[Returns to sea, defeated but not destroyed]
```

### Animation Requirements

**Idle State:**
- 2-3 frames: Swaying, breathing (subtle rib expansion)
- FX: Green glow pulses steadily
- Loop: 2 seconds per cycle

**Charge Attack:**
- 4 frames: Rear back → accelerate forward → impact
- FX: Green glow brightens during charge, dimmer at end
- Duration: 1.5 seconds total

**Tail Sweep:**
- 3 frames: Lift tail → sweep arc → return
- FX: Trailing green light follows tail
- Duration: 1.2 seconds

**Roar/Shockwave:**
- 2 frames: Mouth opens → releases shockwave
- FX: Green wave emanates outward (particle effect)
- Duration: 0.8 seconds

**Hit/Damage Reaction:**
- 1-2 frame flicker: Brief red glow at impact point
- Knockback: Slight backward movement
- FX: Bone splinter particles

**Desperation Phase:**
- Aura flickers (green → dark green → green)
- Movement becomes more erratic (less smooth)
- Attacks are more frequent but less coordinated

**Retreat Animation:**
- Slow backward movement
- Glow dims as it sinks into the sea
- Forlorn whale call plays
- Fade into darkness over 2 seconds

### Visual Reference Notes for Artists

**Silhouette Priority:** Cá Ông should be **instantly recognizable as a massive, otherworldly threat**
- Whale-like body is essential (curved spine, large frame)
- Skeletal structure should be clearly visible
- Bones should look ancient and scarred
- Bioluminescence should create an eerie, unnatural feeling

**Scale Reference:**
- Cá Ông is **2-3x larger than the player character**
- When at full height, it extends from ground to upper third of screen
- Positioning: Mostly right side of screen, leaving room for player movement

**Lighting Context:**
- **Day phase (before fight):** Barely visible in distance, just a silhouette
- **Dusk/transition:** Green glow becomes visible as it emerges
- **Night phase (during fight):** Primary light source is its own bioluminescence
- **Player's perspective:** Fighting a creature made of its own ghostly light

**Comparison References:**
- **Whale anatomy:** Use reference photos of whale skeletons (especially sperm whale or great whale)
- **Ghostly quality:** Similar to spirit designs in *Princess Mononoke* (San's forest guardians) or *Spirited Away* (the radish spirit's parents)
- **Color palette:** Deep-sea anglerfish bioluminescence (green and cyan), combined with bone white
- **Movement style:** Whale grace combined with skeletal jerking (uncanny valley effect)

**Deep-Sea Design Principles:**
- **Barnacles and encrustations:** Show age and time spent in the ocean
- **Seaweed/kelp:** Trails naturally from joints, suggests deep-sea origin
- **Decay patterns:** Green-black areas should suggest oxidation and decomposition
- **Bioluminescence:** Should seem natural for a deep-sea creature, not magical

---

## VISUAL STYLE GUIDE

### Overall Aesthetic

**Genre Tone:** Vietnamese Coastal Folk Horror  
**Time Period:** 1990s-2000s (visually) but timeless (spiritually)  
**Color Saturation:** Desaturated during day, increasingly saturated as night approaches  
**Lighting Style:** Harsh (daylight), mystical (dusk), eerie (night)  

### Color Philosophy

#### Day Phase (High Sun)
- **Palette:** Tans, pale golds, washed-out blues
- **Saturation:** Low (30-40% of normal)
- **Contrast:** High (harsh shadows, though no traditional shadows due to hollow sun)
- **Mood:** Oppressive, dreamlike, unnatural

#### Transition (Sunset)
- **Palette:** Indigo bleeding in, purples emerging, oranges deepening
- **Saturation:** Increasing (40-70%)
- **Contrast:** Medium (softer than day, not yet night)
- **Mood:** Bittersweet, dramatic, transitional

#### Night Phase (After Sun Sets)
- **Palette:** Deep blues, purples, greens, blacks
- **Saturation:** High (80-100%)
- **Contrast:** Low (moonlight is soft, bioluminescence creates specific highlights)
- **Mood:** Mysterious, dangerous, intimate

### Character Design Language

#### Visual Clarity
- **Every character should be recognizable by silhouette alone**
- Line weight should be consistent across all characters
- No character should "blend into" the background unintentionally

#### Emotional Indicators
- **Color:** Primary emotional indicator (aura shifts)
- **Form stability:** Secondary indicator (flickering = distress)
- **Movement:** Tertiary indicator (speed, smoothness)
- **Glow/luminescence:** Quaternary indicator (intensity = emotional state)

#### Cultural Specificity
- **Clothing:** Reflects Vietnamese traditions (áo dài, traditional fishing garments)
- **Architecture:** Shrine design based on real Vietnamese coastal temples
- **Symbols:** Include traditional motifs (bronze drum patterns, fishing nets, incense)
- **Not stereotypical:** Avoid generic "Asian" tropes; be specific to Vietnamese culture

### Animation Principles

#### Smoothness
- **Ghosts move smoothly** (no jerky transitions)
- **Cá Ông moves unnaturally** (jerky, frame-skipping to convey otherworldliness)
- **Player character moves realistically** (consistent physics)

#### Timing
- **Ông Văn:** Slow, deliberate (1-2 seconds per action)
- **Em Linh:** Smooth, flowing (1-1.5 seconds per action)
- **Anh Minh:** Purposeful, sharp (0.8-1 second per action)
- **Cá Ông:** Varied — slow swaying, fast attacks, erratic retreat

#### Spacing
- **More expressive:** Ghosts have larger spacing between frames (anticipation is visual)
- **More efficient:** Cá Ông has minimal spacing (lurching, not flowing)

---

## COLOR SPECIFICATIONS (DETAILED)

### Web Colors (RGB) for Programmers

#### Ông Văn Color Palette
```
Primary Shirt:       RGB(139, 115, 85)    #8B7355
Secondary Pants:     RGB(160, 149, 122)   #A0957A
Accent Skin:         RGB(212, 165, 116)   #D4A574
Aura Peaceful:       RGB(232, 160, 64)    #E8A040
Aura Distressed:     RGB(184, 116, 79)    #B8744F
```

#### Em Linh Color Palette
```
Primary Dress:       RGB(232, 232, 232)   #E8E8E8
Skin:                RGB(157, 184, 200)   #9DB8C8
Accent Trim:         RGB(200, 232, 240)   #C8E8F0
Aura Peaceful:       RGB(111, 179, 213)   #6FB3D5
Aura Sad:            RGB(74, 159, 181)    #4A9FB5
```

#### Anh Minh Color Palette
```
Primary Dress:       RGB(245, 230, 211)   #F5E6D3
Accent Gold:         RGB(212, 175, 55)    #D4AF37
Accent Stains:       RGB(139, 111, 122)   #8B6F7A
Aura Angry:          RGB(255, 215, 0)     #FFD700
Aura Vulnerable:     RGB(232, 199, 76)    #E8C74C
Aura Flickering:     RGB(168, 157, 94)    #A89D5E
```

#### Cá Ông Color Palette
```
Primary Bones:       RGB(245, 245, 245)   #F5F5F5
Secondary Gray:      RGB(211, 211, 211)   #D3D3D3
Flesh/Decay:         RGB(26, 58, 58)      #1A3A3A
Barnacle/Rust:       RGB(107, 68, 35)     #6B4423
Glow Primary:        RGB(46, 204, 113)    #2ECC71
Glow Secondary:      RGB(93, 235, 179)    #5DEBB3
Aura Fog:            RGB(15, 47, 31)      #0F2F1F
```

### Saturation Adjustments by Phase

**Day (High Sun):**
- Desaturate all colors by 30-40%
- Example: Ông Văn's shirt goes from #8B7355 → desaturated tan-brown

**Transition (Sunset):**
- Increase saturation by 10-20%
- Add warm undertones (increase red/orange channels by ~5-10%)

**Night (After Sunset):**
- Increase saturation to full (100%)
- Add cool undertones (increase blue/purple channels by ~5-10%)

### Aura Glow Effect (Technical Specs)

#### Ông Văn
- **Effect:** Soft golden outline, ~10-15px blur radius
- **Animation:** Steady pulse (1 second cycle, opacity 60% → 80% → 60%)
- **Color shift:** Warm #E8A040 when peaceful, dim #B8744F when troubled

#### Em Linh
- **Effect:** Soft blue shimmer, ~8-12px blur radius
- **Animation:** Gentle shimmer (particle-like water droplets)
- **Color shift:** Bright #6FB3D5 when content, dim #4A9FB5 when sad

#### Anh Minh
- **Effect:** Flickering golden edge, ~10-15px blur radius
- **Animation:** Unstable flicker (opacity jitters randomly between 50-100%)
- **Color shift:** Sharp #FFD700 when angry, soft #E8C74C when vulnerable

#### Cá Ông
- **Effect:** Bioluminescent glow from within, ~15-20px bloom
- **Animation:** Pulsing rhythm (similar to creature's breathing)
- **Color shift:** Bright #2ECC71 when alert, dim when weakened

---

## ANIMATION REQUIREMENTS (SUMMARY TABLE)

| Character | Idle | Walk | Sit | Special Action | Loop Time |
|-----------|------|------|-----|----------------|-----------|
| **Ông Văn** | 2 fr | 4 fr | 2 fr | Drink (3 fr) | 1-2 sec |
| **Em Linh** | 2 fr | 4 fr | 2 fr | Cry (2 fr) | 2 sec |
| **Anh Minh** | 2 fr | 4 fr | 2 fr | Flicker (8 fr) | 2 sec |
| **Cá Ông** | 2-3 fr | N/A | N/A | Charge (4 fr), Roar (2 fr) | 2 sec |
| **Player** | 2 fr | 4 fr | N/A | Attack (3 fr) | 1.2 sec |

**Frame Budget per Character:**
- Ông Văn: 15-20 frames total
- Em Linh: 15-20 frames total
- Anh Minh: 15-25 frames total (includes flicker frames)
- Cá Ông: 25-35 frames total (boss, more complex)
- **Total Act 1:** ~80-100 unique frames

---

## PRODUCTION CHECKLIST FOR ARTISTS

### Ông Văn
- [ ] Character sketch (multiple angles)
- [ ] Final line art (clean silhouette)
- [ ] Color blocking (using palette above)
- [ ] Idle animation (2 frames)
- [ ] Walk animation (4 frames, with limp)
- [ ] Sit animation (2 frames)
- [ ] Drink animation (3 frames)
- [ ] Emotional state variations (peaceful, distressed)
- [ ] Aura effect template
- [ ] Export all frames as PNG (transparent background)

### Em Linh
- [ ] Character sketch (multiple angles, showing wet hair)
- [ ] Final line art (emphasize ethereal quality)
- [ ] Color blocking (pale blue-gray palette)
- [ ] Idle animation (2 frames, with bobbing)
- [ ] Walk animation (4 frames, gliding motion)
- [ ] Sit animation (2 frames)
- [ ] Water droplet FX sprites (small circles, 3-5 variants)
- [ ] Emotional state variations (shy, bonding, peaceful)
- [ ] Aura effect template (water shimmer)
- [ ] Export all frames as PNG (transparent background)

### Anh Minh
- [ ] Character sketch (multiple angles, showing torn dress)
- [ ] Final line art (elegant but deteriorating)
- [ ] Color blocking (gold and white palette)
- [ ] Idle animation (2 frames)
- [ ] Walk animation (4 frames, sharp strides)
- [ ] Sit animation (2 frames, dignified posture)
- [ ] Flicker animation (8 frames showing pixelation)
- [ ] Emotional state variations (angry, vulnerable)
- [ ] Aura effect template (unstable gold)
- [ ] Export all frames as PNG (transparent background)

### Cá Ông Bộ Xương
- [ ] Character sketch (side view, showing scale)
- [ ] Final line art (whale skeleton, clear silhouette)
- [ ] Color blocking (bone white + green bioluminescence)
- [ ] Idle animation (2-3 frames, swaying)
- [ ] Charge animation (4 frames)
- [ ] Tail sweep animation (3 frames)
- [ ] Roar animation (2 frames)
- [ ] Damage reaction (flicker frames)
- [ ] Retreat animation (3-4 frames, fading)
- [ ] Bioluminescence glow FX
- [ ] Seaweed/barnacle detail sprites
- [ ] Export all frames as PNG (transparent background)

---

## NOTES FOR PROGRAMMERS

### Ghost State Machine
Each ghost follows this state progression:
1. **Neutral:** Hasn't interacted with player yet
2. **Present:** At the stall, waiting to be served
3. **Being Served:** Animation playing
4. **Satisfied:** Order fulfilled, dialogue triggered
5. **Departing:** Exit animation, resolved (for final visit only)

### Branching Logic
- Player's cumulative **kindness score** determines which ending occurs
- Each positive interaction with a ghost: +1 kindness point
- Each negative interaction: -1 kindness point (or 0 if neutral)
- **Good ending:** Kindness score ≥ 6 (all three main NPCs served 2+ times)
- **Neutral ending:** Kindness score 2-5 (most NPCs served once)
- **Bad ending:** Kindness score ≤ 1 (Chú Ba appears instead of boss)

### Aura/Glow Rendering
- Aura is NOT the character sprite itself
- Render as: Base sprite + separate glow layer (can use bloom/blur post-processing)
- Glow layer should be slightly offset (gives depth)
- Color of glow should transition smoothly based on emotional state

### Animation Timing
- Use consistent frame rate (60 FPS recommended)
- Animation timing should match dialogue (e.g., drink animation must align with "strong tea" spoken line)
- Consider adding slight anticipation frames (1-2 frames before main action) for appeal

---

## ADDITIONAL REFERENCES

### Vietnamese Folk Beliefs Referenced
- **Trống Đồng (Bronze Drum):** Ancient symbol of divine authority and celestial cycles
- **Nước Mắm (Fish Sauce):** Essential Vietnamese ingredient; here used as spiritual protection
- **Áo Dài:** Traditional Vietnamese dress; worn by both male and female characters historically
- **Shrine Rituals:** Incense, mock money (vàng mã), offerings of rice and fruit
- **Sea Spirits:** Vietnamese folklore includes various sea deities and protective whale spirits

### Film/Art References
- **Spirited Away (2001):** Ghost design, ritual service mechanics, emotional resonance
- **The Tale of Kieu (Truyện Kiều):** Vietnamese epic poem; themes of fate, duty, sacrifice
- **Vietnamese coastal cinema:** Color grading, lighting, atmosphere
- **Deep-sea photography:** Bioluminescence effects, creature design

### Research Resources
- Vietnamese fishing village architecture
- 1975 Fall of Saigon historical context (for Anh Minh's backstory)
- Central Vietnam coastal geography and weather patterns
- Traditional fishing techniques and maritime culture

---

## FINAL NOTES

This character sheet document is a **living reference**. As assets are created and tested:

1. **Update color values** if final renders differ from hex codes
2. **Adjust animation frame counts** based on engine capabilities
3. **Refine silhouettes** based on playtest feedback
4. **Iterate on aura effects** based on visual testing

All characters should feel **emotionally resonant** — not just visually interesting. Every frame, every color choice, every animation curve should serve the story.

**The goal:** Players should remember these characters after finishing the game.

---

**End of Character Sheets Document**

**Questions?** Refer to the Narrative Bible (SOLAR_PHOBIA_ACT1_NARRATIVE_BIBLE.md) for deeper context, dialogue, and story arcs.

---

*Version 1.0 — Ready for Production*  
*Last Updated: [Current Date]*  
*Status: Final for Act 1 Game Jam*
