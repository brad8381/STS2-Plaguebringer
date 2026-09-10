# The Plaguebringer

> Created with help from AI while I learn mod development.

A plague doctor character for Slay the Spire 2. Small wounds build into a sickness that grows stronger each turn.

**Status:** early source prototype. No playable download is available yet. Compilation, in-game testing and final artwork are still pending.

## Features

- **Plague:** deals damage equal to its stacks at the start of an affected enemy's turn, ignoring Block, then gains 1 stack.
- **Sealed Censer:** applies 1 Plague to all enemies at the start of combat.
- **Open Censer:** the starter-relic upgrade also lets you choose one of up to three Plague cards from your draw pile after the opening draw.
- Six prototype card types, a ten-card starting deck and a black Plague status icon.

Character, card and relic artwork currently uses template placeholders.

## Requirements

- Slay the Spire 2, standard Steam branch.
- BaseLib **3.3.8**.

Compatibility with the current game build and other mods is not yet verified.

## Downloads

There is no compiled `.dll` or `.pck` in this draft. GitHub's **Download ZIP** contains source code, not an installable mod.

Tested packages will be attached to **GitHub Releases** and uploaded to Nexus Mods. The game package will contain the mod's `.dll`, `.pck` and `.json`.

## Development

[Build and debugging guide](docs/DEVELOPMENT.md) · [Test checklist](docs/TESTING.md) · [Release process](docs/RELEASING.md)

## Support

The mod will remain free. [Optional support via PayPal](https://paypal.me/brad8381) is appreciated and does not unlock content or access.

## Credits

Built on [BaseLib](https://github.com/Alchyr/BaseLib-StS2) using [Alchyr's character template](https://github.com/Alchyr/ModTemplate-StS2) and its placeholder assets.

Unofficial fan mod; not affiliated with Mega Crit.
