# SOLAR PHOBIA: NẮNG GẮT
## ACT 1: BÃI THUYỀN THIÊU RỤI
### Complete Narrative Design Document

**Status:** Game Jam Prototype (Weeks 1-4)  
**Target:** Steam Release (Full production post-jam)  
**Scope:** Act 1 only — self-contained story arc  
**Team:** 2 Artists 2D (narrative-focused), 1-2 Programmers  

---

## PART 1: STORY OVERVIEW

### ACT 1 PREMISE

**Setting:** A forgotten fishing village on Vietnam's central coast (Quảng Ngãi / Bình Định region), circa 1990s-2000s. The boundary between life and death has worn thin. A relentless, **hollow sun** (Mặt Trời Trống Đồng) burns the beach during the day, but its heat is *wrong* — it saps the life from all things and attracts the vengeful dead.

**Player Role:** 
- You are **Tú**, a young soul-merchant (cò linh hồn) operating a makeshift **spirit stall** under a deteriorating shrine. 
- By day, you serve the restless dead — offering them ritual necessities (tea, incense, mock money) to keep them pacified.
- By night, when the sun retreats into an undersea void, you must venture across the cursed beach to the **next shrine** before your protective charm (nước mắm cốt — essence of fish sauce) wears off and the hollow sun returns to burn you.

**Core Emotional Arc:**
1. **Confusion → Discovery**: Who are these ghosts? Why do they come to your stall?
2. **Compassion → Horror**: Each ghost has a tragic story. Serving them brings closure… but also attracts worse things.
3. **Defiance → Acceptance**: The hollow sun cannot be defeated, only survived. Your role is not to save the dead, but to *witness* them.

---

## PART 2: THE WORLD — ENVIRONMENTAL STORYTELLING

### THE HOLLOW SUN (Mặt Trời Trống Đồng)

**Visual Description:**
- Oversized, pale, with a faint **drum-like pattern** visible on its surface (ancient Vietnamese bronze drum aesthetic).
- No shadows beneath objects during its reign — the light is uniform, unnatural, dreamlike.
- When it hangs high noon, the beach becomes **monochromatic tan** — desaturated, lifeless.
- Birds flee. Fish float belly-up in tidal pools. The air shimmers with heat that has *weight*.

**Mythological Basis:**
- Trống Đồng = ancient Vietnamese bronze drums, symbols of divine authority and celestial connection.
- The "hollow" sun evokes the empty resonance of a drum that sounds but produces no music — a void authority, a corrupted cosmic order.
- In Vietnamese folk belief, certain times/celestial events open doors to the spirit realm. The hollow sun is one such corruption.

**Mechanical Implications:**
- During **Day Phase** (Management): The sun is directly overhead. Long shadows are impossible. The beach is a pressure-cooker of spiritual energy — ghosts manifest hungrily.
- During **Night Phase** (Expedition): The sun sinks into the sea, but its residual heat (stored in the sand) still burns. The player has ~3-4 minutes of **nước mắm cốt** (sunscreen made from fermented essence) before they must reach the next shrine.

**Player Fear:** Not just "health disappearing" but *existential dread*. The sun doesn't kill you like a weapon; it erases you, turns you into another ghost wandering the beach.

---

### THE SHRINES (Am Thờ Chúng Sinh)

**Architectural Style:**
- Small, weathered structures (2-3 meters tall).
- Tiled roofs with raised eaves (traditional Vietnamese design).
- Peeling red paint, faded gold trim.
- Inscriptions on the lintel (half-readable):
  - "天地之靈 Linh hồn Trời Đất" (Spirits of Heaven and Earth)
  - "南無觀世音菩薩" or "Quy Mạng Âm Dương" (Guanyin/Yin-Yang refuge)
- Inside: Simple altar with incense urns, oil lamps (long extinguished), faded photographs of fishermen.

**Gameplay Function:**
- Acts as a **safe zone** during day phase (sun's power is lessened in the shrine's shadow).
- Stall is set up here: player serves ghosts, earns Spirit Essence.
- During night phase: player receives a burst of **nước mắm cốt** and must run to the next shrine before dawn.

**Narrative Implication:**
- Shrines are the *last bastions* of the old fishing community. They were built to appease the spirits of drowned fishermen, sea spirits, and ancestors.
- But they're failing. The hollow sun grows stronger each day, and even the shrines can't hold back the tide.
- By Act 1's end, it becomes clear: the shrines were never meant to save the living. They were built to *contain* the dead.

---

## PART 3: CHARACTER PROFILES (GHOST CUSTOMERS)

### CUSTOMER ROSTER FOR ACT 1

Each ghost has:
- A **visual design** (assets for artists)
- A **personality + voice** (dialogue for gameplay/cutscenes)
- A **tragic backstory** (worldbuilding, revealed through repeated visits)
- A **ritual need** (what they order at the stall)
- A **character arc** (how they change over the game)

---

### GHOST #1: "ONG VĂN" (Uncle Văn) — The Fisherman

**Visual Design:**
- Age: 60s-70s (in life)
- Appearance: Weathered face, burn marks on forearms, missing two fingers on left hand (fishing accident).
- Ghostly form: Slightly translucent, still wearing a faded blue fishing shirt. His edges **blur and pixelate** when he's distressed.
- Recognizable silhouette: Hunched posture, limping gait (old war injury + fishing injuries).

