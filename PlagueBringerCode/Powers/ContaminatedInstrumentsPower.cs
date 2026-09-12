using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Powers;

public sealed class ContaminatedInstrumentsPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Contaminated Instruments",
            "Whenever you Exhaust a card, apply Plague equal to Contaminated Instruments to ALL enemies.",
            "Whenever you Exhaust a card, apply [gold]{Amount} Plague[/gold] to ALL enemies."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (card.Owner.Creature != Owner || Owner.CombatState == null || Amount <= 0)
            return;

        Flash();
        foreach (var enemy in Owner.CombatState.GetOpponentsOf(Owner).Where(enemy => enemy.IsAlive).ToArray())
            await PB.Mechanics.PlagueActions.Apply(choiceContext, enemy, Amount, Owner, card);
    }
}
