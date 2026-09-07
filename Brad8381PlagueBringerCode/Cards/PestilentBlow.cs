using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PestilentBlow : PlagueBringerCard, IPlagueCard
{
    public PestilentBlow() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6);
        WithVars(new DynamicVar("Multiplier", 1.5m));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive) return;

        var plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0) return;

        var newAmount = (int)Math.Ceiling(plague.Amount * DynamicVars["Multiplier"].BaseValue);
        var increase = newAmount - plague.Amount;
        if (increase > 0)
            await PowerCmd.ModifyAmount(choiceContext, plague, increase, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["Multiplier"].UpgradeValueBy(1m);
    }
}
