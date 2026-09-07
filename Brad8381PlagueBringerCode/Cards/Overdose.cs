using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class Overdose : PlagueBringerCard, IPlagueCard
{
    public Overdose() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(
            new PowerVar<PlaguePower>("Plague", 7),
            new DynamicVar("BonusPlague", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var alreadyInfected = PlagueCardUtils.GetPlague(target) > 0;
        var amount = DynamicVars["Plague"].IntValue +
                     (alreadyInfected ? DynamicVars["BonusPlague"].IntValue : 0);

        await PowerCmd.Apply<PlaguePower>(
            choiceContext,
            target,
            amount,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(2m);
        DynamicVars["BonusPlague"].UpgradeValueBy(1m);
    }
}