**Personality:**
- Gruff, nostalgic, irritable but ultimately kind.
- Speaks in a regional dialect (rough pronunciation, dropped tones).
- Uses fishing metaphors constantly.

**Tragic Backstory:**
- Died 5 years ago in a storm (drowned while trying to recover lost nets).
- Left behind a widow (now remarried) and two adult children (one in Saigon, one abroad).
- His children never came home to pay respects at the shrine.
- He doesn't blame them — he's disappointed in *himself* for not providing enough security.

**Ritual Need:**
- **Order 1 (first visit):** Strong tea (đặc, đặc — "strong, strong!") + incense stick (to remember old fishing rituals).
- **Order 2 (later):** A small pile of mock money (vàng mã) to "leave to his grandchildren, in case they need luck."

**Character Arc:**
- **Early visits:** Complains, demanding. "Why does your tea taste like seawater? Do you not know how to serve a man?"
- **Mid-game:** Opens up about his family. Asks player questions: "Do you have family? Do they come to shrines for you?"
- **Late-game (if player serves him correctly all 3 times):** A moment of peace. He sits quietly, sips tea, and whispers: "Thank you, Tú. I think... I can rest now. Tell the living — tell them the sea is not cruel. They just forgot how to listen."

**Dialogue Examples:**

*First Order (impatient):*
> **Ông Văn:** *shuffles impatiently* "You there! The tea — why is it not ready? I have been waiting since the sun rose. My throat is sand."

*Mid-Service (revealing):*
> **Ông Văn:** *pauses, stares at tea* "My oldest son, he went to America. Fifteen years, no letter. You understand? The sea took his father, so the boy ran to the opposite shore. Smart boy. Coward, but smart."

*Final Visit (peaceful):*
> **Ông Văn:** *holds cup with both hands, eyes closed* "This tea... it tastes like home. Before the war. Before the storms. Thank you, Tú. When you meet my son in the next life, tell him — the sea forgives him."

**Visual Storytelling (Environment):**
- His stall area: There's a small **fishing net** hanging in the corner (decoration + visual shorthand).
- If player looks closely: Small photos of a young man and woman (his children) are tucked into the shrine's corner, covered in dust. No one's cleaned them in years.

---

### GHOST #2: "EM LINH" (Little Linh) — The Drowned Girl

**Visual Design:**
- Age: 12-14 (appearance in death)
- Appearance: Pale, with long black hair (wet, always dripping). She wears a faded áo dài (traditional dress) torn at the hem. Her skin has a **blue-gray pallor**.
- Ghostly effect: She trails **water droplets** as she moves, leaving brief puddles that evaporate instantly.
- Her face is serene but sad — not monstrous.

**Personality:**
- Soft-spoken, polite to a fault. She seems almost *eager* to please.
- Often apologizes for existing: "Sorry, sorry, am I in the way?"
- Giggles occasionally, but it sounds distant — like a memory of laughter rather than real joy.

**Tragic Backstory:**
- Drowned 2 years ago while swimming with friends.
- Her body was never found. Parents performed funeral rites anyway, but couldn't let her go.
- She wanders the beach at night, sometimes trying to warn living children to stay away from the water.
- No one listens. She doesn't blame them.

**Ritual Need:**
- **Order 1:** Incense (she wants "the kind that smells like flowers — my mother used to burn it on my birthday").
- **Order 2:** A small offering of **rice** (not money — she asks if the player can spare rice to "feed the fish, so they don't get lonely").

**Character Arc:**
- **Early visits:** Withdrawn, almost transparent. "I won't stay long. Someone else probably needs the space."
- **Mid-game:** Bonds with player over shared observations. "You know, at night, the crabs come out. They click and click. Do you think they're talking to each other?"
- **Late-game:** If player has been kind, she asks a favor: "When your shift ends... will you walk to the water's edge? Not to go in. Just to... sit. Sometimes I can't remember why I was swimming that day. If someone sits by the water and remembers me, maybe that will help."

**Dialogue Examples:**

*First Order (apologetic):*
> **Em Linh:** *whispers* "Excuse me... can I... do you have tea? No, wait — I'm sorry. You're busy. I'll come back. I always come back too early."

*Mid-Game (bonding):*
> **Em Linh:** *points at incoming tide* "Look — there. A shell. It's so beautiful. Do you think something lived in it? Do you think it misses the creature that made it home?"

*Final Request (emotionally vulnerable):*
> **Em Linh:** *voice cracks* "I don't remember my mother's face clearly anymore. But I remember she loved me. That's enough, isn't it? That should be enough."

**Visual Storytelling:**
- Her area of the beach always has **flowers** washed ashore (lilies, lotus petals) — a visual signature suggesting her peaceful nature.
- Near the shrine, there's a **small stone shrine** (more weathered than the main one) — probably built by her grieving parents.

---

### GHOST #3: "ANH MINH" (Elder Minh) — The Jilted Bride

