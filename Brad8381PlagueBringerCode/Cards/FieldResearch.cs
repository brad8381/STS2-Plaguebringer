using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class FieldResearch : PlagueBringerCard
{
    public FieldResearch() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self)
    {
        WithVars(new DynamicVar("SpecimenPerExhaust", 1));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Field Research",
            "Whenever you Exhaust a card, gain {SpecimenPerExhaust} Specimen."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        await PowerCmd.Apply<FieldResearchPower>(
            choiceContext,
            Owner.Creature,
            DynamicVars["SpecimenPerExhaust"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
