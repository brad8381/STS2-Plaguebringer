# Prototype acceptance checks

Status: all in-game checks below are pending. Static file validation is not evidence of compilation or multiplayer compatibility.

## Environment

Record the game version and Steam branch, BaseLib version, Extra Multiplayer link/version, mod commit, OS and player count. Test a fresh modded run and keep existing saves separate.

## Build and assets

- Restore the pinned BaseLib package and compile against the installed `sts2.dll`; fix any signature mismatches before gameplay testing.
- Export with the matching Godot build. Verify DLL, PDB, JSON and PCK are present and the game loads the mod.
- Confirm character selection works, card names/descriptions render and the black Plague icon is readable.
- Verify the opening hand, ten-card deck, 70 HP and correct relic. Confirm default energy from the placeholder character is suitable.

## Plague

- Apply one stack: successive enemy turns deal 1, 2, 3 damage and leave 2, 3, 4 stacks.
- Enemy Block does not absorb Plague; attacker Strength/Weak do not change its unpowered damage.
- Applying two more stacks adds two rather than replacing the current amount.
- Verify standard debuff prevention/removal behavior and interactions with damage modifiers such as Intangible.
- Lethal damage does not add another stack or act on a removed creature.
- No tick on player turns or for enemies absent from that turn's participants.
- Save/load and combat end do not duplicate, leak or preserve combat-only state incorrectly.

## Relics and card choice

- Starter applies once before the first draw, never on later turns or later card draws.
- Upgrade encounter offers Open Censer and replaces Sealed Censer rather than retaining both.
- Upgraded selection happens after the opening hand. Confirm `AfterPlayerTurnStart` has that ordering on the installed build.
- With 0/1/2/3/4 eligible draw-pile cards, offer 0/1/2/3/3 cards in pile order.
- No card can be selected from the hand, discard, exhaust pile or permanent deck.
- The selected card retains its upgrade and instance state; only it leaves the draw pile. Remaining order is unchanged.
- Test a full hand and other first-turn draw relics: confirm the game's normal hand-limit behavior; do not silently delete a card.
- Reconnect or save/load around selection must not reopen or duplicate the reward.

## Additional installed mods

The supplied folder screenshot shows BetterSpire2, Damage Meter, MoreCardRewards, NoAscendersBane and RouteSuggest, alongside BaseLib. Extra Multiplayer was reported separately. Folder names are not sufficient to establish versions or compatibility.

- Record each manifest version and exact mod source. Verify which mods actually load.
- Add each mod individually after the BaseLib-only baseline passes, then test the complete set.
- Check card reward generation and selection, damage reporting for Plague, run-start changes, route UI and any overlapping combat hooks.
- If a failure appears, retain the first exception and isolate the smallest mod combination that reproduces it.

## Multiplayer

- Run with BaseLib alone, then BaseLib plus Extra Multiplayer, on host and client.
- Test one and two Plaguebringers, then more than four players when supported by the installed multiplayer mod.
- Both relic owners apply their own stack, but a shared enemy power ticks and grows only once per enemy turn.
- Only the relic owner chooses a card. Every peer receives the same choice and pile state.
- Compare enemy HP/stacks and selected card after a reconnect and after replaying the same actions.

## Balance and art follow-up

- Combat-start Plague on all enemies is powerful in long fights. Record encounter duration and damage contribution before increasing card power.
- Expand the reward pool after the core loop passes; two common reward cards are insufficient for a release.
- Replace all template placeholders after reviewing the plague doctor design and optional horse concept.
