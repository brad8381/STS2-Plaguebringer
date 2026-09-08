using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class SwayPower : PlagueBringerPower
{
    public const int MaxStacks = 6;
    public const int MaxReductionStacks = 5;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Sway",
            "Each stack reduces this creature's Attack damage by 10%, up to 50%. Maximum 6 Sway. Lose 1 Sway after this creature's turn.",
            "Each stack reduces this creature's Attack damage by 10%, up to 50%. Maximum 6 Sway. Lose 1 Sway after this creature's turn."
        );

    public override string CustomPackedIconPath =>
        "res://Brad8381PlagueBringer/images/powers/sway_power.png";
    public override string CustomBigIconPath =>
        "res://Brad8381PlagueBringer/images/powers/big/sway_power.png";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override decimal ModifyDamageMultiplicative(
        Creature? target,
        decimal amount,
        ValueProp props,
        Creature? dealer,
        CardModel? cardSource,
        CardPlay? cardPlay)
    {
        if (dealer != Owner || !props.IsPoweredAttack())
            return 1m;

        var effectiveStacks = Math.Min(Amount, MaxReductionStacks);
        return 1m - (0.10m * effectiveStacks);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        if (side != Owner.Side || !participants.Contains(Owner))
            return;

        await PowerCmd.TickDownDuration(this);
    }
}