**Visual Design:**
- Age: 30s (appearance in death)
- Appearance: Beautiful but severe. She wears a **wedding áo dài** (traditional white-and-gold wedding dress), but it's torn, muddy, and stained with what looks like rust.
- Her expression: Serene rage. A beautiful face twisted with decades of sorrow.
- Ghostly effect: She **flickers** — her form destabilizes when she's emotional, becoming pixelated or translucent.

**Personality:**
- Intelligent, articulate, fiercely proud.
- Speaks with bitter humor. Sarcasm is her defense mechanism.
- Beneath the anger: profound loneliness.

**Tragic Backstory:**
- Was engaged to a fisherman named Hùng. Wedding was set for spring 1975 (right before the Fall of Saigon).
- Hùng was conscripted last-minute. During the chaos of evacuation, his family fled to the South. He never came back.
- Minh waited 3 years, thinking he'd return. When it became clear he wouldn't, she walked into the sea.
- Her body was never recovered. Rumor has it she haunts the beach, waiting for a man who's now lived another 40 years in exile.

**Ritual Need:**
- **Order 1:** Wine or liquor ("to steady my nerves") + incense.
- **Order 2:** Mock money ("to build a shrine for myself, since no one else will").
- **Order 3 (rare):** A quiet moment of conversation — she needs to *talk*, not just be served.

**Character Arc:**
- **Early visits:** Hostile. Critiques player's every action: "Is this the best service you can offer? Pathetic."
- **Mid-game:** Vulnerability creeps in. She mentions Hùng's name accidentally, then tries to cover it.
- **Late-game:** If player shows patience, she offers a confession: "Do you know what's worse than death? Outliving your future."

**Dialogue Examples:**

*First Order (cutting):*
> **Anh Minh:** *examines cup critically* "This wine tastes like it's been sitting since 1975. Perhaps we have something in common. Stale. Forgotten. Waiting."

*Mid-Game (emotional slip):*
> **Anh Minh:** *stares at wedding dress hem, voice soft* "Hùng liked this dress. He said it made me look like a starlight. Stupid boy. Cowardly boy."

*Late-Game (confession):*
> **Anh Minh:** *grips player's hand (her touch is cold)* "I know why you serve us so carefully. You're waiting for someone too, aren't you? Or maybe... you're trying to save us because you couldn't save yourself. Either way — thank you. That's braver than Hùng ever was."

**Visual Storytelling:**
- Her stall area has **wilted flowers** (always dead, always being slowly swept away by the tide).
- A **torn photograph** is pinned to the shrine — you can barely make out a young couple, smiling, faces faded by decades.

---

### GHOST #4: "CHÚ BA" (Uncle Ba) — The Malevolent Fisherman (OPTIONAL / SECRET)

