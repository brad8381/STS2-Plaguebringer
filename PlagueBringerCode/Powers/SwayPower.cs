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
    public const int MaxStacks = 6;
    public const int MaxReductionStacks = 6;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Sway",
            "Each stack reduces this creature's Attack damage by 10%, up to 60%. When this creature is hit by an Attack, the attacker gains 1 Block per Sway. Maximum 6 Sway. Lose 1 Sway after this creature's turn.",
            "Each stack reduces this creature's Attack damage by 10%, up to 60%. When this creature is hit by an Attack, the attacker gains 1 Block per Sway. Maximum 6 Sway. Lose 1 Sway after this creature's turn."
        );

    public override string CustomPackedIconPath =>
        "res://PlagueBringer/images/powers/sway_power.png";

    public override string CustomBigIconPath =>
        "res://PlagueBringer/images/powers/big/sway_power.png";

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

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        // Only trigger when this Sway-covered creature is the target.
        if (target != Owner)
            return;

        // Must have an attacker.
        if (dealer == null)
            return;

        // Don't trigger from allies/self damage.
        if (dealer.Side == Owner.Side)
            return;

        // Only actual Attack damage.
        // Prevents Plague and other indirect damage from generating Block.
        if (!props.IsPoweredAttack())
            return;

        // Must actually deal HP damage.
        if (result.UnblockedDamage <= 0)
            return;

        var sway = Math.Min(Amount, MaxStacks);

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