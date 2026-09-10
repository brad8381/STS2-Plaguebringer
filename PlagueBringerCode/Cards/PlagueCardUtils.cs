using PB.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Cards;

internal static class PlagueCardUtils
{
    public static int GetPlague(Creature creature) =>
        (int)(creature.GetPower<PlaguePower>()?.Amount ?? 0m);

    public static async Task<int> RemovePlague(
        PlayerChoiceContext choiceContext,
        Creature target,
        int amount,
        CardModel source)
    {
        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0 || amount <= 0)
            return 0;

        var removed = Math.Min(amount, (int)plague.Amount);

        if (removed >= plague.Amount)
            await PowerCmd.Remove(plague);
        else
            await PowerCmd.ModifyAmount(choiceContext, plague, -removed, source.Owner.Creature, source);

        await NotifyPlagueRemoved(choiceContext, source, removed);
        return removed;
    }

    public static async Task<int> RemoveAllPlague(
        PlayerChoiceContext choiceContext,
        Creature target,
        CardModel source)
    {
        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0)
            return 0;

        var removed = (int)plague.Amount;
        await PowerCmd.Remove(plague);
        await NotifyPlagueRemoved(choiceContext, source, removed);
        return removed;
    }

    public static async Task TriggerPlague(
        PlayerChoiceContext choiceContext,
        Creature target,
        int times = 1)
    {
        for (var i = 0; i < times && target.IsAlive; i++)
        {
            var plague = target.GetPower<PlaguePower>();
            if (plague == null || plague.Amount <= 0)
                break;

            await plague.TriggerPlague(choiceContext);
        }
    }

    public static async Task TriggerAllEnemies(
        PlayerChoiceContext choiceContext,
        ICombatState combatState,
        Creature owner,
        int times = 1)
    {
        foreach (var enemy in combatState.GetOpponentsOf(owner).Where(enemy => enemy.IsAlive).ToArray())
            await TriggerPlague(choiceContext, enemy, times);
    }

    public static async Task ApplyPlagueToAllEnemies(
        PlayerChoiceContext choiceContext,
        ICombatState combatState,
        Creature owner,
        int amount,
        CardModel? source)
    {
        if (amount <= 0)
            return;

        foreach (var enemy in combatState.GetOpponentsOf(owner).Where(enemy => enemy.IsAlive).ToArray())
            await PB.Mechanics.PlagueActions.Apply(choiceContext, enemy, amount, owner, source);
    }

    private static async Task NotifyPlagueRemoved(
        PlayerChoiceContext choiceContext,
        CardModel source,
        int removed)
    {
        if (removed <= 0)
            return;

        if (source.Owner.Creature.GetPower<SepticReservePower>() is { } reserve)
            await reserve.OnPlagueRemoved(choiceContext);
    }
}
