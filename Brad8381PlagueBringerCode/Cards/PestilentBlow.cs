using BaseLib.Utils;
using Brad8381PlagueBringer.Brad8381PlagueBringerCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace Brad8381PlagueBringer.Brad8381PlagueBringerCode.Cards;

public sealed class PestilentBlow : PlagueBringerCard, IPlagueCard
{
    private const decimal MultiplierFraction = 0.5m;

    public PestilentBlow() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6);
        // Generic DynamicVar formatting is integer-based, so keep the changing whole
        // number as the var and append the fixed .5 in localization.
        WithVars(new DynamicVar("MultiplierWhole", 1m));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay play)
    {
        if (play.Target is not { IsAlive: true } target)
            return;

        await CommonActions.CardAttack(this, play).Execute(choiceContext);
        if (!target.IsAlive)
            return;

        PlaguePower? plague = target.GetPower<PlaguePower>();
        if (plague == null || plague.Amount <= 0)
            return;

        decimal multiplier = DynamicVars["MultiplierWhole"].BaseValue + MultiplierFraction;
        int newAmount = (int)Math.Ceiling(plague.Amount * multiplier);
        int increase = newAmount - plague.Amount;

        if (increase > 0)
        {
            await PowerCmd.ModifyAmount(
                choiceContext,
                plague,
                increase,
                Owner.Creature,
                this);
        }
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3m);
        DynamicVars["MultiplierWhole"].UpgradeValueBy(1m);
    }
}
