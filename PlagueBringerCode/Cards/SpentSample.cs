using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class SpentSample : PlagueBringerCard, IPlagueCard
{
    public SpentSample() : base(0, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("PlagueCost", 4),
            new DynamicVar("Energy", 1));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var plagueCost = DynamicVars["PlagueCost"].IntValue;
        if (PlagueCardUtils.GetPlague(target) < plagueCost)
            return;

        var removed = await PlagueCardUtils.RemovePlague(choiceContext, target, plagueCost, this);
        if (removed == plagueCost)
            await PlayerCmd.GainEnergy(DynamicVars["Energy"].IntValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlagueCost"].UpgradeValueBy(-1m);
    }
}
