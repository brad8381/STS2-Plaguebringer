using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class Outbreak : PlagueBringerCard, IPlagueCard
{
    public Outbreak() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithVars(new DynamicVar("Triggers", 1));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        var combatState = CombatState;
        if (combatState == null)
            return;

        for (var i = 0; i < DynamicVars["Triggers"].IntValue; i++)
            await PlagueCardUtils.TriggerAllEnemies(choiceContext, combatState, Owner.Creature);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
        DynamicVars["Triggers"].UpgradeValueBy(1m);
    }
}
