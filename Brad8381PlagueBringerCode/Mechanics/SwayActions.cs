using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;

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

        return toApply;
    }
}