**Status:** Easter egg / bad-ending path. Only appears if player makes certain choices (serves Ông Văn poorly, ignores Em Linh, dismisses Anh Minh's story).

**Visual Design:**
- Age: 50s
- Appearance: Thin, wiry, with **bloated face** (drowned look). His skin has a **greenish tint**. His eyes are **hollow** — two dark voids.
- Ghostly effect: He **doesn't move smoothly** — he *jerks* or glitches from one position to another, breaking normal physics.
- His voice: Distorted, echoing, like it's coming from underwater.

**Personality:**
- Silent, unsettling. He barely speaks.
- When he does, he speaks in riddles or reversed sentences.
- His presence causes the temperature to drop and the sky to darken prematurely.

**Tragic Backstory:**
- Drowned 30 years ago under mysterious circumstances — murder is suspected.
- Never received proper funeral rites (his killer was never caught).
- He's been festering in the sea, slowly turning into something worse than human.
- If the living don't remember the dead with kindness, they become *hungry*.

**Ritual Need:**
- He doesn't ask for things. He *takes* them.
- If he appears, he drains resources from the shrine and from your stall.

**Character Arc:**
- Appears only in **bad ending path**: If player has been dismissive of all ghosts, Chú Ba appears as a punishment.
- He represents the consequence of indifference: the dead don't stay peaceful. They become corrupted.

**Why Include Him:**
- Adds moral weight to the stall management phase.
- Teaches player: "If you don't serve the dead with compassion, they'll consume you instead."
- Sets up Act 2 threat: Something worse than ghosts is stirring.

---

## PART 4: THE STALL — GAMEPLAY + NARRATIVE

### THE STALL SETUP (BÃI THUYỀN SHRINE)

**Location:** A small, crumbling shrine built into the cliff face of a fishing village. The stall is makeshift — a **wooden lean-to** with palm-leaf roof, held up with salvaged fishing boat wood.

**Interior Details (for artists to illustrate):**
- **Altar table:** Three sticks of incense urns (one on the left, one center, one right). The center urn is the oldest.
- **Tea station:** A small brazier with a clay pot. Cups stack haphazardly.
- **Offerings shelf:** Mock money (vàng mã) stacked in bundles, flowers (fresh and wilted), bowls of rice.
- **Walls:** Covered in faded photographs and newspaper clippings — memorial notices, old fishing schedules, a calendar from 1998 that no one removed.
- **Hanging:** A fishing net, lantern (unlit), a small bell (ding-ding-dong sound when customers arrive).

**Atmosphere (for sound design / visual tone):**
- **Quiet, meditative, slightly melancholic.**
- Soft creaking of the lean-to in the breeze.
- The **distant sound of waves** (always present).
- Occasional **seagull cries** (becoming less frequent as day progresses — they flee when the hollow sun rises).

---

### STALL MECHANICS — NARRATIVE INTERPRETATION

**Order System (Gameplay ↔ Story):**

Each ghost's order has **two layers**:

1. **Practical Layer (Gameplay):**
   - Player sees: "Tea + Incense" or "Money" in an order UI.
   - Player clicks items to fulfill order.
   - Timer counts down.
   - Success → Spirit Essence earned.

2. **Narrative Layer (Story):**
   - Why does Ông Văn ask for strong tea? *He wants to remember the taste of home.*
   - Why does Em Linh ask for rice? *She's sharing her loneliness with the sea creatures.*
   - Why does Anh Minh ask for wine? *She's trying to numb the pain of waiting.*

**Design Philosophy:**
- **The stall is a confessional, not a transaction.**
- Every order is a prayer. Every service is an act of witnessing.
- The *way* player serves (hasty vs. careful, dismissive vs. engaged) affects the ghost's state of mind.

**Mechanical Reflection:**
- If player serves quickly but carelessly → Ghost leaves satisfied but unchanged (no dialogue bonus).
- If player serves with "good enough" speed but shows signs of listening (hovering over their stories) → Ghost opens up, reveals more lore.
- If player is too slow → Ghost gets irritable, leaves early, Spirit Essence reward reduced.

---

### THE NƯỚC MẮM CỐT (SUNSCREEN MECHANIC)

**Narrative Context:**
- **Nước mắm cốt** = fermented fish sauce essence, traditionally used by fishermen to protect from sun and sea salt.
- In this world, it's been **blessed by shrine monks** (long dead) to protect against the hollow sun specifically.
- It's **not permanent**. Over time (maybe 10-15 minutes of playtime), the protection fades and the player must reach a shrine or risk being burned.

**Storytelling Purpose:**
- Reinforces the **time pressure** of escaping the beach.
- Introduces the concept of **ritual preparation** — every journey requires spiritual readiness.
- Visual cue: Player's skin might **glow slightly** when protected (indicating blessed state) and **fade/blister** as it wears off.

**In Dialogue:**
- When player receives nước mắm cốt at shrine, an NPC (or Ông Văn) might say: "This will protect you for a while. But the sun grows stronger each day. Soon, even this won't be enough. You must move fast."

---

## PART 5: ACT 1 STORY STRUCTURE

### THREE-ACT MINI-NARRATIVE

**Overall Arc:** "The Witness's First Day"

---

### ACT 1A: MORNING — CONFUSION (Gameplay: Tutorial + Setup)

**Duration:** ~10-15 minutes of playtime

**Scene Breakdown:**

**Scene 1: Awakening at the Shrine**
- Player wakes up in/near the shrine. 
- Dialogue from an **unseen voice** (could be Ông Văn, could be a shrine spirit):
  > "You've arrived at last. The sun will rise soon. Are you ready to serve?"
- Tutorial starts: "Click the items to arrange the stall. Light the incense. Prepare the altar."
- Visual: The sun is still low on the horizon, painting the beach in red-gold. It's almost beautiful.

**Scene 2: First Customer Arrives (Ông Văn)**
- Ông Văn emerges from the beach mist, limping.
- Tutorial: "Fulfill his order. He wants strong tea and incense."
- After serving: Ông Văn grunts approval. "Good. That's how my wife used to make it. You might not be useless after all."
- **Story Beat:** Player learns what this is — serving the dead.

**Scene 3: Morning Accelerates**
- More ghosts arrive. The pace picks up.
- Sky gradually becomes **less golden, more pale and harsh**.
- Player notices the sun is climbing, and its light feels *draining* — colors start to wash out.
- A title card appears: **"High Sun Pressure"** — orders arrive faster, customer patience decreases.

**Scene 4: First Horror Moment**
- Around noon, a customer *almost* manifests wrong.
- You see a figure approaching, but its outline is **distorted, wrong** — it glitches between forms.
- Then it stabilizes, and it's just Anh Minh, but with a cruel smile: "What's wrong, Tú? Scared?"
- **Story Beat:** Hint that something darker is possible if the dead aren't properly tended.

**Story Themes in Act 1A:**
- Introduction to the mechanic and the world.
- Tú is learning their role as a witness.
- The dead are people first — sad, lonely, needing kindness.

---

### ACT 1B: AFTERNOON — COMPASSION (Gameplay: Character Development + Moral Choice)

**Duration:** ~15-20 minutes

**Scene Breakdown:**

**Scene 1: Em Linh Appears**
- Soft sound cue: water droplets instead of footsteps.
- Em Linh enters shyly. Player serves her (she asks for incense that smells like flowers).
- Dialogue: Em Linh mentions her drowned mother and the flowers. Player can choose:
  - **(A) Show sympathy:** "I'll remember you, Em Linh."
  - **(B) Stay professional:** Serve and move on.
- If (A): Em Linh's dialogue shifts later. She becomes a recurring, emotional anchor.
- If (B): She still serves the story function, but less deeply.

**Scene 2: Anh Minh's Hostility**
- Anh Minh arrives, demanding service.
- Her dialogue is biting: "I don't have time for slow service. The sun is burning away the day."
- Player must serve her *correctly* (including getting the ritual items right) to avoid her getting angrier.
- Subplot hint: She mentions a man's name (Hùng) bitterly, then clams up.

**Scene 3: Ông Văn Returns (Unexpected)**
- Ông Văn comes back a second time in the same day (unusual).
- He sits quietly for a moment, then: "That girl with the wet hair. The drowned one. Be kind to her. She reminds me of my daughter when she was small. Before the world hardened her."
- **Story Beat:** The ghosts care about each other. They're not isolated souls; they form a small community.

**Scene 4: The Sun Reaches Its Peak**
- Visual: The beach is now **completely desaturated**. Colors are gone. Everything is tan/gray.
- The hollow sun sits directly overhead, and its presence is **oppressive**.
- Text notification: "The Hollow Sun reaches its apex. Spirits grow restless."
- Rapid-fire orders. The pressure mounts.

**Scene 5: First Failure (Teaching Moment)**
- One customer's order times out or goes unfulfilled.
- Instead of just losing points, something narrative happens:
  - The ghost *starts to distort* — their form glitches.
  - They get angry or sad: "Why won't anyone listen? Why am I here if no one cares?"
  - Then the shrine's light flares, and they calm down (the shrine's protection stabilizes them).
