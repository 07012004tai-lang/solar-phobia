<p align="center">
  <a href="https://github.com/truong204/Solar-Phobia">
    <img src="images/logo.png" alt="Solar Phobia Logo" width="80" height="80">
  </a>

  <h3 align="center">Solar Phobia: Nắng Gắt</h3>

  <p align="center">
    A consequence-driven survival game set in a sun-scorched Vietnamese fishing village.
    <br />
    By day, you choose who to save. By night, you run from what you left behind.
    <br />
    <br />
    <a href="https://github.com/truong204/Solar-Phobia/issues/new?labels=bug&template=bug-report---.md">Report Bug</a>
    ·
    <a href="https://github.com/truong204/Solar-Phobia/issues/new?labels=enhancement&template=feature-request---.md">Request Feature</a>
  </p>
</p>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#gameplay">Gameplay</a></li>
    <li><a href="#roadmap">Roadmap</a></li>
    <li><a href="#license">License</a></li>
    <li><a href="#contact">Contact</a></li>
    <li><a href="#acknowledgments">Acknowledgments</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

[![Game Screenshot][product-screenshot]](https://example.com)

I wanted to make a game where your choices actually matter—not in some vague "good ending/bad ending" way, but immediate, gut-punch consequences you feel the same night.

Set in the fictional Làng Chài Hắc Hải, a fishing village cursed by "Nắng Gắt" (Scorching Sun), the game puts you in a tight day/night loop:

**By day**, you serve three villagers—Linh, Van, and Minh. You've got tea for light, incense for safe zones, offerings for skills. But you can only save two. One must be left behind.

**By night**, you flee to the shrine while the abandoned soul hunts you down. Each one becomes a different obstacle:
- Linh drags you down
- Van blocks your path
- Minh fakes shrines to waste your time

You don't lose because the game is hard. You lose because of what you chose.

Here's why I built it this way:
* I wanted players to feel the weight of every decision, not just see a different cutscene
* The day/night contrast lets me play with hot yellows vs. deep blues, calm ambience vs. escalating tension
* Keeping it small (3 NPCs, 1 boss, 1 map) means I can polish the hell out of that one loop

<p align="right">(<a href="#readme-top">back to top</a>)</p>

### Built With

This section lists the major technologies I'm using to build this.

* ![Unity](https://img.shields.io/badge/Unity%206000.3.11f1-100000?style=for-the-badge&logo=unity&logoColor=white)
* ![C#](https://img.shields.io/badge/C%23%209.0-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
* ![VContainer](https://img.shields.io/badge/VContainer-DI-blue?style=for-the-badge)
* ![UniTask](https://img.shields.io/badge/UniTask-Async-green?style=for-the-badge)
* ![DOTween](https://img.shields.io/badge/DOTween-Animation-orange?style=for-the-badge)

The code is split into clean layers (Domain → Application → Infrastructure → Presentation) with separate assembly definitions for each, so it stays organized as it grows.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

This is how to get a local copy up and running on your machine.

### Prerequisites

* [Unity 6000.3.11f1 (Unity 6)](https://unity.com/download) – make sure you have this specific version via Unity Hub
* [Git](https://git-scm.com/)

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/truong204/Solar-Phobia.git
   ```
2. Open Unity Hub and add the project folder
3. Open the project with Unity 6000.3.11f1

### Running Tests

Inside Unity Editor:
- Open **Test Runner**: `Window → General → Test Runner` (or Ctrl+Alt+T)
- Run EditMode tests from `Assets/_Project/**/Editor/Tests/`
- Run PlayMode tests from `Assets/_Project/**/Tests/`

Or via command line:
```sh
# EditMode tests
Unity.exe -runTests -projectPath "I:\unityVers\Solar phobia" -testPlatform EditMode -testResults results.xml

# PlayMode tests
Unity.exe -runTests -projectPath "I:\unityVers\Solar phobia" -testPlatform PlayMode -testResults results.xml
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GAMEPLAY -->
## Gameplay

The core loop is tight and repeatable:

1. **Day Phase (Serve)** – Talk to Linh, Van, and Minh. Give them tea, incense, or offerings. Choose which two to save.
2. **Choice Lock** – The one left behind is locked in. The game computes how they'll mess with you at night.
3. **Night Phase (Survive)** – Run to the shrine. Avoid solar residue hazards, manage your Ward Timer, and deal with the curse of the soul you abandoned.
4. **Resolve** – Made it to the shrine? You win that loop. Die? Face the consequence.

### The Map (Act 1)

The journey is split into 3 nodes, each with its own vibe:

| Node | Name | Vibe | Gameplay |
|------|------|------|----------|
| 1 | Am Đầu Sóng | Calmest area, plenty of shade | Tutorial: serving villagers, short runs |
| 2 | Am Rừng Dừa | Dead coconut trees, mobile shade | Introduces mobile shadow mechanics |
| 3 | Lăng Ông Nam Hải | Ruined shrine, cracked ground, hottest | Boss fight: Cá Ông Bộ Xương |

At each shrine, you can open your old map and plan your next night route—short/fast vs. long/safe with more loot.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ROADMAP -->
## Roadmap

- [x] Game concept and core loop design
- [x] NPC/Soul data model (Linh, Van, Minh)
- [x] Day/Night phase state machine design
- [ ] Implement day/night state machine
- [ ] Build consequence resolver (how abandoned souls become night hazards)
- [ ] Create boss chase AI for Cá Ông Bộ Xương
- [ ] Implement solar residue hazard system
- [ ] Write custom shadow/sun zone shader
- [ ] Build the 3-node map
- [ ] Add audio state director (day/night mix transitions)
- [ ] Polish HUD and diegetic feedback

See the [open issues](https://github.com/truong204/Solar-Phobia/issues) for a full list of proposed features and known issues.

### Cut Plan (if needed)
- Remove 1 NPC
- Simplify boss mechanics
- Reduce number of endings

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- LICENSE -->
## License

This project is proprietary. All rights reserved.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- CONTACT -->
## Contact

Your Name - [@your_twitter](https://twitter.com/your_username) - email@example.com

Project Link: [https://github.com/truong204/Solar-Phobia](https://github.com/truong204/Solar-Phobia)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- ACKNOWLEDGMENTS -->
## Acknowledgments

* [Unity Technologies](https://unity.com) for the engine
* [VContainer](https://github.com/vcontainer/vcontainer) for dependency injection
* [UniTask](https://github.com/Cysharp/UniTask) for async/await support
* [DOTween](http://dotween.demigiant.com/) for tween animations
* [Odin Inspector](https://odininspector.com/) for editor enhancements

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- MARKDOWN LINKS & IMAGES -->
<!-- https://www.markdownguide.org/basic-syntax/#reference-style-links -->
[product-screenshot]: images/screenshot.png
