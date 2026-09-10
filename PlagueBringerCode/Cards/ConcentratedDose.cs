using BaseLib.Abstracts;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class ConcentratedDose : PlagueBringerCard, IPlagueCard
{
    protected override bool HasEnergyCostX => true;

    public ConcentratedDose() : base(-1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("PlaguePerEnergy", 4),
            new DynamicVar("EnergyRefund", 0));

        WithKeywords(CardKeyword.Exhaust);
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Concentrated Dose",
            "X times: Apply {PlaguePerEnergy:diff()} [gold]Plague[/gold].{IfUpgraded:show:\nGain 1 [gold]Energy[/gold].|}"
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var x = ResolveEnergyXValue();

        if (x <= 0)
            return;

        var plaguePerEnergy =
            DynamicVars["PlaguePerEnergy"].IntValue;

        for (var i = 0; i < x; i++)
        {
            if (!target.IsAlive)
                break;

            await PB.Mechanics.PlagueActions.Apply(
                choiceContext,
                target,
                plaguePerEnergy,
                Owner.Creature,
                this);
        }

        var refund =
            DynamicVars["EnergyRefund"].IntValue;

        if (refund > 0)
            await PlayerCmd.GainEnergy(refund, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlaguePerEnergy"].UpgradeValueBy(1m);
        DynamicVars["EnergyRefund"].UpgradeValueBy(1m);
    }
}