- **Story Beat:** If the dead aren't cared for, they *degrade*. The stall isn't just a job; it's mercy.

**Scene 6: Breakthrough Moment (Optional)**
- If player has been consistently kind and patient, Anh Minh opens up slightly:
  > "You serve us like we matter. Like we're not just... forgotten. That's rare, Tú. Don't lose that."
- **Story Beat:** The player is doing something important, whether or not they realize it yet.

**Story Themes in Act 1B:**
- The dead aren't monsters. They're people with unfinished business and heartbreak.
- Compassion is a choice, and it changes the story.
- The living world is forgetting the dead. The player is one of the last bridges.

---

### ACT 1C: SUNSET — DEFIANCE (Gameplay: Boss Encounter + Climax)

**Duration:** ~15-20 minutes

**Scene Breakdown:**

**Scene 1: The Decline**
- Visual shift: The sun begins to sink toward the horizon, but its descent is *unnatural* — jerky, wrong-paced.
- Color starts to return to the world, but now there's a **deep indigo shadow** creeping across the beach.
- A new notification: "The Hollow Sun recedes. The beach transitions to Night. You must prepare to leave the shrine."

**Scene 2: Final Customer Confessions**
- In the last moments of daylight, the three main ghosts (or two, depending on choices) come one final time.
- Each offers a small moment of closure:
  - **Ông Văn:** "When you reach the next shrine, remember that fishermen once thrived here. We were strong. Don't let them forget."
  - **Em Linh:** "Will you come back? I don't want to be alone tonight."
  - **Anh Minh:** "I've waited so long for someone to listen. Thank you, Tú. Now go. The sun's rage only grows as night comes."
- **Story Beat:** The ghosts are sending the player onward. This is their blessing.

**Scene 3: The Preparation**
- Ông Văn or a shrine spirit hands player the **nước mắm cốt**: "This will protect you. Run to the next shrine. Don't look back."
- UI notification: "Nước Mắm Cốt equipped. Duration: 15 minutes. Exposure to sunlight (when night ends) will drain HP."
- Visual: Player sees the path ahead — a beach stretching into darkness, with a faint silhouette of the *next shrine* in the distance.

**Scene 4: THE BOSS — Cá Ông Bộ Xương (Whale Lord Skeleton)**
- As player begins the sprint, a **massive silhouette** emerges from the sea.
- It's **Cá Ông** — a mythological whale-like spirit, half-skeleton, half-rotted flesh, covered in barnacles and seaweed.
- Cá Ông is **not evil**. It's desperate, territorial, and *hungry for spiritual energy*.
- It attacks the player not out of malice, but because the player is leaving the shore, and Cá Ông is bound to the beach.

**Combat Mechanics (Narrative Framing):**
- Player must defend themselves while also *running* toward the next shrine.
- Every few steps, player can attack (swing the Đòn Gánh).
- Cá Ông attacks in slow, heavy arcs — it's not fast, but its hits are devastating.
- The goal is not to *kill* Cá Ông, but to **survive long enough to reach the shrine**.

**Dialogue During Boss Fight:**
- Cá Ông speaks in a **deep, reverberating voice** (sound design: distorted, whale-like):
  > "WHY DO YOU LEAVE? THE SHORE IS OUR HOME. THE DEAD MUST NOT ABANDON THE SHORE."
- Player can respond (in their head): "I'm not abandoning them. I'm surviving."

**Scene 5: The Shrine Gateway**
- As player approaches the next shrine, Cá Ông's attacks become more frantic.
- A **sudden burst of light** from the shrine repels Cá Ông back into the sea.
- Cá Ông's final words (sadder than angry):
  > "FORGOTTEN... ALWAYS FORGOTTEN..."
- Player collapses at the threshold of the new shrine, breathing hard.

