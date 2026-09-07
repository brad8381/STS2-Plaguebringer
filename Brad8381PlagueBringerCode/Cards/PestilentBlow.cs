using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PestilentBlow : PlagueBringerCard, IPlagueCard
{
    public PestilentBlow() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6);
        WithKeywords(CardKeyword.Exhaust);
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
        DynamicVars.Damage.UpgradeValueBy(3m);
    }
}
