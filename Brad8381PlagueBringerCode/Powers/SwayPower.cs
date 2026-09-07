using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class SwayPower : PlagueBringerPower
{
    public const int MaxEffectiveStacks = 3;

    public override string CustomPackedIconPath => "res://Brad8381PlagueBringer/images/powers/sway_power.svg";
    public override string CustomBigIconPath => "res://Brad8381PlagueBringer/images/powers/big/sway_power.svg";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;
    public override int DisplayAmount => Math.Min(Amount, MaxEffectiveStacks);

    public override decimal ModifyDamageMultiplicative(Creature? target, decimal amount, ValueProp props,
        Creature? dealer, CardModel? cardSource)
    {
        if (dealer != Owner || !props.IsPoweredAttack()) return 1m;
        var stacks = Math.Min(Amount, MaxEffectiveStacks);
        return 1m - 0.10m * stacks;
    }

    public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
    {
        if (side == CombatSide.Enemy && Owner.Side == CombatSide.Enemy)
            await PowerCmd.TickDownDuration(this);
    }
}