**Scene 6: Epilogue (Act 1 Conclusion)**
- A new shrine, a new setting, but also a new stall to set up.
- Em Linh's voice (echoing from the previous shrine, now distant): "Come back, Tú. Don't forget us."
- **Title card: "END OF ACT 1: THE WITNESS'S FIRST DAY"**
- **New title: "ACT 2: THE LIGHTHOUSE SHRINE" (teaser for next section)**

**Story Themes in Act 1C:**
- The player understands their role: not a savior, but a *bridge* between the living and dead.
- The world is bigger and more dangerous than one shrine.
- But the work is worth doing.

---

## PART 6: OPTIONAL ENDINGS (PLAYER CHOICE)

### GOOD ENDING (If player has been kind to all three main ghosts)

**Final image:** As player reaches the second shrine, all three ghosts (Ông Văn, Em Linh, Anh Minh) appear briefly in the distant mist, waving goodbye. They're **glowing faintly**, more peaceful.

**Text:**
> "The dead remember those who remember them. You have given them what they needed: witness, compassion, a moment when someone cared. The sun may burn the living, but kindness endures."

---

### NEUTRAL ENDING (If player was mostly neutral/professional)

**Final image:** The ghosts are still present at the first shrine, but they feel more distant. The stall is still running, but there's a sense that something essential is missing.

**Text:**
> "You have done your duty. The stall operates. The shrine is maintained. But the dead wonder: did you ever truly see them? Or were we just orders to fill?"

---

### BAD ENDING (If player was consistently dismissive/cruel)

**Special Scene: Chú Ba's Appearance**
- Instead of Cá Ông, a different creature emerges: **Chú Ba**, the corrupted ghost.
- The boss fight is the same mechanically, but Chú Ba is **faster, more aggressive, glitching**.
- The shrine's light is *weaker* against Chú Ba — it doesn't repel him as effectively.

**Final image:** Player barely makes it to the shrine. Behind them, Chú Ba is *pounding* at the shrine's boundary, unable to enter but clearly *furious*.

**Text:**
> "The dead do not forget cruelty. You ignored their pain, dismissed their stories. Now something worse festers in the shore. The hollow sun has found a new servant."

**Implication:** Sets up Act 2 as a *consequence* — the player has made an enemy.

---

## PART 7: VISUAL DIRECTION FOR ARTISTS

### COLOR PALETTE

**Day Phase (High Sun):**
- **Desaturated tan, pale gold, washed-out blue.**
- Ghosts appear as **darker silhouettes**, almost monochromatic.
- The sky is **hazy, bleached** — all reds and oranges have been burned away.
- Shrine casts **almost no shadow** (the hollow sun negates traditional shadow).

**Transition/Sunset:**
- **Indigo bleeds in from the edges.**
- The first purple and magenta appear.
- Ghosts become **more vivid and detailed** as the hollow sun loses power.
- The beach becomes **layered with color again**.

**Character-Specific Visual Cues:**

1. **Ông Văn:**
   - **Brown tones, weathered textures.**
   - Visual shorthand: **fishing net, rope, boat wood.**
   - Aura effect (optional): Slight **orange glow** (warmth of memory).

2. **Em Linh:**
   - **Blue-gray-white palette.**
   - Visual shorthand: **water droplets, flowers, flowing fabric.**
   - Aura effect: **Blue shimmer, like underwater light.**

3. **Anh Minh:**
   - **Gold and white (wedding dress), with dark stains.**
   - Visual shorthand: **torn fabric, wilted flowers, old photograph.**
   - Aura effect: **Gold shimmer that flickers to gray** (unstable between hope and despair).

4. **Cá Ông Bộ Xương:**
   - **Monochromatic bone-white with deep green-black shadows.**
   - Visual shorthand: **whale shape, barnacles, seaweed, skeleton.**
   - Aura effect: **Dark green bioluminescence** (like deep-sea creature).

---

### ENVIRONMENTAL ASSETS (PRIORITY FOR ART TEAM)

**High Priority (Necessary for Act 1):**
1. **Bãi Thuyền Shrine** (multiple states: calm, pressured, damaged)
2. **The Hollow Sun** (overhead perspective, varying opacity)
3. **The Stall** (interior, with all interactive elements visible)
4. **Beach transition parallax** (foreground, mid-ground, horizon)
5. **Cá Ông Bộ Xương** (boss sprite, 8-12 frames attack animation)

**Medium Priority (Enhances immersion):**
1. Small details: **fishing nets, urns, photographs, lanterns**
2. **Weather effects: mist, dust, shimmer**
3. **Particle effects: spirit orbs, water spray, smoke**

**Low Priority (Polish):**
1. **Secondary shrine interior details**
2. **Background characters** (living fishermen in Act 2)
3. **Animated foliage**

---

## PART 8: DIALOGUE SCRIPT (FULL)

### ONG VĂN — COMPLETE DIALOGUE TREE

**First Visit (Impatient):**
```
[Enter animation]
Ông Văn: "You there! The stall—is it ready? I have been waiting since the sun rose. My throat is sand."
Player (thought): [What does he want?]
Ông Văn: "Tea. Strong tea. And one incense stick. Just one. I am not greedy."
```

