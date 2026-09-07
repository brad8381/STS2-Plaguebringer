using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class ContaminatedInstrumentsPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Contaminated Instruments",
            "Whenever you Exhaust a card, apply {Amount} Plague to ALL enemies.",
            "Whenever you Exhaust a card, apply {Amount} Plague to ALL enemies."
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
            await PowerCmd.Apply<PlaguePower>(choiceContext, enemy, Amount, Owner, card);
    }
}
