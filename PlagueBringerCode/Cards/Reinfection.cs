using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class Reinfection : PlagueBringerCard, IPlagueCard
{
    public Reinfection() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(new PowerVar<PlaguePower>("Plague", 5));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        if (PlagueCardUtils.GetPlague(target) <= 0)
        {
            await PowerCmd.Apply<PlaguePower>(
                choiceContext,
                target,
                DynamicVars["Plague"].IntValue,
                Owner.Creature,
                this);
            return;
        }

        await PlagueCardUtils.TriggerPlague(choiceContext, target);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(2m);
    }
}
