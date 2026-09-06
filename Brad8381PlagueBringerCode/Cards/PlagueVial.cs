using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PlagueVial : PlagueBringerCard, IPlagueCard
{
    public PlagueVial() : base(1, CardType.Skill, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithVars(new PowerVar<PlaguePower>("Plague", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;
        await PowerCmd.Apply<PlaguePower>(choiceContext, target, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
