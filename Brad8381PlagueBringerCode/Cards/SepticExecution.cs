using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class SepticExecution : PlagueBringerCard, IPlagueCard
{
    public SepticExecution() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithDamage(14);
        WithVars(new DynamicVar("Triggers", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;
        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        for (var i = 0; i < DynamicVars["Triggers"].IntValue && target.IsAlive; i++)
        {
            var plague = target.GetPower<PlaguePower>();
            if (plague == null || plague.Amount <= 0) break;
            await plague.TriggerPlague(choiceContext);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Triggers"].UpgradeValueBy(1m);
    }
}
