using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Mechanics;

public static class SpecimenActions
{
    public static int Count(Creature owner) =>
        owner.GetPower<SpecimenPower>()?.Amount ?? 0;

    public static async Task<int> Gain(
        PlayerChoiceContext choiceContext,
        Creature owner,
        int amount,
        CardModel? source)
    {
        if (amount <= 0)
            return 0;

        var current = Count(owner);
        var gain = Math.Min(amount, SpecimenPower.MaxAmount - current);

        if (gain <= 0)
            return 0;

        await PowerCmd.Apply<SpecimenPower>(
            choiceContext,
            owner,
            gain,
            owner,
            source);

        return gain;
    }

    public static async Task<int> Spend(
        PlayerChoiceContext choiceContext,
        Creature owner,
        int amount,
        CardModel? source)
    {
        if (amount <= 0)
            return 0;

        var power = owner.GetPower<SpecimenPower>();

        if (power == null)
        {
            await PowerCmd.Apply<SpecimenPower>(
                choiceContext,
                owner,
                -amount,
                owner,
                source);

            return amount;
        }

        await PowerCmd.ModifyAmount(
            choiceContext,
            power,
            -amount,
            owner,
            source);

        return amount;
    }
}