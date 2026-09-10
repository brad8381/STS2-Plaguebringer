using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace PB.Potions;

public sealed class QuarantineDraught : PlagueBringerPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.Self;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("BlockPerPlague", 1)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        var combatState = Owner.Creature.CombatState;
        if (combatState == null)
            return;

        var totalPlague = combatState
            .GetOpponentsOf(Owner.Creature)
            .Where(enemy => enemy.IsAlive)
            .Sum(enemy => Math.Max(0, enemy.GetPower<PlaguePower>()?.Amount ?? 0));

        var block = totalPlague * DynamicVars["BlockPerPlague"].IntValue;
        if (block <= 0)
            return;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            block,
            ValueProp.Unpowered,
            null);
    }
}
