# Changelog

## v0.1.1

### Mechanics and balance
- Reworked **Sway** to reduce Attack damage by **10% per stack**, capped at **5 stacks / 50%**.
- Split Sway's temporary Block reward into a separate **Off Balance** debuff. Applying Sway also applies matching Off Balance for the current turn; attackers gain Block equal to Off Balance when they deal unblocked Attack damage to that creature.
- Moved **Pestilent Blow** to Rare. It now multiplies Plague by **1.5x** normally and **3x** when upgraded, with damage increasing from 6 to 9 on upgrade.
- Changed **Black Tonic** to cost **0 Energy**, apply 1 Plague to yourself, gain **2 Energy (3 upgraded)**, retain unspent Energy into the next turn and Exhaust.
- Changed **Plague Vial+** to cost **0 Energy** while remaining at **4 Plague**.

### Fixes and UI
- Fixed **Contagion Engine** so its Plague application occurs at the **start of your turn** as described.
- Fixed Contagion Engine's power tooltip to display the actual live Plague amount.
- Fixed **Cross Immunity** localization and upgrade text.
- Standardized custom power tooltips so live values and important effects are easier to read.
- Updated Sway's tooltip to show the current damage reduction and explain that applying Sway also applies Off Balance.
- Fixed Plaguebringer merchant/shop character sizing.

### Packaging and release
- Reduced the exported PCK size by excluding source artwork, source code, documentation and debug symbols from the Godot export.
- Bumped the mod and assembly version to **0.1.1**.

## v0.1.0

- Initial Steam Workshop release of The Plaguebringer.
