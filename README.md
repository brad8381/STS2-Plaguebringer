# The Plaguebringer

Plague doctor character mod for Slay the Spire 2, built on BaseLib.

**Draft source prototype, not a playable release.** Compilation and in-game testing are outstanding. Character/card/relic art uses the upstream template's placeholders; the black Plague status icon is a simple SVG. No finished plague doctor or horse artwork is included yet.

## Prototype mechanics

- 70 starting HP; 4 Strikes, 4 Defends, Infected Scalpel and Plague Vial.
- Plague: at the start of an affected enemy's turn, deal its stack count as unpowered damage that ignores Block, then add 1 stack if the enemy and power survive. Reapplication adds stacks. It is a separate power from Poison.
- Sealed Censer: before the owner's first hand draw, apply 1 Plague to every living opponent.
- Open Censer: replaces Sealed Censer through BaseLib's starter-relic upgrade hook. Retains its Plague effect; on the owner's first turn, after the opening draw, choose one of the first three eligible cards in the draw pile and move it into the hand.
- Eligible cards implement `IPlagueCard`. The choice uses pile order and the game's multiplayer choice context, with no independent random generator. Zero eligible cards skips the choice; one or two shows only those cards. It moves an existing card, not a generated copy or normal draw.

| Card | Cost | Base effect | Upgrade |
|---|---:|---|---|
| Strike | 1 | 5 damage | 8 damage |
| Defend | 1 | 5 Block | 8 Block |
| Infected Scalpel | 1 | 3 damage; apply 1 Plague | 5 damage |
| Plague Vial | 1 | Apply 2 Plague | Apply 3 |
| Fumigate | 1 | Apply 1 Plague to all enemies; Exhaust | Apply 2 |
| Quarantine | 1 | 8 Block | 11 Block |

This is a small mechanics test set, not a complete reward pool or balanced character. Only Fumigate and Quarantine currently fill normal reward slots.

## Build on Windows

1. Install the .NET 9 SDK and the upstream-recommended MegaDot/Godot 4.5.1 .NET build. See the [BaseLib setup guide](https://github.com/Alchyr/ModTemplate-StS2/wiki/Setup).
2. Copy `local.props.example` to `local.props`; enter your actual game and editor paths. The local file is ignored by Git.
3. Install BaseLib 3.3.8 in the game for this draft. The project pins that dependency; confirm it matches the game branch before testing. The manifest's minimum game version comes from the template and is not a verified compatibility claim.
4. Run `dotnet build Brad8381PlagueBringer.csproj -c Debug`. This compiles without copying output into the game.
5. To install and export the resource pack, run `dotnet publish Brad8381PlagueBringer.csproj -c Debug -p:InstallToGame=true`. This writes the DLL, PDB, JSON and PCK under the game's `mods/Brad8381PlagueBringer` directory. Godot export failures must stop the build.

Do not upload game DLLs, exported game assets, `local.props`, `.godot`, or build output. Keep the PDB locally for readable exception traces.

## Compatibility and diagnostics

Compatibility is unverified. The reported setup includes Extra Multiplayer; a later mods-folder screenshot also shows BetterSpire2, Damage Meter, MoreCardRewards, NoAscendersBane and RouteSuggest, plus BaseLib. Versions and whether every folder is actively loaded still need confirming. First test with BaseLib alone, then add Extra Multiplayer using identical game and mod versions on each client. Test both one and multiple Plaguebringers: enemy Plague must grow once per enemy turn, not once per player. Relic application is per relic owner, so two owners initially apply two stacks.

All content uses the `Brad8381PlagueBringer` root namespace. BaseLib prefixes model IDs from it; do not change it after release without save migration. No global Harmony patches are installed by this mod.

Search `godot.log` for `Brad8381PlagueBringer`. Startup logs identify the assembly and BaseLib versions. Debug logs record Plague ticks and relic selections. Report the first exception with its full stack trace, game build/branch, BaseLib and Extra Multiplayer versions, mod commit, player count and steps. See [the test checklist](docs/TESTING.md).

## Art direction

A black-coated plague doctor, pale beaked mask, worn brass medical tools and restrained charcoal effects. Clear silhouettes and painted shading, with a pale outline around black status effects. A pale horse is a concept option for selection art; mounted combat is not implemented. Final art requires review before replacement of placeholders.

## Support

The mod will remain free. If you would like to support development, you can [send an optional tip via PayPal](https://paypal.me/brad8381). Support is voluntary and does not unlock content or access.

Mega Crit [permits donations for modding work](https://www.megacrit.com/content-policy/), but does not permit paid access, paid mod content or stretch-goal-based mod fundraising. This is an unofficial fan mod and is not affiliated with Mega Crit.

## Credits

Project layout and placeholder assets: [Alchyr's character template](https://github.com/Alchyr/ModTemplate-StS2), commit `55ca2c606e6c78dd39689a5cf979b243a49652e7`.

Dependency: [BaseLib](https://github.com/Alchyr/BaseLib-StS2), MIT licensed. Hook and command usage was checked against BaseLib, [Oddmelt](https://github.com/Alchyr/Oddmelt) and [Watcher](https://github.com/lamali292/WatcherMod) source; neither character mod is bundled or used as this mod's base. A distribution license for this project's original work has not yet been chosen.
