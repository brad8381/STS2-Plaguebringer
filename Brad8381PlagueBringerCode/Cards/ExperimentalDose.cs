using BaseLib.Utils;
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
        WithVars(new PowerVar<PlaguePower>("PlaguePerSpecimen", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        var spent = await SpecimenActions.Spend(choiceContext, Owner.Creature, 3, this);
        if (spent <= 0) return;

        var plague = spent * DynamicVars["PlaguePerSpecimen"].IntValue;
        await PowerCmd.Apply<PlaguePower>(choiceContext, target, plague, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlaguePerSpecimen"].UpgradeValueBy(1m);
    }
}
