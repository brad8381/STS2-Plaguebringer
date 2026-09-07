using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class BloodSample : PlagueBringerCard
{
    public BloodSample() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithVars(new DynamicVar("Specimen", 1), new CardsVar(1));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Blood Sample",
            "Exhaust a card from your hand. Gain {Specimen:diff()} Specimen. Draw {Cards:diff()} card."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var hand = PileType.Hand.GetPile(Owner).Cards;
        if (!hand.Any(card => !ReferenceEquals(card, this)))
            return;

        var prompt = new LocString("gameplay_ui", "CHOOSE_CARD_HEADER");
        var prefs = new CardSelectorPrefs(prompt, 1);
        CardModel? selected = (await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                prefs,
                card => !ReferenceEquals(card, this),
                this))
            .FirstOrDefault();

        if (selected == null)
            return;

        await CardCmd.Exhaust(choiceContext, selected);
        await SpecimenActions.Gain(
            choiceContext,
            Owner.Creature,
            DynamicVars["Specimen"].IntValue,
            this);
        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Specimen"].UpgradeValueBy(1m);
    }
}