**Serving (If quick):**
```
[Player delivers tea and incense]
Ông Văn: [Sips]. "Hnh. Better than I expected."
[Sits, satisfied, offers +1 Spirit Essence]
```

**Serving (If slow):**
```
[Timer running low]
Ông Văn: "How long does it take to pour tea? A child could do this faster!"
[If not completed: Leaves irritated, -Spirit Essence]
```

**Mid-Game Visit (Revelation):**
```
[Sits again]
Ông Văn: "You know... my oldest son, he went to America. Fifteen years. No letter."
Player (thought): [He wants to talk...]
Ông Văn: "The sea took his father. The boy ran to the opposite shore. Smart. Cowardly, but smart."
Player (thought): [Should I respond?]
Ông Văn: "If you meet him in the next life, tell him—the sea doesn't hate him. He just forgot how to listen."
[+2 Spirit Essence, unlocks his final visit]
```

**Final Visit (Peace):**
```
[Approaches slowly]
Ông Văn: [Holds the tea cup with both hands, eyes closed]
Ông Văn: "This tastes like home. Before the war. Before the storms. Thank you, Tú."
[Long pause]
Ông Văn: "I think... I can rest now."
[His form glows softly, becomes more translucent]
[+3 Spirit Essence, Ông Văn's arc complete]
```

---

### EM LINH — COMPLETE DIALOGUE TREE

**First Visit (Shy):**
```
[Emerges from the mist, footsteps are water droplets]
Em Linh: *whispers* "Excuse me... can I... do you have tea?"
Player (thought): [A young voice...]
Em Linh: "No, wait. I'm sorry. You're busy. I'll come back. I always come back too early."
Player: [Can choose response or stay silent]
Em Linh: "Um. If you have it... incense? The kind that smells like flowers? My mother used to burn it on my birthday."
```

**Serving:**
```
[Player delivers flower-scented incense]
Em Linh: [Inhales deeply, tears form]
Em Linh: "Yes. Yes. This is it. Thank you."
[Sits quietly, +1 Spirit Essence]
```

**Mid-Game Visit (Bonding):**
```
Em Linh: [Points at incoming tide]
Em Linh: "Look—there. A shell. It's so beautiful. Do you think something lived in it?"
Player (thought): [She's trying to share something...]
Em Linh: "Do you think it misses the creature that made it home?"
Player (thought): [I should listen...]
Em Linh: [Smiles sadly] "I think about things like that a lot. Since I can't ask my mother anymore."
[+2 Spirit Essence]
```

**Final Visit (Vulnerable Request):**
```
Em Linh: [Looks up at player directly]
Em Linh: "I don't remember my mother's face clearly anymore. But I remember she loved me."
Em Linh: "That's enough, isn't it? That should be enough."
Player (thought): [What can I say?]
Em Linh: "Will you remember me? Even after you leave? I don't want to be completely alone."
Player (thought): [Yes. I will.]
Em Linh: [Her form glows with blue light]
Em Linh: "Thank you, Tú. I'll be at peace now."
[+3 Spirit Essence, Em Linh's arc complete]
```

---

### ANH MINH — COMPLETE DIALOGUE TREE

**First Visit (Hostile):**
```
[Enters with sharp, deliberate steps]
Anh Minh: [Examines the stall critically]
Anh Minh: "Is this the best service you can offer? Pathetic."
Player (thought): [She's angry...]
Anh Minh: "Wine. Strong wine. And mock money—a large pile. I don't have time to explain."
```

**Serving (If correct):**
```
[Player delivers wine and money]
Anh Minh: [Sips wine, expression softens slightly]
Anh Minh: "You did well. For now."
[Sits, but remains distant, +1 Spirit Essence]
```

**Mid-Game Visit (Emotional Slip):**
```
Anh Minh: [Stares at the mock money pile]
Anh Minh: "This money... it used to mean something. Before he left."
Player (thought): [Who is 'he'?]
Anh Minh: [Catches herself, voice becomes sharp again]
Anh Minh: "Never mind. Serve me. The sun is burning away the day."
[But she lingers, +2 Spirit Essence]
```

**Late-Game Visit (Confession):**
```
[If player has served her well twice before]
Anh Minh: [Sits without ordering]
Anh Minh: "His name was Hùng. We were going to marry in spring of 1975."
Player (thought): [Now she's really opening up...]
Anh Minh: "But the war came. He was sent away. He never came back."
Anh Minh: [Voice breaks] "I waited three years. Three years of waiting."
Anh Minh: [Grips player's hand—her touch is cold]
Anh Minh: "You serve us like we matter. That's rarer than you know. Thank you, Tú."
Anh Minh: "Now go. Complete your work. The sun grows worse each day."
[+3 Spirit Essence, Anh Minh's arc complete]
```

---

## PART 9: LEVEL DESIGN (SIMPLIFIED)

### BÃII THUYỀN SHRINE — LAYOUT BLUEPRINT

```
[EXTERIOR VIEW — Bird's Eye]

                    [CLIFFS - Background]
              [Weathered Shrine Building]
                        |
                    [STALL AREA]
                        |
    [Beach — Sand]  [Player Area]  [Beach — Sand]
                        |
    [Tide marks]    [Waves]  [Tide marks]
                        
                    [Distant: Next Shrine]
```

