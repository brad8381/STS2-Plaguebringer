using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace PB.Cards;

public sealed class CenserHook : PlagueBringerCard, IPlagueCard
{
    public CenserHook() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(5);
        WithVars(new PowerVar<PlaguePower>("Plague", 4));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        var infected = (target.GetPower<PlaguePower>()?.Amount ?? 0) > 0;
        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        if (infected && target.IsAlive)
        {
            await PowerCmd.Apply<PlaguePower>(
                choiceContext,
                target,
                DynamicVars["Plague"].IntValue,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
        DynamicVars["Plague"].UpgradeValueBy(1m);
    }
}
