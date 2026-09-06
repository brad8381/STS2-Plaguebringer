using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class InfectedScalpel : PlagueBringerCard, IPlagueCard
{
    public InfectedScalpel() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy)
    {
        WithDamage(3);
        WithVars(new PowerVar<PlaguePower>("Plague", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;
        await CommonActions.CardAttack(this, target).Execute(choiceContext);
        if (target.IsAlive)
            await PowerCmd.Apply<PlaguePower>(target, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
