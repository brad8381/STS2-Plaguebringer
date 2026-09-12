using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Mechanics;

public static class SwayActions
{
    public static int Count(Creature target) =>
        Math.Min(target.GetPower<SwayPower>()?.Amount ?? 0, SwayPower.MaxStacks);

    public static async Task<int> Apply(
        PlayerChoiceContext choiceContext,
        Creature target,
        int amount,
        Creature? applier,
        CardModel? source)
    {
        var current = Count(target);
        var toApply = Math.Clamp(amount, 0, SwayPower.MaxStacks - current);
        if (toApply <= 0)
            return 0;

        var power = await PowerCmd.Apply<SwayPower>(
            choiceContext,
            target,
            toApply,
            applier,
            source);

        if (power == null)
            return 0;

        if (power.Amount > SwayPower.MaxStacks)
        {
            await PowerCmd.ModifyAmount(
                choiceContext,
                power,
                SwayPower.MaxStacks - power.Amount,
                applier,
                source);
        }

        // Off Balance is the temporary part of Sway. While it is active,
        // attackers gain Block equal to the target's current Sway.
        // Keep it synchronized instead of stacking the total repeatedly
        // when multiple Sway applications happen during the same turn.
        var desiredOffBalance = Math.Min(power.Amount, SwayPower.MaxStacks);
        var offBalance = target.GetPower<OffBalancePower>();

        if (offBalance == null)
        {
            await PowerCmd.Apply<OffBalancePower>(
                choiceContext,
                target,
                desiredOffBalance,
                applier,
                source);
        }
        else
        {
            var difference = desiredOffBalance - offBalance.Amount;
            if (difference != 0)
            {
                await PowerCmd.ModifyAmount(
                    choiceContext,
                    offBalance,
                    difference,
                    applier,
                    source);
            }
        }

        return toApply;
    }
}
