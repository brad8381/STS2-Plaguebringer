using BaseLib.Utils;
using PB.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace PB.Cards;

public sealed class FinalIncision : PlagueBringerCard, IPlagueCard
{
    public FinalIncision() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(24);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive) return;

        var plague = target.GetPower<PlaguePower>();
        if (plague != null && plague.Amount > 0)
            await PowerCmd.ModifyAmount(choiceContext, plague, plague.Amount, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(6m);
    }
}
