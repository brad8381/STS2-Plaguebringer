using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Mechanics;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class UnsteadyStep : PlagueBringerCard
{
    public UnsteadyStep() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithVars(new PowerVar<SwayPower>("Sway", 2), new CardsVar(1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        await SwayActions.Apply(
            choiceContext,
            target,
            DynamicVars["Sway"].IntValue,
            Owner.Creature,
            this);

        await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Sway"].UpgradeValueBy(1m);
    }
}
