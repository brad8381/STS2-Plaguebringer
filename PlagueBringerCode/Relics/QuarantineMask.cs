using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace PB.Relics;

public sealed class QuarantineMask : PlagueBringerRelic
{
    public override List<(string, string)>? Localization =>
        new RelicLoc(
            "Quarantine Mask",
            "At the start of combat, gain 6 [gold]Block[/gold].",
            "Protection is mostly ritual."
        );

    public override RelicRarity Rarity => RelicRarity.Common;

    public override async Task BeforeHandDraw(Player player, PlayerChoiceContext choiceContext, ICombatState combatState)
    {
        if (player != Owner || Owner.PlayerCombatState is not { TurnNumber: 1 }) return;
        Flash();
        await CreatureCmd.GainBlock(Owner.Creature, 6, ValueProp.Unpowered, null);
    }
}
