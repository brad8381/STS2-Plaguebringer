using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class FleaBite : PlagueBringerCard, IPlagueCard
{
    public FleaBite() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(3);
        WithVars(new PowerVar<PlaguePower>("Plague", 1));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target) return;

        var wasInfected = (target.GetPower<PlaguePower>()?.Amount ?? 0m) > 0m;
        await CommonActions.CardAttack(this, play).Execute(choiceContext);

        if (wasInfected && target.IsAlive)
            await PowerCmd.Apply<PlaguePower>(choiceContext, target, DynamicVars["Plague"].IntValue, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}