**Interior (for visual reference):**
- **Left nook:** Incense urns, shrine bells.
- **Center:** Tea station with brazier.
- **Right nook:** Offerings shelf (mock money, flowers, rice bowls).
- **Background:** Altar with faded photographs.
- **Overhead decoration:** Fishing net, lantern.

**Functional Areas (for gameplay):**
1. **Altar zone** (where ghosts arrive and sit)
2. **Service zone** (where player interacts with items)
3. **Transition zone** (where player prepares to leave, receives nước mắm cốt)

---

### BEACH EXPEDITION ROUTE

```
[Shrine 1 (Bãi Thuyền)]
            |
         ~300px (Beach)
            |
    [Cá Ông Boss encounter]
    (Spawns mid-route, attacks player)
            |
         ~200px (Final Sprint)
            |
   [Shrine 2 (Entrance)]
```

---

## PART 10: POST-GAME JAM EXPANSION ROADMAP

**Act 1** (Game Jam): Bãi Thuyền Shrine + Beach Expedition  
↓  
**Act 2** (Post-Jam): Lăng Ông Shrine + Corrupted Temple Interior  
↓  
**Act 3** (Full Release): Chợ Âm Phủ + Climactic Confrontation  

### ACT 2 TEASER HOOKS
- **Who is Cá Ông, really?** Why is it so protective of the beach?
- **What corrupted the temple?** Hints of a greater darkness: perhaps the hollow sun is a symptom, not the cause.
- **Where does the nước mắm cốt come from?** Introduction to the shrine keeper NPC who blesses it.

### ACT 3 THEMES
- The player learns that they are also a ghost (or will become one).
- The final boss is revealed: not Cá Ông, but the **hollow sun itself** — a sentient, malevolent force.
- Multiple endings based on player's accumulated choices across all three acts.

---

## PART 11: REFERENCE & INSPIRATION

### VISUAL REFERENCES
- **Vietnamese shrine architecture:** Photos of pagodas and temples along the Quảng Ngãi coast.
- **Ghost design:** Studio Ghibli's "Spirited Away" (the spirits are humanoid, emotional, not just monsters).
- **Color and mood:** Autumn color palettes, desaturated tones, bioluminescence.
- **The hollow sun:** Ancient Vietnam bronze drums (Trống Đồng), their surface patterns and spiritual significance.

### NARRATIVE REFERENCES
- **"The Tale of Kieu" (Truyện Kiều):** Vietnamese epic about fate, longing, sacrifice, and duty.
- **"Spirited Away":** Themes of service, ritual, and the boundary between living and dead.
- **Folk horror traditions:** Japanese yokai tales, Korean shamanism, Vietnamese folk beliefs about water spirits.
- **Personal themes:** Memory, abandonment, the cost of survival, compassion as a choice.

---

## PART 12: PRODUCTION NOTES

### FOR ARTISTS:

1. **Asset Priority:**
   - Week 1: Player character (8 frames), Ghost character (6 frames).
   - Week 2: Enemy ghost, Shrine exterior (calm state).
   - Week 3: Stall details, Shrine interior, NPCs.
   - Week 4: Boss, parallax backgrounds, polish.

2. **Consistency:**
   - Establish a **line weight** and stick to it (all outlines should be consistent thickness).
   - Use a limited **color palette** per scene (no more than 8 colors per scene).
   - All ghosts should be **recognizable by silhouette** (important for visual clarity).

3. **Emotional Cues (Visual Language):**
   - **Warm colors (gold, orange, brown):** Memory, comfort, human warmth.
   - **Cool colors (blue, indigo, purple):** Sorrow, water, death, mystery.
   - **Flickering effects:** Emotional instability or corruption.
   - **Smooth animation:** Peace and acceptance.
   - **Jerky animation:** Distress or malevolence.

### FOR PROGRAMMERS:

1. **State Machine Hierarchy:**
   - Game State: Day Phase ↔ Night Phase
   - Day Phase: Order Queue Manager → Spirit Essence Counter → Customer Satisfaction
   - Night Phase: Movement Controller → Health System → Boss Encounter

2. **Data Structure (per ghost):**
   ```
   Ghost {
     name: string
     personalityTier: "happy" | "neutral" | "angry" | "corrupted"
     loreUnlocked: boolean
     timestampServed: array<int>
     satisfactionLevel: 0-100
     currentOrder: Order | null
   }
   ```

3. **Branching Logic:**
   - Track cumulative player kindness score.
   - At boss encounter: if (kindnessScore > 60) → Cá Ông; else → Chú Ba.
   - After each ending: save player choice to unlock alternate dialogue in Act 2.

---

## END OF NARRATIVE BIBLE

**Total Document:** ~8,000 words of story, character, and design detail.  
**Ready for:** Artists (visual reference), writers (dialogue scripts), programmers (mechanic descriptions).

**Next Steps:**
1. Artists begin concepting character designs.
2. Programmers set up core game loop.
3. Weekly check-ins on lore consistency.

**Questions?** Refer back to Parts 1-7 for story context, Parts 8-10 for visual/mechanical details.

---

*This document is a living design bible. It will evolve during production. Major changes should be documented in version notes.*

**Version 0.1** — Initial draft, game jam scope.
