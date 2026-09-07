using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class PlaguePower : PlagueBringerPower
{
    public override string CustomPackedIconPath => "res://Brad8381PlagueBringer/images/powers/plague_power.png";
    public override string CustomBigIconPath => "res://Brad8381PlagueBringer/images/powers/big/plague_power.png";

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Plague",
            "At the start of this creature's turn, take {Amount} damage that ignores Block, then increase Plague by 15%, rounded up.",
            "At the start of this creature's turn, take {Amount} damage that ignores Block, then increase Plague by 15%, rounded up."
        );

    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterSideTurnStart(
        CombatSide side,
        IReadOnlyList<Creature> participants,
        ICombatState combatState)
    {
        if (!participants.Contains(Owner)) return;
        await TriggerPlague(new ThrowingPlayerChoiceContext());
    }

    public async Task TriggerPlague(PlayerChoiceContext choiceContext)
    {
        if (!Owner.IsAlive || Amount <= 0)
            return;

        var stacks = Amount;
        MainFile.Logger.Debug($"Plague tick: target={Owner}, stacks={stacks}");

        await CreatureCmd.Damage(
            choiceContext,
            Owner,
            stacks,
            ValueProp.Unpowered | ValueProp.Unblockable,
            null,
            null);

        if (!Owner.IsAlive)
        {
            MainFile.Logger.Debug("Plague lethal tick complete; returning to combat flow.");
            return;
        }

        if (Owner.GetPower<PlaguePower>() != this)
            return;

        var baseNextAmount = Math.Ceiling(stacks * 1.15m);
        var bonusGrowthPercent = Owner.CombatState?
            .GetOpponentsOf(Owner)
            .Where(opponent => opponent.IsAlive)
            .Sum(opponent => opponent.GetPower<NoCurePower>()?.Amount ?? 0m) ?? 0m;

        var noCureBonus = bonusGrowthPercent > 0
            ? Math.Ceiling(stacks * bonusGrowthPercent / 100m)
            : 0m;

        var increase = baseNextAmount - stacks + noCureBonus;

        MainFile.Logger.Debug(
            $"Plague growth: {stacks} -> {stacks + increase} (+{increase}, No Cure {bonusGrowthPercent}% = +{noCureBonus})");

        if (increase > 0)
        {
            await PowerCmd.ModifyAmount(
                choiceContext,
                this,
                increase,
                null,
                null);
        }
    }
}
