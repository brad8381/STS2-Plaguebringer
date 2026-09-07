using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Potions;

public sealed class QuarantineDraught : PlagueBringerPotion
{
    public override PotionRarity Rarity => PotionRarity.Common;
    public override PotionUsage Usage => PotionUsage.CombatOnly;
    public override TargetType TargetType => TargetType.AnyEnemy;

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new DynamicVar("BlockPerPlague", 3)];

    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        if (target == null) return;

        var plague = target.GetPower<PlaguePower>()?.Amount ?? 0;
        var block = plague * DynamicVars["BlockPerPlague"].IntValue;
        if (block <= 0) return;

        await CreatureCmd.GainBlock(
            Owner.Creature,
            block,
            ValueProp.Unpowered,
            null);
    }
}
