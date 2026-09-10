using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class PatientZero : PlagueBringerCard, IPlagueCard
{
    public PatientZero() : base(1, CardType.Skill, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithVars(new PowerVar<PlaguePower>("Plague", 8));
        WithKeywords(CardKeyword.Exhaust);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        await PowerCmd.Apply<PlaguePower>(
            choiceContext,
            target,
            DynamicVars["Plague"].IntValue,
            Owner.Creature,
            this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(3m);
    }
}
