using BaseLib.Abstracts;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class ExperimentalDose : PlagueBringerCard, IPlagueCard
{
    public ExperimentalDose() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithVars(
            new DynamicVar("MaxSpecimens", 3),
            new PowerVar<PlaguePower>("PlaguePerSpecimen", 4));
    }

    public override List<(string, string)>? Localization =>
        new CardLoc(
            "Experimental Dose",
            "Spend up to {MaxSpecimens} Specimens. Apply {PlaguePerSpecimen:diff()} [gold]Plague[/gold] per Specimen spent."
        );

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var amountToSpend = Math.Min(
            DynamicVars["MaxSpecimens"].IntValue,
            SpecimenActions.Count(Owner.Creature));
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
