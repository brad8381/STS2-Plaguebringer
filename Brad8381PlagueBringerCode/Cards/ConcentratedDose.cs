using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ConcentratedDose : PlagueBringerCard, IPlagueCard
{
    protected override bool HasEnergyCostX => true;

    public ConcentratedDose() : base(-1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(new DynamicVar("PlaguePerEnergy", 3));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        var x = ResolveEnergyXValue();
        if (x <= 0) return;

        var amount = x * DynamicVars["PlaguePerEnergy"].IntValue;
        await PowerCmd.Apply<PlaguePower>(choiceContext, target, amount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlaguePerEnergy"].UpgradeValueBy(1m);
    }
}
