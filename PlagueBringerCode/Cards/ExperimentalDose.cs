using BaseLib.Abstracts;
using PB.Mechanics;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class ExperimentalDose : PlagueBringerCard, IPlagueCard
{
    public ExperimentalDose() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(new PowerVar<PlaguePower>("PlaguePerSpecimen", 4));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Experimental Dose",
            "Spend ALL Specimens. Apply {PlaguePerSpecimen:diff()} [gold]Plague[/gold] per Specimen spent."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var amountToSpend = SpecimenActions.Count(Owner.Creature);
        if (amountToSpend <= 0)
            return;

        var spent = await SpecimenActions.Spend(choiceContext, Owner.Creature, amountToSpend, this);
        if (spent <= 0)
            return;

        await PowerCmd.Apply<PlaguePower>(
            choiceContext,
            target,
            spent * DynamicVars["PlaguePerSpecimen"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlaguePerSpecimen"].UpgradeValueBy(1m);
    }
}
