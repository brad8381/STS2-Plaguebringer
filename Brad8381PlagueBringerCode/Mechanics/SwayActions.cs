using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;

public static class SwayActions
{
    public static int Count(Creature target)
    {
        return Math.Min(target.GetPower<SwayPower>()?.Amount ?? 0, SwayPower.MaxEffectiveStacks);
    }

    public static async Task<int> Apply(
        PlayerChoiceContext choiceContext,
        Creature target,
        int amount,
        Creature? applier,
        CardModel? source)
    {
        int current = Count(target);
        int toApply = Math.Clamp(amount, 0, SwayPower.MaxEffectiveStacks - current);
        if (toApply <= 0)
            return 0;

        SwayPower? power = await PowerCmd.Apply<SwayPower>(
            choiceContext,
            target,
            toApply,
            applier,
            source);

        if (power == null)
            return 0;

        if (power.Amount > SwayPower.MaxEffectiveStacks)
        {
            await PowerCmd.ModifyAmount(
                choiceContext,
                power,
                SwayPower.MaxEffectiveStacks - power.Amount,
                applier,
                source);
        }

        return Math.Min(toApply, SwayPower.MaxEffectiveStacks - current);
    }
}
