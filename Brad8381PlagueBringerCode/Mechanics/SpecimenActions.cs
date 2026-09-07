using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;

public static class SpecimenActions
{
    public static int Count(Creature owner) => owner.GetPower<SpecimenPower>()?.Amount ?? 0;

    public static async Task<int> Gain(PlayerChoiceContext choiceContext, Creature owner, int amount, CardModel? source)
    {
        var current = Count(owner);
        var gain = Math.Clamp(amount, 0, SpecimenPower.MaxAmount - current);
        if (gain <= 0) return 0;

        await PowerCmd.Apply<SpecimenPower>(choiceContext, owner, gain, owner, source);
        return gain;
    }

    public static async Task<int> Spend(PlayerChoiceContext choiceContext, Creature owner, int amount, CardModel? source)
    {
        var power = owner.GetPower<SpecimenPower>();
        if (power == null || amount <= 0) return 0;

        var spent = Math.Min(amount, power.Amount);
        await PowerCmd.ModifyAmount(choiceContext, power, -spent, owner, source);
        return spent;
    }
}
