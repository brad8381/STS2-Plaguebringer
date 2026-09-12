using BaseLib.Abstracts;
using PB.Mechanics;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace PB.Powers;

public sealed class FieldResearchPower : PlagueBringerPower
{
    public override List<(string, string)>? Localization =>
        new PowerLoc(
            "Field Research",
            "Whenever you Exhaust a card, gain Specimen equal to Field Research.",
            "Whenever you Exhaust a card, gain [gold]{Amount} Specimen[/gold]."
        );

    public override string CustomPackedIconPath =>
        "res://PlagueBringer/images/powers/field_research_power.png";
    public override string CustomBigIconPath =>
        "res://PlagueBringer/images/powers/big/field_research_power.png";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(
        PlayerChoiceContext choiceContext,
        CardModel card,
        bool causedByEthereal)
    {
        if (card.Owner.Creature != Owner || Amount <= 0)
            return;

        Flash();
        await SpecimenActions.Gain(choiceContext, Owner, Amount, null);
    }
}

