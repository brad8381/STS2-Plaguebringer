using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class CatalyticStrainPower : PlagueBringerPower
{
    private bool _resolving;

    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Catalytic Strain",
            "Whenever you apply Plague to an enemy, trigger its Plague, then add 10% of its current Plague, rounded up.",
            "Whenever you apply Plague to an enemy, trigger its Plague, then add 10% of its current Plague, rounded up."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override async Task AfterPowerAmountChanged(
        PlayerChoiceContext choiceContext,
        PowerModel power,
        decimal amount,
        Creature? applier,
        CardModel? cardSource)
    {
        if (_resolving || amount <= 0m || applier != Owner)
            return;

        if (power is not PlaguePower plague)
            return;

        var target = plague.Owner;
        if (!target.IsAlive || target.Side == Owner.Side)
            return;

        _resolving = true;
        try
        {
            Flash();
            await plague.TriggerPlague(choiceContext);

            if (!target.IsAlive)
                return;

            plague = target.GetPower<PlaguePower>();
            if (plague == null || plague.Amount <= 0)
                return;

            var bonus = (int)Math.Ceiling(plague.Amount * 0.10m);
            if (bonus <= 0)
                return;

            // This is growth caused by Catalytic Strain, not a fresh application,
            // so leave applier null to avoid recursively retriggering apply effects.
            await PowerCmd.ModifyAmount(
                choiceContext,
                plague,
                bonus,
                null,
                cardSource);
        }
        finally
        {
            _resolving = false;
        }
    }
}
