using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Potions;

public sealed class IncubationSerum : PlagueBringerPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target is not { IsAlive: true })
            return;

        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0)
            return;

        await PowerCmd.ModifyAmount(
            choiceContext,
            plague,
            plague.Amount,
            Owner.Creature,
            null);
    }
}
