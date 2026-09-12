using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace PB.Powers;

public sealed class VirulentCutsPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Virulent Cuts",
            "Whenever you deal unblocked Attack damage, apply Plague equal to Virulent Cuts.",
            "Whenever you deal unblocked Attack damage, apply [gold]{Amount} Plague[/gold]."
        );

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterDamageGiven(
        PlayerChoiceContext choiceContext,
        Creature? dealer,
        DamageResult result,
        ValueProp props,
        Creature target,
        CardModel? cardSource)
    {
        if (dealer == Owner && props.IsPoweredAttack() && result.UnblockedDamage > 0 && target.IsAlive)
            await PB.Mechanics.PlagueActions.Apply(choiceContext, target, Amount, Owner, cardSource);
    }
}
