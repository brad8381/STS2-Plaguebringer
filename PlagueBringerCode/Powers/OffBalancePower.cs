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

public sealed class OffBalancePower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Off Balance",
            "This turn, attackers gain 1 Block per Off Balance when they deal unblocked Attack damage to this creature.",
            "This turn, attackers gain [blue]{Amount} Block[/blue] when they deal unblocked Attack damage to this creature."
        );

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
        if (target != Owner || dealer == null)
            return;

        if (dealer.Side == Owner.Side || !props.IsPoweredAttack())
            return;

        if (result.UnblockedDamage <= 0 || Amount <= 0)
            return;

        Flash();

        await CreatureCmd.GainBlock(
            dealer,
            Amount,
            ValueProp.Unpowered,
            null);
    }

    public override async Task AfterSideTurnEnd(
        PlayerChoiceContext choiceContext,
        CombatSide side,
        IEnumerable<Creature> participants)
    {
        // Off Balance belongs to the opposing side's current turn.
        // Sway is normally applied to enemies during the player's turn,
        // so remove it as soon as that opposing turn ends.
        if (side == Owner.Side)
            return;

        await PowerCmd.Remove(this);
    }
}
