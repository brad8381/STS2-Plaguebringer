using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using PB.Powers;

namespace PB.Mechanics;

public static class PlagueActions
{
    public static async Task Apply(
        PlayerChoiceContext choiceContext,
        Creature target,
        int amount,
        Creature applier,
        CardModel? source)
    {
        if (amount <= 0 || !target.IsAlive)
            return;

        await PowerCmd.Apply<PlaguePower>(
            choiceContext,
            target,
            amount,
            applier,
            source);

        // Cross Immunity only triggers when applying Plague to an enemy.
        if (target.Side == applier.Side)
            return;

        var power = applier.GetPower<CrossImmunityPower>();

        if (power == null || !power.CanTrigger)
            return;

        var applied = await SwayActions.Apply(
            choiceContext,
            target,
            1,
            applier,
            source);

        if (applied > 0)
        {
            power.ConsumeTrigger();

        }
    }
}


