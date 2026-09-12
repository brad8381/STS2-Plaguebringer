using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PB.Powers;

public sealed class SwayPower : PlagueBringerPower
{
    public const int MaxStacks = 5;
    public const int MaxReductionStacks = 5;

    private int EffectiveStacks => Math.Min(Amount, MaxStacks);

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Sway",
            "Each Sway reduces Attack damage by 12%. Attackers gain 1 Block per Sway when they hit this creature. Lose 1 Sway after its turn. Max 5.",
            "[red]Attack damage -{Amount:choose(1|2|3|4|5):12|24|36|48|60|60}%[/red] ([red]12% per Sway[/red]). Attackers gain [blue]{Amount} Block[/blue] when they hit this creature. Lose [gold]1 Sway[/gold] after its turn. Max [gold]5[/gold]."
        );

    public override string CustomPackedIconPath =>
        "res://PlagueBringer/images/powers/sway_power.png";

    public override string CustomBigIconPath =>
        "res://PlagueBringer/images/powers/big/sway_power.png";

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (target != Owner)
            return;

        if (dealer == null)
            return;

        if (dealer.Side == Owner.Side)
            return;

        if (!props.IsPoweredAttack())
            return;

        if (result.UnblockedDamage <= 0)
            return;

        var sway = EffectiveStacks;

        if (sway <= 0)
            return;

        Flash();

        await CreatureCmd.GainBlock(
            dealer,
            sway,
            ValueProp.Unpowered,
            null);
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
