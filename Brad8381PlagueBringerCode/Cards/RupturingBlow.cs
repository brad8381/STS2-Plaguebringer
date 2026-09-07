using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class RupturingBlow : PlagueBringerCard, IPlagueCard
{
    public RupturingBlow() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(12);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;
        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive) return;

        var plague = target.GetPower<PlaguePower>();
        if (plague != null && plague.Amount > 0)
            await plague.TriggerPlague(choiceContext);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
