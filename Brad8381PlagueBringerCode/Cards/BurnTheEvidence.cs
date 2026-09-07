using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class BurnTheEvidence : PlagueBringerCard, IPlagueCard
{
    public BurnTheEvidence() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithVars(new DynamicVar("Plague", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var hand = PileType.Hand.GetPile(Owner).Cards;
        if (!hand.Any(card => !ReferenceEquals(card, this)))
            return;

        var prompt = new LocString("gameplay_ui", "CHOOSE_CARD_HEADER");
        var prefs = new CardSelectorPrefs(prompt, 1);
        var selected = (await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                prefs,
                card => !ReferenceEquals(card, this),
                this))
            .FirstOrDefault();

        if (selected == null)
            return;

        await CardPileCmd.Add(selected, PileType.Exhaust);

        var combatState = CombatState;
        if (combatState != null)
        {
            await PlagueCardUtils.ApplyPlagueToAllEnemies(
                choiceContext,
                combatState,
                Owner.Creature,
                DynamicVars["Plague"].IntValue,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(2m);
    }
}
