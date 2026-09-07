using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

public sealed class FieldResearchPower : PlagueBringerPower
{
    public override string CustomPackedIconPath => "res://Brad8381PlagueBringer/images/powers/field_research_power.svg";
    public override string CustomBigIconPath => "res://Brad8381PlagueBringer/images/powers/big/field_research_power.svg";

    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card.Owner.Creature != Owner) return;
        Flash();
        await SpecimenActions.Gain(choiceContext, Owner, Amount, null);
    }
}
