using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class BloodSample : PlagueBringerCard
{
    public BloodSample() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVars(new DynamicVar("Specimen", 1), new CardsVar(1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var selected = (await CardSelectCmd.FromHand(
            prefs: new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1),
            context: choiceContext,
            player: Owner,
            filter: null,
            source: this)).FirstOrDefault();

        if (selected != null)
            await CardCmd.Exhaust(choiceContext, selected);

        await SpecimenActions.Gain(choiceContext, Owner.Creature, DynamicVars["Specimen"].IntValue, this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Specimen"].UpgradeValueBy(1m);
    }
}
