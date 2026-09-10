using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class SepticCut : PlagueBringerCard, IPlagueCard
{
    public SepticCut() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(5);
        WithVars(new PowerVar<PlaguePower>("Plague", 2));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (target.IsAlive)
            await PB.Mechanics.PlagueActions.Apply(choiceContext, target, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
