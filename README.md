# ⚔️ CardGame - Unity 2D Card Battle Game

A text-free 2D card battle game developed in Unity as part of an undergraduate thesis (TCC). All gameplay feedback is communicated through animations, icons, and visual effects — no text required during combat.

---

## 🎮 About the Game

Players select cards from their hand to battle against an NPC opponent. Each card type has unique interactions, creating a strategic rock-paper-scissors-style combat system. The game features a progressive tutorial across multiple levels, teaching mechanics through visual feedback alone.

---

## 🃏 Card Types

| Card | Effect |
|------|--------|
| ⚔️ **ESPADA** (Sword) | Deals attack damage |
| 🛡️ **ESCUDO** (Shield) | Blocks attacks, generates mana |
| 🔮 **PODER** (Power) | Uses mana to deal magic damage |
| 🤚 **GRAB** | Reflects power attacks |
| 🧘 **MEDITAÇÃO** (Meditation) | Restores HP |
| 💀 **PODER NEGRO** (Dark Power) | Weaker power used when mana is 0 |


![Screenshot do jogo](Media/Captura%20de%20tela%202026-06-04%20100047.png)

---

## ✨ Features

### Gameplay
- 5 card types with unique interactions
- Progressive tutorial (3 → 4 → 5 card slots)
- Configurable victory conditions per level (`mustKillToWin`)
- Configurable HP and damage per level
- Undo system — return cards to hand before confirming selection
- Independent slot system — no auto-reorganization

### Visual Feedback
- Combat animations with dark overlay
- HP shake + damage number animation
- Hero/NPC damage frame swap on hit
- NPC cards slide in from the right at round start
- Player card hover effect with placeholder
- Glow effect on selectable cards
- Scale pulse on action buttons
- Hero tail fire animation after intro
- NPC frame animation during intro sequence

### UI Systems
- Real-time HP and Mana display
- Undo icon appears on slot hover
- Fight button appears only when all cards are selected
- Play button shown only on first run (auto-skipped on retry)

---

## 🗂️ Project Structure

```
Assets/
├── Scripts/
│   ├── BattleController.cs       # Battle flow, animations, buttons
│   ├── BattleManager.cs          # Game state, cards, slots
│   ├── BattleIntroController.cs  # Intro sequence, overlays, glow
│   ├── HeroUI.cs                 # HP, mana, shake, damage frame
│   ├── CombatResolver.cs         # Combat logic
│   ├── CardUI.cs                 # Card click handler
│   ├── CardHover.cs              # Hover effect on cards
│   ├── SelectionSlotUI.cs        # Selection slot management
│   ├── UndoButton.cs             # Undo button per slot
│   ├── SlotHoverArea.cs          # Slot hover detection
│   ├── PlayerGlowEffect.cs       # Glow on player cards
│   ├── ScalePulseEffect.cs       # Scale pulse on buttons
│   ├── Hero.cs                   # Hero ScriptableObject
│   ├── Card.cs                   # Card ScriptableObject
│   └── LevelData.cs              # Level configuration
├── Sprites/
│   ├── Animations/               # Combat animation frames
│   ├── Cards/                    # Card sprites
│   ├── Icons/                    # UI icons
│   └── Hero/                     # Hero sprites
├── Scenes/
│   ├── Level1_Tutorial.unity
│   ├── Level2_Tutorial.unity
│   └── Level3_Tutorial.unity
└── CardData/                     # ScriptableObject assets
    ├── Hero1, Hero2
    ├── Level1, Level2, Level3
    └── Card assets
```

---

## ⚙️ Level Configuration (LevelData)

Each level is configured via ScriptableObject:

```
numberOfSlots     → Number of card slots (3/4/5)
playerStartHP     → Player starting HP
npcStartHP        → NPC starting HP
mustKillToWin     → true = must zero HP to win | false = most damage wins
nextSceneName     → Name of the next scene to load
shuffleNPC        → false for tutorial (fixed NPC cards)
npcCards          → NPC card sequence
```

---

## 🛠️ Built With

- **Unity 2022.3.62f3**
- **C#**
- **TextMeshPro**
- **Unity UI (Canvas)**

---

## 🚀 How to Run

1. Clone the repository
2. Open the project in **Unity 2022.3 LTS**
3. Open `Assets/Scenes/Level1_Tutorial.unity`
4. Press **Play** in the Unity Editor

### Build
1. File → Build Settings
2. Add all scenes in order
3. Platform: PC, Mac & Linux Standalone
4. Build and Run

---

## 🎓 Academic Context

This project was developed as part of an undergraduate thesis (TCC) at a Brazilian university. The core research question explores how card game mechanics can be taught and communicated through visual feedback alone, without relying on text-based instructions.

---

## 👤 Author

**Luiz Amorim**  
Undergraduate Thesis Project — 2026S
