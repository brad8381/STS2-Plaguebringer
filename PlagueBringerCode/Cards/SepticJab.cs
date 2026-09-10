using BaseLib.Utils;
using PB.Mechanics;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class SepticJab : PlagueBringerCard
{
    public SepticJab() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6);
        WithVars(new DynamicVar("Specimen", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var wasInfected = PlagueCardUtils.GetPlague(target) > 0;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        if (wasInfected)
        {
            await SpecimenActions.Gain(
                choiceContext,
                Owner.Creature,
                DynamicVars["Specimen"].IntValue,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
