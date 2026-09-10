using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class SepticLance : PlagueBringerCard, IPlagueCard
{
    public SepticLance() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(10);
        WithVars(new PowerVar<PlaguePower>("Plague", 5));
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
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
